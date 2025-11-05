// Nueva implementacion
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

using MangaRica.BLL;
using MangaRica.UI.Reports;
using Manga_Rica_P1.DAL.Reports;

namespace MangaRica.UI.Forms
{
    /// <summary>
    /// Nueva implementacion: Form para visualizar el reporte de Empleados Activos y exportar a PDF.
    /// </summary>
    public partial class FormReporteEmpleadosActivos : Form
    {
        private readonly ReportesEmpleadoActivoService _service;
        private readonly HtmlReportRenderer _renderer;
        private readonly string _assetsFolder;
        private readonly string _templatesFolder;
        private readonly string _virtualHost;

        // Nueva implementacion: recibe connectionString y opcionales (virtual host)
        public FormReporteEmpleadosActivos(string connectionString, string virtualHost = "appassets")
        {
            InitializeComponent();

            // Nueva implementacion: dimensiones y título por defecto
            Text = "Reporte: Empleados Activos";
            Width = 1100; Height = 700;

            // Nueva implementacion: BLL a partir del DAL de reportes
            var repo = new EmpleadosReportRepository(connectionString);
            _service = new ReportesEmpleadoActivoService(repo);

            // Nueva implementacion: rutas de plantillas y assets (en bin\...\UI\Reportes\...)
            _templatesFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Templates");
            _assetsFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Assets");

            // Nueva implementacion: fallback dev por si no se copiaron al bin (útil en debug)
            if (!Directory.Exists(_templatesFolder))
            {
                var devRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
                var devTemplates = Path.Combine(devRoot, "UI", "Reportes", "Templates");
                if (Directory.Exists(devTemplates))
                    _templatesFolder = devTemplates;
            }

            // Nueva implementacion: validación clara de templates
            if (!Directory.Exists(_templatesFolder))
            {
                throw new DirectoryNotFoundException(
                    $"No se encontraron las plantillas Razor en:\n{_templatesFolder}\n\n" +
                    "Asegúrate de copiarlas al output en el .csproj:\n" +
                    "<Content Include=\"UI\\Reportes\\Templates\\**\\*.cshtml\">\n  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>\n</Content>");
            }

            // Nueva implementacion: garantizar carpeta de assets
            Directory.CreateDirectory(_assetsFolder);

            // Nueva implementacion: host virtual (de appsettings o parámetro)
            _virtualHost = virtualHost;

            // Nueva implementacion: renderer con soporte de ViewBag y caché
            _renderer = new HtmlReportRenderer(_templatesFolder);

            // Nueva implementacion: eventos de UI
            _btnExportar.Click += BtnExportar_Click;
            Load += Form_Load;
        }

        // Nueva implementacion: carga HTML en WebView2 y mapea assets locales
        private async void Form_Load(object? sender, EventArgs e)
        {
            try
            {
                await _web.EnsureCoreWebView2Async();

                // Nueva implementacion: Mapear carpeta de assets a un host virtual: https://{_virtualHost}/...
                _web.CoreWebView2.SetVirtualHostNameToFolderMapping(
                    _virtualHost,
                    _assetsFolder,
                    CoreWebView2HostResourceAccessKind.DenyCors);

                // Nueva implementacion: ViewModel del reporte
                var vm = await _service.GetEmpleadosActivosVmAsync();

                // Nueva implementacion: leer Brand de appsettings.json (Program.Configuration)
                var cfg = Manga_Rica_P1.Program.Configuration; // Ajusta si tu Program está en otro namespace
                var company = cfg?["Brand:Company"] ?? "Manga Rica S.A.";
                var phones = cfg?["Brand:Phones"] ?? "";
                var address = cfg?["Brand:Address"] ?? "";
                var logoFile = cfg?["Brand:LogoFile"] ?? "logo.png";

                // Nueva implementacion: Render con ViewBag para el layout (_ReportBase.cshtml)
                var html = await _renderer.RenderAsync(
                    templateName: "EmpleadosActivos.cshtml",
                    model: vm,
                    title: vm.Titulo,
                    footer: vm.PieDePagina,
                    virtualHost: _virtualHost,
                    company: company,
                    phones: phones,
                    address: address,
                    logoFile: logoFile
                );

                _web.CoreWebView2.NavigateToString(html);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar reporte: {ex.Message}\n\n{ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Nueva implementacion: exportación a PDF con márgenes en pulgadas
        private async void BtnExportar_Click(object? sender, EventArgs e)
        {
            await EsperarCargaAsync();

            using var sfd = new SaveFileDialog { Filter = "PDF|*.pdf", FileName = "EmpleadosActivos.pdf" };
            if (sfd.ShowDialog(this) != DialogResult.OK) return;

            var settings = _web.CoreWebView2.Environment.CreatePrintSettings();
            settings.ShouldPrintHeaderAndFooter = false;
            settings.ShouldPrintBackgrounds = true;
            settings.Orientation = CoreWebView2PrintOrientation.Portrait;
            settings.MarginTop = settings.MarginBottom = settings.MarginLeft = settings.MarginRight = 0.5;

            bool ok = await _web.CoreWebView2.PrintToPdfAsync(sfd.FileName, settings);
            MessageBox.Show(ok ? "PDF generado." : "No se pudo generar el PDF.");
        }

        // Nueva implementacion: helper para esperar navegación terminada
        private Task EsperarCargaAsync()
        {
            var tcs = new TaskCompletionSource();
            void Handler(object? s, CoreWebView2NavigationCompletedEventArgs e)
            {
                _web.CoreWebView2.NavigationCompleted -= Handler;
                tcs.SetResult();
            }
            _web.CoreWebView2.NavigationCompleted += Handler;
            return tcs.Task;
        }
    }
}
