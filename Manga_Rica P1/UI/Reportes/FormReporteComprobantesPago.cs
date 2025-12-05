using Manga_Rica_P1.BLL;                  // SemanasService
using Manga_Rica_P1.BLL.Reportes;         // ComprobantePagoReportService
using Manga_Rica_P1.DAL;                  // SemanaRepository
using Manga_Rica_P1.DAL.Reports;          // ComprobantePagoReportRepository
using Manga_Rica_P1.Entity.Reports;       // ReporteComprobantesPagoVm
using Manga_Rica_P1.UI.Reportes.Export;   // ReportExportHelper, ReportTableBuilder
using Manga_Rica_P1.UI.Reportes.Shared;   // SemanaFiltroSidebar
using MangaRica.UI.Reports;               // HtmlReportRenderer
using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes
{
    public partial class FormReporteComprobantesPago : Form
    {
        // ===== Campos BLL/DAL específicos de este reporte =====
        private readonly ComprobantePagoReportService _service;
        private readonly SemanasService _semanasService;
        private readonly HtmlReportRenderer _renderer;

        // ===== Rutas y host virtual para assets y plantillas =====
        private readonly string _assetsFolder;
        private readonly string _templatesFolder;
        private readonly string _virtualHost;

        // ===== VM actual y título para exportar =====
        private ReporteComprobantesPagoVm? _vm;
        private string _titulo = "Comprobantes_Pago";

        public FormReporteComprobantesPago(string connectionString, string virtualHost = "appassets")
        {
            InitializeComponent();

            Text = "Reporte: Comprobantes de Pago";
            Width = 1100;
            Height = 700;

            // === DAL + BLL propios del reporte de comprobantes ===
            var repoComprobantes = new ComprobantePagoReportRepository(connectionString);
            _service = new ComprobantePagoReportService(repoComprobantes);

            // Reutilizamos SemanasService para llenar el combo de semanas
            var repoSemanas = new SemanaRepository(connectionString);
            _semanasService = new SemanasService(repoSemanas);

            // === Carpetas de plantillas y assets ===
            _templatesFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Templates");
            _assetsFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Assets");

            // Fallback para debug: buscar plantillas en la raíz del proyecto
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

            // === Configurar el sidebar reutilizable de semanas ===
            filtroSemana.Titulo = "Filtro de Semana";
            filtroSemana.EtiquetaCampo = "Semana :";
            filtroSemana.TextoBoton = "Generar comprobantes";

            CargarSemanasEnSidebar();

            // Suscribir el evento del sidebar
            filtroSemana.GenerarReporte += FiltroSemana_GenerarReporte;

            // Inicializar WebView2 al cargar el form
            Load += FormReporteComprobantesPago_Load;
        }

        /// <summary>
        /// Llena el combo del sidebar con las semanas disponibles
        /// usando el SemanasService existente.
        /// </summary>
        private void CargarSemanasEnSidebar()
        {
            try
            {
                var semanas = _semanasService.GetAllForCombo(); // Lista de SemanaItem (propiedad Semana)
                filtroSemana.DataSource = semanas;
                filtroSemana.DisplayMember = "Semana"; // texto visible
                filtroSemana.ValueMember = "Semana";   // valor que luego se parsea a int
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al cargar las semanas para el filtro:\n{ex.Message}",
                    "Comprobantes de Pago",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ===== Inicializar WebView2 y HTML inicial =====
        private async void FormReporteComprobantesPago_Load(object? sender, EventArgs e)
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
  <h3>Comprobantes de Pago</h3>
  <p>Seleccione una <b>semana</b> en el panel izquierdo y presione <b>Generar comprobantes</b>.</p>
</body>
</html>";
                webView.CoreWebView2.NavigateToString(htmlInicio);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al inicializar el visor de reportes: {ex.Message}",
                    "Comprobantes de Pago",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ===== Handler del sidebar: clic en "Generar comprobantes" =====
        private async void FiltroSemana_GenerarReporte(
            object? sender,
            SemanaFiltroSidebar.GenerarReporteSemanaEventArgs e)
        {
            if (e.SemanaNumero is null)
            {
                MessageBox.Show(
                    "Debe seleccionar una semana válida.",
                    "Comprobantes de Pago",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            await CargarReporteAsync(e.SemanaNumero.Value);
        }

        /// <summary>
        /// Llama al servicio de BLL para construir el VM y renderiza el HTML con Razor.
        /// </summary>
        private async Task CargarReporteAsync(int semana)
        {
            try
            {
                // 1. BLL construye el ViewModel de reportes
                var vm = await _service.BuildReporteAsync(semana);

                // 2. Branding para el layout base (igual que otros reportes)
                var cfg = Manga_Rica_P1.Program.Configuration;
                var company = cfg?["Brand:Company"] ?? "Manga Rica S.A.";
                var phones = cfg?["Brand:Phones"] ?? "";
                var address = cfg?["Brand:Address"] ?? "";
                var logoFile = cfg?["Brand:LogoFile"] ?? "Manga Rica Logo.jpeg";

                // 3. Renderizar el cshtml específico de comprobantes
                var html = await _renderer.RenderAsync(
                    templateName: "ReporteComprobantesPago.cshtml",
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
                    ? $"Comprobantes_Pago_Semana_{vm.Semana}"
                    : vm.Titulo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al generar el reporte de comprobantes: {ex.Message}",
                    "Comprobantes de Pago",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ===== Exportar a PDF usando el contenido de WebView2 =====
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

        // ===== Exportar a Excel a partir del ViewModel =====
        private async void btnExportExcel_Click(object? sender, EventArgs e)
        {
            if (_vm == null || _vm.Comprobantes == null || !_vm.Comprobantes.Any())
            {
                MessageBox.Show(
                    "Primero genere el reporte para alguna semana.",
                    "Exportar Excel",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var vm = _vm;

            // Proyectar cada comprobante a una fila plana para el DataTable
            var rows = vm.Comprobantes.Select(c => new Row
            {
                Semana = c.Semana,
                Fecha = c.Fecha,
                Carne = c.Carne,
                Cedula = c.Cedula,
                Nombre = c.Nombre,
                PrimerApellido = c.PrimerApellido,
                SegundoApellido = c.SegundoApellido,
                Departamento = c.Departamento,

                HorasNormales = c.HorasNormales,
                HorasExtras = c.HorasExtras,
                HorasDobles = c.HorasDobles,
                Feriados = c.Feriados,

                DeduccionSoda = c.DeduccionSoda,
                DeduccionUniforme = c.DeduccionUniforme,
                DeduccionOtras = c.DeduccionOtras,

                SalarioBruto = c.SalarioBruto,
                SalarioNeto = c.SalarioNeto
            });

            var table = ReportTableBuilder.From(
                rows,
                new ReportTableBuilder.Column<Row>("Semana", r => r.Semana, typeof(int)),
                new ReportTableBuilder.Column<Row>("Fecha", r => r.Fecha, typeof(DateTime)),
                new ReportTableBuilder.Column<Row>("Carne", r => r.Carne, typeof(long)),
                new ReportTableBuilder.Column<Row>("Cedula", r => r.Cedula, typeof(string)),
                new ReportTableBuilder.Column<Row>("Nombre", r => r.Nombre, typeof(string)),
                new ReportTableBuilder.Column<Row>("Apellido1", r => r.PrimerApellido, typeof(string)),
                new ReportTableBuilder.Column<Row>("Apellido2", r => r.SegundoApellido, typeof(string)),
                new ReportTableBuilder.Column<Row>("Departamento", r => r.Departamento, typeof(string)),

                new ReportTableBuilder.Column<Row>("H.Normales", r => r.HorasNormales, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("H.Extras", r => r.HorasExtras, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("H.Dobles", r => r.HorasDobles, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("Feriados", r => r.Feriados, typeof(decimal)),

                new ReportTableBuilder.Column<Row>("Soda", r => r.DeduccionSoda, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("Uniforme", r => r.DeduccionUniforme, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("Otras", r => r.DeduccionOtras, typeof(decimal)),

                new ReportTableBuilder.Column<Row>("Bruto", r => r.SalarioBruto, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("Neto", r => r.SalarioNeto, typeof(decimal))
            );

            var cfg = Manga_Rica_P1.Program.Configuration;
            var company = cfg?["Brand:Company"] ?? string.Empty;
            var phones = cfg?["Brand:Phones"] ?? string.Empty;
            var address = cfg?["Brand:Address"] ?? string.Empty;
            var title = _titulo ?? "Comprobantes de Pago";
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
                groupByColumn: "Departamento",
                subtotalColumns: new[] { "Bruto", "Neto", "Soda", "Uniforme", "Otras" },
                moneyColumns: new[] { "Soda", "Uniforme", "Otras", "Bruto", "Neto" },
                decimalColumns: new[] { "H.Normales", "H.Extras", "H.Dobles", "Feriados" },
                logoHeightPx: 42
            );
        }

        // Clase auxiliar para el DataTable (Excel)
        private sealed class Row
        {
            public int Semana { get; set; }
            public DateTime Fecha { get; set; }

            public long Carne { get; set; }
            public string Cedula { get; set; } = string.Empty;
            public string Nombre { get; set; } = string.Empty;
            public string PrimerApellido { get; set; } = string.Empty;
            public string SegundoApellido { get; set; } = string.Empty;
            public string Departamento { get; set; } = string.Empty;

            public decimal HorasNormales { get; set; }
            public decimal HorasExtras { get; set; }
            public decimal HorasDobles { get; set; }
            public decimal Feriados { get; set; }

            public decimal DeduccionSoda { get; set; }
            public decimal DeduccionUniforme { get; set; }
            public decimal DeduccionOtras { get; set; }

            public decimal SalarioBruto { get; set; }
            public decimal SalarioNeto { get; set; }
        }
    }
}
