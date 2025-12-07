using Manga_Rica_P1.BLL;                     // ReportesEntradasSalidasService
using Manga_Rica_P1.Entity.Reports;         // ReporteEntradasSalidasVm
using Manga_Rica_P1.UI.Reportes.Export;     // ReportExportHelper, ReportTableBuilder
using Manga_Rica_P1.UI.Reportes.Shared;     // EmpleadoFechaFiltroSidebar
using MangaRica.UI.Reports;                 // HtmlReportRenderer
using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes
{
    public partial class FormReporteEntradasSalidas : Form
    {
        private readonly ReportesEntradasSalidasService _service;
        private readonly HtmlReportRenderer _renderer;

        private readonly string _assetsFolder;
        private readonly string _templatesFolder;
        private readonly string _virtualHost;

        private ReporteEntradasSalidasVm? _vm;
        private string _titulo = "Entradas_Salidas";

        public FormReporteEntradasSalidas(
            ReportesEntradasSalidasService service,
            string virtualHost = "appassets")
        {
            if (service is null) throw new ArgumentNullException(nameof(service));
            if (virtualHost is null) throw new ArgumentNullException(nameof(virtualHost));

            InitializeComponent();

            _service = service;
            _virtualHost = virtualHost;

            Text = "Reporte: Entradas y Salidas";
            Width = 1100;
            Height = 700;

            // ===== Carpetas de plantillas y assets =====
            _templatesFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Templates");
            _assetsFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Assets");

            // Fallback para entorno de desarrollo
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

            _renderer = new HtmlReportRenderer(_templatesFolder);

            // ===== Configurar sidebar (rango fechas + carné opcional) =====
            empleadoFechaFiltro.GenerarReporte += EmpleadoFechaFiltro_GenerarReporte;

            // Inicializar WebView2 al cargar el form
            Load += FormReporteEntradasSalidas_Load;
        }

        // Inicializar WebView2 sin cargar reporte aún
        private async void FormReporteEntradasSalidas_Load(object? sender, EventArgs e)
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

                // HTML inicial
                var htmlInicio = @"
<html>
<head>
<meta charset='utf-8' />
<style>
 body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 13px; }
</style>
</head>
<body>
  <h3>Entradas y Salidas</h3>
  <p>Seleccione un <b>rango de fechas</b> y, opcionalmente, un <b>carné</b> en el panel izquierdo y presione <b>Generar reporte</b>.</p>
  <p>Si deja el carné vacío, se mostrará el reporte para todos los empleados.</p>
</body>
</html>";
                webView.CoreWebView2.NavigateToString(htmlInicio);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al inicializar el visor de reportes: {ex.Message}",
                    "Entradas y Salidas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Handler del sidebar: cuando el usuario pulsa "Generar" o Enter
        // Handler del sidebar: cuando el usuario pulsa "Generar" o Enter
        private async void EmpleadoFechaFiltro_GenerarReporte(
            object? sender,
            EmpleadoFechaFiltroSidebar.FiltroEmpleadoFechasEventArgs e)
        {
            if (e.FechaHasta < e.FechaDesde)
            {
                MessageBox.Show(
                    "La fecha final no puede ser menor que la fecha inicial.",
                    "Entradas y Salidas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Normalizamos el texto del carné
                var carneTexto = (e.CarneTexto ?? string.Empty).Trim();

                // 👉 Regla nueva:
                //  - Vacío  => reporte general
                //  - "0"    => también reporte general
                //  - Otro   => reporte por carné
                if (!string.IsNullOrEmpty(carneTexto) && carneTexto != "0")
                {
                    await CargarReportePorEmpleadoAsync(e.FechaDesde, e.FechaHasta, carneTexto);
                }
                else
                {
                    await CargarReporteAsync(e.FechaDesde, e.FechaHasta);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al generar el reporte de Entradas y Salidas:\n{ex.Message}",
                    "Entradas y Salidas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private async Task CargarReporteAsync(DateTime desde, DateTime hasta)
        {
            var vm = await _service.GenerarReporteAsync(desde, hasta);

            await RenderVmAsync(
                vm,
                string.IsNullOrWhiteSpace(vm.Titulo)
                    ? $"Entradas_Salidas_{desde:yyyyMMdd}_{hasta:yyyyMMdd}"
                    : vm.Titulo);
        }

        // Firma y uso con string
        private async Task CargarReportePorEmpleadoAsync(DateTime desde, DateTime hasta, string carne)
        {
            var vm = await _service.GenerarReportePorEmpleadoAsync(desde, hasta, carne);

            await RenderVmAsync(
                vm,
                string.IsNullOrWhiteSpace(vm.Titulo)
                    ? $"Entradas_Salidas_{carne}_{desde:yyyyMMdd}_{hasta:yyyyMMdd}"
                    : vm.Titulo);
        }

        private async Task RenderVmAsync(ReporteEntradasSalidasVm vm, string tituloSugerido)
        {
            var cfg = Program.Configuration;
            var company = cfg?["Brand:Company"] ?? "Manga Rica S.A.";
            var phones = cfg?["Brand:Phones"] ?? "";
            var address = cfg?["Brand:Address"] ?? "";
            var logoFile = cfg?["Brand:LogoFile"] ?? "Manga Rica Logo.jpeg";

            var html = await _renderer.RenderAsync(
                templateName: "EntradasSalidas.cshtml",
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
            _titulo = tituloSugerido;
        }

        // ===== Exportar PDF =====
        private async void btnExportPdf_Click(object? sender, EventArgs e)
        {
            if (webView?.CoreWebView2 == null)
            {
                MessageBox.Show(
                    "El visor aún no está listo.",
                    "Exportar PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
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
                    "Primero genere el reporte para algún rango de fechas.",
                    "Exportar Excel",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var vm = _vm;

            // Proyección a filas para el DataTable
            var rows = vm.Lineas.Select(l => new Row
            {
                Fecha = l.Fecha,
                Carne = l.Carne,
                Apellidos = l.Apellidos,
                Nombre = l.Nombre,
                HoraEntrada = l.HoraEntrada?.ToString("HH:mm"),
                HoraSalida = l.HoraSalida?.ToString("HH:mm"),
                TotalHoras = l.TotalHoras
            });

            var table = ReportTableBuilder.From(
                rows,
                new ReportTableBuilder.Column<Row>("Fecha", r => r.Fecha, typeof(DateTime)),
                new ReportTableBuilder.Column<Row>("Carne", r => r.Carne, typeof(string)),
                new ReportTableBuilder.Column<Row>("Apellidos", r => r.Apellidos, typeof(string)),
                new ReportTableBuilder.Column<Row>("Nombre", r => r.Nombre, typeof(string)),
                new ReportTableBuilder.Column<Row>("Entrada", r => r.HoraEntrada, typeof(string)),
                new ReportTableBuilder.Column<Row>("Salida", r => r.HoraSalida, typeof(string)),
                new ReportTableBuilder.Column<Row>("TotalHoras", r => r.TotalHoras, typeof(decimal))
            );

            var cfg = Program.Configuration;
            var company = cfg?["Brand:Company"] ?? string.Empty;
            var phones = cfg?["Brand:Phones"] ?? string.Empty;
            var address = cfg?["Brand:Address"] ?? string.Empty;
            var title = _titulo ?? "Entradas y Salidas";

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
                groupByColumn: null,               // sin agrupaciones
                subtotalColumns: Array.Empty<string>(),
                moneyColumns: Array.Empty<string>(),
                decimalColumns: new[] { "TotalHoras" },
                logoHeightPx: 42
            );
        }

        // Clase auxiliar para proyectar al DataTable (Excel)
        private sealed class Row
        {
            public DateTime Fecha { get; set; }
            public string Carne { get; set; } = "";
            public string Apellidos { get; set; } = "";
            public string Nombre { get; set; } = "";
            public string? HoraEntrada { get; set; }
            public string? HoraSalida { get; set; }
            public decimal TotalHoras { get; set; }
        }
    }
}
