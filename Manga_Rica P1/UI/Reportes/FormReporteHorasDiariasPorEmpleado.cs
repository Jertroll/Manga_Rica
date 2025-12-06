using Manga_Rica_P1.BLL;                    // ReportesHorasDiariasService
using Manga_Rica_P1.DAL.Reports;           // HorasDiariasReportRepository
using Manga_Rica_P1.Entity.Reports;        // ReporteHorasDiariasVm
using Manga_Rica_P1.UI.Reportes.Export;    // ReportExportHelper, ReportTableBuilder
using Manga_Rica_P1.UI.Reportes.Shared;    // FechaCarneFiltroSidebar
using MangaRica.UI.Reports;                // HtmlReportRenderer
using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes
{
    public partial class FormReporteHorasDiariasPorEmpleado : Form
    {
        private readonly ReportesHorasDiariasService _service;
        private readonly HtmlReportRenderer _renderer;

        private readonly string _assetsFolder;
        private readonly string _templatesFolder;
        private readonly string _virtualHost;

        private ReporteHorasDiariasVm? _vm;
        private string _titulo = "Horas_Diarias_Por_Empleado";

        public FormReporteHorasDiariasPorEmpleado(
            string connectionString,
            string virtualHost = "appassets")
        {
            InitializeComponent();

            Text = "Reporte: Horas Diarias por Empleado";
            Width = 1100;
            Height = 700;

            // ===== DAL + BLL del reporte =====
            var repo = new HorasDiariasReportRepository(connectionString);
            _service = new ReportesHorasDiariasService(repo);

            // ===== Carpetas de plantillas y assets =====
            _templatesFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Templates");
            _assetsFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Assets");

            // Fallback para entorno de desarrollo (igual que en otros reportes)
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

            // ===== Configurar sidebar (fecha + carné) =====
            filtroFechaCarne.Titulo = "Filtro: Fecha y Carnet";
            filtroFechaCarne.EtiquetaFecha = "Fecha :";
            filtroFechaCarne.EtiquetaCarne = "Carnet :";
            filtroFechaCarne.TextoBoton = "Generar reporte";

            // >>> AQUÍ el evento correcto según tu UserControl <<<
            filtroFechaCarne.GenerarReporte += FiltroFechaCarne_GenerarReporte;

            // Inicializar WebView2 al cargar el form
            Load += FormReporteHorasDiariasPorEmpleado_Load;
        }

        // Inicializar WebView2 sin cargar reporte aún
        private async void FormReporteHorasDiariasPorEmpleado_Load(object? sender, EventArgs e)
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
  <h3>Horas Diarias por Empleado</h3>
  <p>Seleccione una <b>fecha</b> y un <b>carné</b> en el panel izquierdo y presione <b>Generar reporte</b>.</p>
</body>
</html>";

                webView.CoreWebView2.NavigateToString(htmlInicio);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al inicializar el visor de reportes: {ex.Message}",
                    "Horas Diarias por Empleado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Handler del sidebar: cuando el usuario pulsa "Generar"
        private async void FiltroFechaCarne_GenerarReporte(
            object? sender,
            FechaCarneFiltroSidebar.GenerarReporteFechaCarneEventArgs e)
        {
            // Aquí ya viene el carné validado (long > 0), gracias al UserControl
            await CargarReporteAsync(e.Fecha, e.Carne);
        }

        private async Task CargarReporteAsync(DateTime fecha, long carne)
        {
            try
            {
                // Usamos el método específico por empleado (debes tenerlo en el service)
                var vm = await _service.GenerarReportePorEmpleadoAsync(fecha, carne);

                // Branding para el layout base
                var cfg = Manga_Rica_P1.Program.Configuration;
                var company = cfg?["Brand:Company"] ?? "Manga Rica S.A.";
                var phones = cfg?["Brand:Phones"] ?? "";
                var address = cfg?["Brand:Address"] ?? "";
                var logoFile = cfg?["Brand:LogoFile"] ?? "Manga Rica Logo.jpeg";

                var html = await _renderer.RenderAsync(
                    templateName: "HorasDiarias.cshtml",  // reutilizamos la misma plantilla
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
                    ? $"Horas_Diarias_Empleado_{carne}_{fecha:yyyyMMdd}"
                    : vm.Titulo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al generar el reporte de Horas Diarias por Empleado:\n{ex.Message}",
                    "Horas Diarias por Empleado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
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
                    "Primero genere el reporte para alguna fecha y carné.",
                    "Exportar Excel",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var vm = _vm;

            // Proyección a filas para el DataTable
            var rows = vm.Lineas.Select(l => new Row
            {
                Fecha = vm.Fecha,
                Carne = l.Carne,
                Apellidos = l.Apellidos,
                Nombre = l.Nombre,
                HorasNormales = l.HorasNormales,
                HorasExtras = l.HorasExtras,
                HorasDobles = l.HorasDobles,
                HorasFeriado = l.HorasFeriado
            });

            var table = ReportTableBuilder.From(
                rows,
                new ReportTableBuilder.Column<Row>("Fecha", r => r.Fecha, typeof(DateTime)),
                new ReportTableBuilder.Column<Row>("Carne", r => r.Carne, typeof(long)),
                new ReportTableBuilder.Column<Row>("Apellidos", r => r.Apellidos, typeof(string)),
                new ReportTableBuilder.Column<Row>("Nombre", r => r.Nombre, typeof(string)),
                new ReportTableBuilder.Column<Row>("H.Normales", r => r.HorasNormales, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("H.Extras", r => r.HorasExtras, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("H.Dobles", r => r.HorasDobles, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("Feriado", r => r.HorasFeriado, typeof(decimal))
            );

            var cfg = Manga_Rica_P1.Program.Configuration;
            var company = cfg?["Brand:Company"] ?? string.Empty;
            var phones = cfg?["Brand:Phones"] ?? string.Empty;
            var address = cfg?["Brand:Address"] ?? string.Empty;
            var title = _titulo ?? "Horas Diarias por Empleado";
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
                groupByColumn: null, // no agrupamos por nada en este reporte
                subtotalColumns: Array.Empty<string>(),
                moneyColumns: Array.Empty<string>(),
                decimalColumns: new[] { "H.Normales", "H.Extras", "H.Dobles", "Feriado" },
                logoHeightPx: 42
            );
        }

        // Clase auxiliar para proyectar al DataTable (Excel)
        private sealed class Row
        {
            public DateTime Fecha { get; set; }
            public long Carne { get; set; }
            public string Apellidos { get; set; } = "";
            public string Nombre { get; set; } = "";

            public decimal HorasNormales { get; set; }
            public decimal HorasExtras { get; set; }
            public decimal HorasDobles { get; set; }
            public decimal HorasFeriado { get; set; }
        }
    }
}
