using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

using MangaRica.UI.Reports;               // HtmlReportRenderer
using Manga_Rica_P1.BLL;                  // ReportesSodaGeneralService
using Manga_Rica_P1.DAL.Reports;          // SodaGeneralReportRepository
using Manga_Rica_P1.Entity.Reports;       // ReporteSodaGeneralVm, SodaLineaVm
using Manga_Rica_P1.UI.Reportes.Export;   // ReportExportHelper, ReportTableBuilder
using Manga_Rica_P1.UI.Reportes.Shared;   // FechaRangoFiltroSidebar

namespace Manga_Rica_P1.UI.Reportes
{
    public partial class FormReporteSodaGeneral : Form
    {
        private readonly ReportesSodaGeneralService _service;
        private readonly HtmlReportRenderer _renderer;
        private readonly string _assetsFolder;
        private readonly string _templatesFolder;
        private readonly string _virtualHost;

        private ReporteSodaGeneralVm? _vm;
        private string _titulo = "Detalle_Soda_General";

        public FormReporteSodaGeneral(string connectionString, string virtualHost = "appassets")
        {
            InitializeComponent();

            Text = "Reporte: Soda General";
            Width = 1100;
            Height = 700;

            // DAL + BLL específicos
            var repo = new SodaGeneralReportRepository(connectionString);
            _service = new ReportesSodaGeneralService(repo);

            // Carpetas de plantillas y assets
            _templatesFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Templates");
            _assetsFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Assets");

            // Fallback cuando corres en modo desarrollo
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

            // Configurar el sidebar de fechas
            fechaRangoFiltroSidebar1.TextoBoton = "Generar reporte";
            fechaRangoFiltroSidebar1.BuscarPorRango += FechaRangoFiltroSidebar1_BuscarPorRango;

            Load += FormReporteSodaGeneral_Load;
        }

        // Inicializar WebView2 sin cargar reporte aún
        private async void FormReporteSodaGeneral_Load(object? sender, EventArgs e)
        {
            try
            {
                var opts = new CoreWebView2EnvironmentOptions
                {
                    AdditionalBrowserArguments =
                        "--disable-features=WebContentsForceDark --force-dark-mode=0"
                };
                var env = await CoreWebView2Environment.CreateAsync(null, null, opts);
                await _web.EnsureCoreWebView2Async(env);
                _web.DefaultBackgroundColor = System.Drawing.Color.White;

                _web.CoreWebView2.SetVirtualHostNameToFolderMapping(
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
  <h3>Detalle de Soda</h3>
  <p>Seleccione un <b>rango de fechas</b> en el panel izquierdo y presione <b>Generar reporte</b>.</p>
</body>
</html>";
                _web.CoreWebView2.NavigateToString(htmlInicio);
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
        private async void FechaRangoFiltroSidebar1_BuscarPorRango(
            object? sender,
            FechaRangoFiltroSidebar.BuscarPorRangoFechasEventArgs e)
        {
            await CargarReporteAsync(e.Desde, e.Hasta);
        }

        private async Task CargarReporteAsync(DateTime desde, DateTime hasta)
        {
            try
            {
                var vm = await _service.GetSodaGeneralVmAsync(desde, hasta);

                // Branding para el layout base
                var cfg = Manga_Rica_P1.Program.Configuration;
                var company = cfg?["Brand:Company"] ?? "Manga Rica S.A.";
                var phones = cfg?["Brand:Phones"] ?? "";
                var address = cfg?["Brand:Address"] ?? "";
                var logoFile = cfg?["Brand:LogoFile"] ?? "Manga Rica Logo.jpeg";

                var html = await _renderer.RenderAsync(
                    templateName: "SodaGeneral.cshtml",
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

                _vm = vm;
                _titulo = string.IsNullOrWhiteSpace(vm.Titulo)
                    ? $"Detalle_Soda_{vm.FechaInicio:yyyyMMdd}_{vm.FechaFin:yyyyMMdd}"
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
        private async void _btnExportarPdf_Click(object? sender, EventArgs e)
        {
            if (_web?.CoreWebView2 == null)
            {
                MessageBox.Show("El visor aún no está listo.", "Exportar PDF",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await ReportExportHelper.ExportWebView2ToPdfAsync(
                this, _web, ReportExportHelper.SanitizeFileName(_titulo));
        }

        // ===== Exportar Excel =====
        private async void _btnExportarExcel_Click(object? sender, EventArgs e)
        {
            if (_vm == null || _vm.Lineas == null || !_vm.Lineas.Any())
            {
                MessageBox.Show(
                    "Primero genere el reporte para un rango de fechas.",
                    "Exportar Excel",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var vm = _vm;

            // Proyección a filas para el DataTable
            var rows = vm.Lineas.Select(l => new Row
            {
                Factura = l.Factura,
                Fecha = l.Fecha,
                Carne = l.Carne,
                Nombre = l.Nombre,
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
                groupByColumn: "Fecha",                 // podrías agrupar por fecha si quieres
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
