using Manga_Rica_P1.BLL;                  // ReportesSodaEmpleadoService
using Manga_Rica_P1.DAL.Reports;          // SodaEmpleadoReportRepository
using Manga_Rica_P1.Entity.Reports;       // ReporteSodaEmpleadoVm
using Manga_Rica_P1.UI.Reportes.Export;   // ReportExportHelper, ReportTableBuilder
using Manga_Rica_P1.UI.Reportes.Shared;   // EmpleadoFechaFiltroSidebar
using MangaRica.UI.Reports;               // HtmlReportRenderer
using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes
{
    public partial class FormReporteSodaPorEmpleado : Form
    {
        private readonly ReportesSodaEmpleadoService _service;
        private readonly HtmlReportRenderer _renderer;
        private readonly string _assetsFolder;
        private readonly string _templatesFolder;
        private readonly string _virtualHost;

        private ReporteSodaEmpleadoVm? _vm;
        private string _titulo = "Detalle_Soda_Por_Empleado";

        public FormReporteSodaPorEmpleado(string connectionString, string virtualHost = "appassets")
        {
            InitializeComponent();

            Text = "Reporte: Soda por Empleado";
            Width = 1100;
            Height = 700;

            // DAL + BLL específicos
            var repo = new SodaEmpleadoReportRepository(connectionString);
            _service = new ReportesSodaEmpleadoService(repo);

            // Carpetas de plantillas y assets
            _templatesFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Templates");
            _assetsFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Assets");

            // Fallback cuando corres en modo desarrollo (igual que Soda General)
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
                    "Asegúrate de copiarlas al output en el .csproj.");
            }

            Directory.CreateDirectory(_assetsFolder);

            _virtualHost = virtualHost;
            _renderer = new HtmlReportRenderer(_templatesFolder);

            // Configurar el sidebar de empleado+fechas
            filtroEmpleadoFechas.GenerarReporte += FiltroEmpleadoFechas_GenerarReporte;

            Load += FormReporteSodaPorEmpleado_Load;
        }

        // Inicializar WebView2 sin cargar reporte aún
        private async void FormReporteSodaPorEmpleado_Load(object? sender, EventArgs e)
        {
            try
            {
                var opts = new CoreWebView2EnvironmentOptions
                {
                    AdditionalBrowserArguments =
                        "--disable-features=WebContentsForceDark --force-dark-mode=0"
                };
                var env = await CoreWebView2Environment.CreateAsync(null, null, opts);
                await webView.EnsureCoreWebView2Async(env);
                webView.DefaultBackgroundColor = System.Drawing.Color.White;

                webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                    _virtualHost,
                    _assetsFolder,
                    CoreWebView2HostResourceAccessKind.DenyCors);

                var htmlInicio = @"
<html>
<head>
<meta charset='utf-8' />
<style>
 body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 13px; }
</style>
</head>
<body>
  <h3>Detalle de Soda por Empleado</h3>
  <p>Seleccione un <b>carné</b> y un <b>rango de fechas</b> en el panel izquierdo y presione <b>Generar reporte</b>.</p>
</body>
</html>";
                webView.CoreWebView2.NavigateToString(htmlInicio);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al inicializar el visor de reportes: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Al hacer clic en "Generar reporte" del sidebar
        private async void FiltroEmpleadoFechas_GenerarReporte(
            object? sender,
            EmpleadoFechaFiltroSidebar.FiltroEmpleadoFechasEventArgs e)
        {
            if (e.CarneNumero is null)
            {
                MessageBox.Show(
                    "Digite un carné numérico válido.",
                    "Reporte de Soda",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            await CargarReporteAsync(e.CarneNumero.Value, e.FechaDesde, e.FechaHasta);
        }

        private async Task CargarReporteAsync(long carne, DateTime desde, DateTime hasta)
        {
            try
            {
                var vm = await _service.GenerarReporteAsync(carne, desde, hasta);

                // Branding para el layout base
                var cfg = Manga_Rica_P1.Program.Configuration;
                var company = cfg?["Brand:Company"] ?? "Manga Rica S.A.";
                var phones = cfg?["Brand:Phones"] ?? "";
                var address = cfg?["Brand:Address"] ?? "";
                var logoFile = cfg?["Brand:LogoFile"] ?? "Manga Rica Logo.jpeg";

                var html = await _renderer.RenderAsync(
                    templateName: "SodaEmpleado.cshtml",
                    model: vm,
                    title: vm.Titulo,
                    footer: vm.PieDePagina,
                    virtualHost: _virtualHost,
                    company: company,
                    phones: phones,
                    address: address,
                    logoFile: logoFile
                );

                webView.CoreWebView2.NavigateToString(html);

                _vm = vm;
                _titulo = string.IsNullOrWhiteSpace(vm.Titulo)
                    ? $"Detalle_Soda_{carne}_{vm.FechaInicio:yyyyMMdd}_{vm.FechaFin:yyyyMMdd}"
                    : vm.Titulo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al generar el reporte: {ex.Message}",
                    "Reporte de Soda",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ===== Exportar PDF =====
        private async void btnExportPdf_Click(object? sender, EventArgs e)
        {
            if (webView?.CoreWebView2 == null)
            {
                MessageBox.Show("El visor aún no está listo.", "Exportar PDF",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await ReportExportHelper.ExportWebView2ToPdfAsync(
                this,
                webView,
                ReportExportHelper.SanitizeFileName(_titulo));
        }

        // ===== Exportar Excel =====
        private async void btnExportExcel_Click(object? sender, EventArgs e)
        {
            if (_vm == null || _vm.Lineas == null || !_vm.Lineas.Any())
            {
                MessageBox.Show(
                    "Primero genere el reporte para un carné y rango de fechas.",
                    "Exportar Excel",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var vm = _vm;

            // Para Excel repetimos Carne / Nombre en cada fila usando el encabezado del VM.
            var carneStr = vm.Carne.ToString();
            var nombreStr = vm.Nombre;

            var rows = vm.Lineas.Select(l => new Row
            {
                Factura = l.Factura,
                Fecha = l.Fecha,
                Carne = carneStr,
                Nombre = nombreStr,
                Descripcion = l.Descripcion,
                Cantidad = l.Cantidad,
                Precio = l.Precio,
                Total = l.Total
            });

            var table = ReportTableBuilder.From(
                rows,
                new ReportTableBuilder.Column<Row>("Factura", r => r.Factura, typeof(long)),
                new ReportTableBuilder.Column<Row>("Fecha", r => r.Fecha, typeof(DateTime)),
                new ReportTableBuilder.Column<Row>("Carne", r => r.Carne, typeof(string)),
                new ReportTableBuilder.Column<Row>("Nombre", r => r.Nombre, typeof(string)),
                new ReportTableBuilder.Column<Row>("Descripcion", r => r.Descripcion, typeof(string)),
                new ReportTableBuilder.Column<Row>("Cantidad", r => r.Cantidad, typeof(int)),
                new ReportTableBuilder.Column<Row>("Precio", r => r.Precio, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("Total", r => r.Total, typeof(decimal))
            );

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
                groupByColumn: "Fecha",                 // opcional
                subtotalColumns: new[] { "Total" },
                moneyColumns: new[] { "Precio", "Total" },
                decimalColumns: Array.Empty<string>(),
                logoHeightPx: 42
            );
        }

        // Clase auxiliar para proyectar al DataTable
        private sealed class Row
        {
            public long Factura { get; set; }
            public DateTime Fecha { get; set; }
            public string Carne { get; set; } = "";
            public string Nombre { get; set; } = "";
            public string Descripcion { get; set; } = "";
            public int Cantidad { get; set; }
            public decimal Precio { get; set; }
            public decimal Total { get; set; }
        }
    }
}
