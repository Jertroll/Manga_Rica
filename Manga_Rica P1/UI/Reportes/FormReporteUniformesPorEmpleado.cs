using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

using MangaRica.UI.Reports;               // HtmlReportRenderer
using Manga_Rica_P1.BLL;                  // ReportesUniformesEmpleadoService
using Manga_Rica_P1.DAL.Reports;          // UniformesEmpleadoReportRepository
using Manga_Rica_P1.UI.Reportes.Export;   // ReportExportHelper, ReportTableBuilder
using Manga_Rica_P1.Entity.Reports;       // ReporteUniformesEmpleadoVm, UniformeEmpleadoLineaVm
using Manga_Rica_P1.UI.Reportes.Shared;   // EmpleadoFiltroSidebar

namespace Manga_Rica_P1.UI.Reportes
{
    public partial class FormReporteUniformesPorEmpleado : Form
    {
        private readonly ReportesUniformesEmpleadoService _service;
        private readonly HtmlReportRenderer _renderer;
        private readonly string _assetsFolder;
        private readonly string _templatesFolder;
        private readonly string _virtualHost;

        private ReporteUniformesEmpleadoVm? _vm;
        private string _titulo = "Detalle_Uniformes_Empleado";

        public FormReporteUniformesPorEmpleado(string connectionString, string virtualHost = "appassets")
        {
            InitializeComponent();

            Text = "Reporte: Uniformes por Empleado";
            Width = 1100;
            Height = 700;

            // DAL + BLL específicos
            var repo = new UniformesEmpleadoReportRepository(connectionString);
            _service = new ReportesUniformesEmpleadoService(repo);

            // Carpetas de plantillas y assets
            _templatesFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Templates");
            _assetsFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Assets");

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

            // Configurar el sidebar
            empleadoFiltroSidebar1.EtiquetaCampo = "Carne:";
            empleadoFiltroSidebar1.TextoBoton = "Generar reporte";
            empleadoFiltroSidebar1.BuscarEmpleado += EmpleadoFiltroSidebar1_BuscarEmpleado;

            Load += Form_Load;
        }

        // Inicializar WebView2 pero sin cargar reporte todavía
        private async void Form_Load(object? sender, EventArgs e)
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

                // HTML inicial de ayuda
                var htmlInicio = @"
<html>
<head>
<meta charset='utf-8' />
<style>
 body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 13px; }
</style>
</head>
<body>
  <h3>Detalle de Uniformes por Empleado</h3>
  <p>Digite un carné en el panel izquierdo y presione <b>Generar reporte</b>.</p>
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

        // Cuando el usuario pulsa "Generar reporte" en el sidebar
        private async void EmpleadoFiltroSidebar1_BuscarEmpleado(
            object? sender,
            EmpleadoFiltroSidebar.BuscarEmpleadoEventArgs e)
        {
            if (e.CarneNumero is null)
            {
                MessageBox.Show(
                    "Por favor, digite un carné numérico válido.",
                    "Filtro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            await CargarReporteAsync(e.CarneNumero.Value);
        }

        private async Task CargarReporteAsync(long carne)
        {
            try
            {
                // 1) Obtener VM desde la BLL
                var vm = await _service.GetUniformesPorEmpleadoVmAsync(carne);

                // 2) Branding para el layout base
                var cfg = Manga_Rica_P1.Program.Configuration;
                var company = cfg?["Brand:Company"] ?? "Manga Rica S.A.";
                var phones = cfg?["Brand:Phones"] ?? "";
                var address = cfg?["Brand:Address"] ?? "";
                var logoFile = cfg?["Brand:LogoFile"] ?? "Manga Rica Logo.jpeg";

                // 3) Renderizar Razor
                var html = await _renderer.RenderAsync(
                    templateName: "UniformesPorEmpleado.cshtml",
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
                    ? $"Detalle_Uniformes_{vm.Carne}"
                    : vm.Titulo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al generar el reporte: {ex.Message}",
                    "Reporte de Uniformes por Empleado",
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
                    "Primero genere el reporte para algún carné.",
                    "Exportar Excel",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var vm = _vm;

            // Proyectar a filas para Excel
            var rows = vm.Lineas.Select(l => new Row
            {
                Carne = vm.Carne,
                Nombre = vm.NombreCompleto,
                Departamento = vm.Departamento,
                Fecha = l.Fecha,
                IdDeduccion = l.IdDeduccion,
                Codigo = l.CodigoArticulo,
                Descripcion = l.DescripcionArticulo,
                Cantidad = l.Cantidad,
                Precio = l.PrecioUnitario,
                Total = l.TotalLinea
            });

            var table = ReportTableBuilder.From(
                rows,
                new ReportTableBuilder.Column<Row>("Carne", r => r.Carne, typeof(string)),
                new ReportTableBuilder.Column<Row>("Nombre", r => r.Nombre, typeof(string)),
                new ReportTableBuilder.Column<Row>("Departamento", r => r.Departamento, typeof(string)),
                new ReportTableBuilder.Column<Row>("Fecha", r => r.Fecha, typeof(DateTime)),
                new ReportTableBuilder.Column<Row>("Deducción", r => r.IdDeduccion, typeof(long)),
                new ReportTableBuilder.Column<Row>("Código", r => r.Codigo, typeof(int)),
                new ReportTableBuilder.Column<Row>("Descripción", r => r.Descripcion, typeof(string)),
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
                groupByColumn: "Carne",                 // siempre el mismo empleado, pero deja la estructura
                subtotalColumns: new[] { "Total" },
                moneyColumns: new[] { "Precio", "Total" },
                decimalColumns: Array.Empty<string>(),
                logoHeightPx: 42
            );
        }

        // Filas para el Excel
        private sealed class Row
        {
            public string Carne { get; set; } = "";
            public string Nombre { get; set; } = "";
            public string Departamento { get; set; } = "";
            public DateTime Fecha { get; set; }
            public long IdDeduccion { get; set; }
            public int Codigo { get; set; }
            public string Descripcion { get; set; } = "";
            public int Cantidad { get; set; }
            public decimal Precio { get; set; }
            public decimal Total { get; set; }
        }
    }
}
