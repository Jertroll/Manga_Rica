using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

using Manga_Rica_P1.DAL.Reports;         // Repo
using MangaRica.BLL;                     // ReportesEmpleadoInactivoService
using MangaRica.UI.Reports;              // HtmlReportRenderer

namespace Manga_Rica_P1.UI.Reportes
{
    /// <summary>
    /// Nueva implementacion: Form para visualizar el reporte de Empleados NO Activos y exportar a PDF.
    /// </summary>
    public partial class FormReporteEmpleadosInactivos : Form
    {
        private readonly ReportesEmpleadoInactivoService _service;
        private readonly HtmlReportRenderer _renderer;
        private readonly string _assetsFolder;
        private readonly string _templatesFolder;
        private readonly string _virtualHost;

        // Nueva implementacion: recibe connectionString y opcionalmente el virtual host
        public FormReporteEmpleadosInactivos(string connectionString, string virtualHost = "appassets")
        {
            InitializeComponent();

            Text = "Reporte: Empleados No Activos";
            Width = 1100; Height = 700;

            // DAL + BLL
            var repo = new EmpleadosReportRepository(connectionString);
            _service = new ReportesEmpleadoInactivoService(repo);

            // Rutas
            _templatesFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Templates");
            _assetsFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Assets");

            // Fallback de desarrollo si no aparecen en bin
            if (!Directory.Exists(_templatesFolder))
            {
                var devRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
                var devTemplates = Path.Combine(devRoot, "UI", "Reportes", "Templates");
                if (Directory.Exists(devTemplates))
                    _templatesFolder = devTemplates;
            }

            if (!Directory.Exists(_templatesFolder))
            {
                throw new DirectoryNotFoundException(
                    $"No se encontraron las plantillas Razor en:\n{_templatesFolder}\n" +
                    "Verifica CopyToOutputDirectory en el .csproj para **UI\\Reportes\\Templates\\**\\*.cshtml**");
            }

            Directory.CreateDirectory(_assetsFolder); // por si no existe
            _virtualHost = virtualHost;

            _renderer = new HtmlReportRenderer(_templatesFolder);

            // Eventos
            _btnExportar.Click += BtnExportar_Click;
            Load += Form_Load;
        }

        // Nueva implementacion: render + WebView2
        private async void Form_Load(object? sender, EventArgs e)
        {
            try
            {
                await _web.EnsureCoreWebView2Async();

                _web.CoreWebView2.SetVirtualHostNameToFolderMapping(
                    _virtualHost,
                    _assetsFolder,
                    CoreWebView2HostResourceAccessKind.DenyCors);

                // ViewModel
                var vm = await _service.GetEmpleadosInactivosVmAsync();

                // Config appsettings
                var cfg = Manga_Rica_P1.Program.Configuration;
                var company = cfg?["Brand:Company"] ?? "Manga Rica S.A.";
                var phones = cfg?["Brand:Phones"] ?? "";
                var address = cfg?["Brand:Address"] ?? "";
                var logoFile = cfg?["Brand:LogoFile"] ?? "logo.png";

                // Render Razor
                var html = await _renderer.RenderAsync(
                    templateName: "EmpleadosInactivos.cshtml",
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

        // Nueva implementacion: exportación a PDF
        private async void BtnExportar_Click(object? sender, EventArgs e)
        {
            await EsperarCargaAsync();

            using var sfd = new SaveFileDialog { Filter = "PDF|*.pdf", FileName = "EmpleadosInactivos.pdf" };
            if (sfd.ShowDialog(this) != DialogResult.OK) return;

            var settings = _web.CoreWebView2.Environment.CreatePrintSettings();
            settings.ShouldPrintHeaderAndFooter = false;
            settings.ShouldPrintBackgrounds = true;
            settings.Orientation = CoreWebView2PrintOrientation.Portrait;
            settings.MarginTop = settings.MarginBottom = settings.MarginLeft = settings.MarginRight = 0.5;

            bool ok = await _web.CoreWebView2.PrintToPdfAsync(sfd.FileName, settings);
            MessageBox.Show(ok ? "PDF generado." : "No se pudo generar el PDF.");
        }


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

        private async Task ExportarPdfAsync()
        {
            await EnsureWebView2ReadyAsync();

            using var fbd = new FolderBrowserDialog
            {
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Description = "Selecciona la carpeta donde guardar el PDF"
            };
            if (fbd.ShowDialog(this) != DialogResult.OK) return;

            var filePath = Path.Combine(
                fbd.SelectedPath,
                SanitizeFileName($"{Text.Replace("Reporte: ", "")}_{DateTime.Now:yyyy-MM-dd_HH-mm}.pdf"));

            var settings = _web.CoreWebView2.Environment.CreatePrintSettings();
            settings.ShouldPrintHeaderAndFooter = false;
            settings.ShouldPrintBackgrounds = true;
            settings.Orientation = CoreWebView2PrintOrientation.Portrait;
            settings.MarginTop = settings.MarginBottom = settings.MarginLeft = settings.MarginRight = 0.5;

            bool ok = await _web.CoreWebView2.PrintToPdfAsync(filePath, settings);
            MessageBox.Show(ok ? $"PDF generado:\n{filePath}" : "No se pudo generar el PDF.",
                "Exportar", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        private async Task EnsureWebView2ReadyAsync()
        {
            await _web.EnsureCoreWebView2Async();
        }

        private static string SanitizeFileName(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars()) name = name.Replace(c, '_');
            return name;
        }

        private async void _btnExportar_Click(object sender, EventArgs e)
        {
            await ExportarPdfAsync();
        }

        private void _btnExportarExcel_Click(object sender, EventArgs e)
        {

        }
    }
}
