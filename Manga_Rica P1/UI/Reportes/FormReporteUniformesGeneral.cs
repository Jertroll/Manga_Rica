using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

using MangaRica.UI.Reports;               // HtmlReportRenderer
using MangaRica.BLL;                      // ReportesUniformesService  (ajusta si tu namespace es otro)
using Manga_Rica_P1.DAL.Reports;         // UniformesReportRepository
using Manga_Rica_P1.UI.Reportes.Export;  // ReportExportHelper, ReportTableBuilder
using Manga_Rica_P1.Entity.Reports;      // ReporteUniformesVm, UniformeEmpleadoVm, UniformeDetalleVm

namespace Manga_Rica_P1.UI.Reportes
{
    public partial class FormReporteUniformesGeneral : Form
    {
        private readonly ReportesUniformesService _service;
        private readonly HtmlReportRenderer _renderer;
        private readonly string _assetsFolder;
        private readonly string _templatesFolder;
        private readonly string _virtualHost;

        // Cache del VM para exportar sin volver a tocar BD
        private ReporteUniformesVm? _vm;
        private string _titulo = "Uniformes_General";

        public FormReporteUniformesGeneral(string connectionString, string virtualHost = "appassets")
        {
            InitializeComponent();

            Text = "Reporte: Uniformes General";
            Width = 1100;
            Height = 700;

            // DAL + BLL específicos del reporte
            var repo = new UniformesReportRepository(connectionString);
            _service = new ReportesUniformesService(repo);

            // Carpetas de plantillas y assets
            _templatesFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Templates");
            _assetsFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Assets");

            // Fallback cuando corres desde VS (por si no se copiaron al bin)
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
                    $"No se encontraron las plantillas Razor en:\n{_templatesFolder}\n\n" +
                    "Asegúrate de copiarlas al output en el .csproj:\n" +
                    "<Content Include=\"UI\\Reportes\\Templates\\**\\*.cshtml\">\n" +
                    "  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>\n</Content>");
            }

            Directory.CreateDirectory(_assetsFolder);

            _virtualHost = virtualHost;
            _renderer = new HtmlReportRenderer(_templatesFolder);

            // Suscribimos el evento Load para inicializar WebView2 y renderizar el reporte
            Load += Form_Load;
        }

        // ===== Carga inicial del reporte (WebView2 + Razor) =====
        private async void Form_Load(object? sender, EventArgs e)
        {
            try
            {
                // 1) Inicializar WebView2 y evitar modo oscuro forzado
                var opts = new CoreWebView2EnvironmentOptions
                {
                    AdditionalBrowserArguments =
                        "--disable-features=WebContentsForceDark --force-dark-mode=0"
                };
                var env = await CoreWebView2Environment.CreateAsync(null, null, opts);
                await _web.EnsureCoreWebView2Async(env);
                _web.DefaultBackgroundColor = System.Drawing.Color.White;

                // 2) Mapear assets locales a https://{_virtualHost}/...
                _web.CoreWebView2.SetVirtualHostNameToFolderMapping(
                    _virtualHost,
                    _assetsFolder,
                    CoreWebView2HostResourceAccessKind.DenyCors);

                // 3) Obtener el ViewModel desde la BLL
                ReporteUniformesVm vm = await _service.GetUniformesVmAsync();

                // 4) Datos de marca (para el layout _ReportBase.cshtml)
                var cfg = Manga_Rica_P1.Program.Configuration;
                var company = cfg?["Brand:Company"] ?? "Manga Rica S.A.";
                var phones = cfg?["Brand:Phones"] ?? "";
                var address = cfg?["Brand:Address"] ?? "";
                var logoFile = cfg?["Brand:LogoFile"] ?? "Manga Rica Logo.jpeg";

                // 5) Renderizar Razor → HTML
                var html = await _renderer.RenderAsync(
                    templateName: "UniformesGeneral.cshtml",
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

                // Guardar VM y título para exportar
                _vm = vm;
                _titulo = vm?.Titulo ?? _titulo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al generar el reporte de uniformes: {ex.Message}\n\n{ex.StackTrace}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ===== Exportar PDF (usa el HTML que ya está en WebView2) =====
        private async void _btnExportarPdf_Click(object? sender, EventArgs e)
        {
            await ReportExportHelper.ExportWebView2ToPdfAsync(
                this, _web, ReportExportHelper.SanitizeFileName(_titulo));
        }

        // ===== Exportar Excel (VM → filas → DataTable → XLSX/CSV) =====
        private async void _btnExportarExcel_Click(object? sender, EventArgs e)
        {
            // Si no tenemos cacheado el VM, lo pedimos otra vez
            var vm = _vm ?? await _service.GetUniformesVmAsync();

            var empleados = vm.Empleados ?? Enumerable.Empty<UniformeEmpleadoVm>();

            // 1) Proyectar VM a filas planas
            var rows = empleados
                .SelectMany(emp => (emp.Detalles ?? Enumerable.Empty<UniformeDetalleVm>())
                    .Select(det => new Row
                    {
                        Carne = emp.Carne,
                        Apellidos = emp.Apellidos,
                        Nombre = emp.Nombre,
                        Departamento = emp.Departamento,
                        Codigo = det.Codigo,
                        Descripcion = det.Descripcion,
                        Cantidad = det.Cantidad,
                        Precio = det.Precio,
                        Total = det.Total,
                        Saldo = det.SaldoDeduccion   // saldo de la deducción
                    }));

            // 2) Declarar columnas EXPLÍCITAS para el DataTable
            var table = ReportTableBuilder.From(
                rows,
                new ReportTableBuilder.Column<Row>("Carne", r => r.Carne, typeof(string)),
                new ReportTableBuilder.Column<Row>("Apellidos", r => r.Apellidos, typeof(string)),
                new ReportTableBuilder.Column<Row>("Nombre", r => r.Nombre, typeof(string)),
                new ReportTableBuilder.Column<Row>("Departamento", r => r.Departamento, typeof(string)),
                new ReportTableBuilder.Column<Row>("Código", r => r.Codigo, typeof(int)),
                new ReportTableBuilder.Column<Row>("Descripción", r => r.Descripcion, typeof(string)),
                new ReportTableBuilder.Column<Row>("Cantidad", r => r.Cantidad, typeof(int)),
                new ReportTableBuilder.Column<Row>("Precio", r => r.Precio, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("Total", r => r.Total, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("Saldo", r => r.Saldo, typeof(decimal))
            );

            // 3) Branding para la cabecera del Excel
            var cfg = Manga_Rica_P1.Program.Configuration;
            var company = cfg?["Brand:Company"] ?? string.Empty;
            var phones = cfg?["Brand:Phones"] ?? string.Empty;
            var address = cfg?["Brand:Address"] ?? string.Empty;
            var title = _titulo ?? "Reporte";
            var logoFile = cfg?["Brand:LogoFile"];
            var logoPath = !string.IsNullOrWhiteSpace(logoFile) && Path.IsPathRooted(logoFile)
                ? logoFile
                : Path.Combine(_assetsFolder, logoFile ?? string.Empty);
            var logo = File.Exists(logoPath) ? logoPath : null;

            // 4) Exportar con la plantilla genérica corporativa
            await ReportExportHelper.ExportStyledXlsxOrCsvAsync(
                owner: this,
                table: table,
                suggestedFileName: ReportExportHelper.SanitizeFileName(title),
                company: company,
                phones: phones,
                address: address,
                title: title,
                logoPath: logo,
                prependPhonesLabel: true,
                groupByColumn: "Carne",                 
                subtotalColumns: new[] { "Total", "Saldo" },
                moneyColumns: new[] { "Precio", "Total", "Saldo" },
                decimalColumns: Array.Empty<string>(),
                logoHeightPx: 42
            );
        }

        // Clase interna para tipar las filas del Excel
        private sealed class Row
        {
            public string Carne { get; set; } = "";
            public string Apellidos { get; set; } = "";
            public string Nombre { get; set; } = "";
            public string Departamento { get; set; } = "";
            public int Codigo { get; set; }
            public string Descripcion { get; set; } = "";
            public int Cantidad { get; set; }
            public decimal Precio { get; set; }
            public decimal Total { get; set; }
            public decimal Saldo { get; set; }
        }
    }
}
