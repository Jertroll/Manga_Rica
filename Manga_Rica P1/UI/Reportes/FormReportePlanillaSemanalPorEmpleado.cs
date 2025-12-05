using Manga_Rica_P1.BLL;                  // ReportesPlanillaSemanalService, SemanasService
using Manga_Rica_P1.DAL;                  // SemanaRepository
using Manga_Rica_P1.DAL.Reports;          // PlanillaSemanalReportRepository
using Manga_Rica_P1.Entity.Reports;       // ReportePlanillaSemanalVm
using Manga_Rica_P1.UI.Reportes.Export;   // ReportExportHelper, ReportTableBuilder
using Manga_Rica_P1.UI.Reportes.Shared;   // SemanaEmpleadoFiltroSidebar
using MangaRica.UI.Reports;               // HtmlReportRenderer
using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes
{
    public partial class FormReportePlanillaSemanalPorEmpleado : Form
    {
        private readonly ReportesPlanillaSemanalService _service;
        private readonly SemanasService _semanasService;
        private readonly HtmlReportRenderer _renderer;

        private readonly string _assetsFolder;
        private readonly string _templatesFolder;
        private readonly string _virtualHost;

        private ReportePlanillaSemanalVm? _vm;
        private string _titulo = "Planilla_Semanal_Por_Empleado";

        public FormReportePlanillaSemanalPorEmpleado(
            string connectionString,
            string virtualHost = "appassets")
        {
            InitializeComponent();

            Text = "Reporte: Planilla Semanal por Empleado";
            Width = 1100;
            Height = 700;

            // ===== DAL + BLL específicos del reporte =====
            var repoPlanilla = new PlanillaSemanalReportRepository(connectionString);
            _service = new ReportesPlanillaSemanalService(repoPlanilla);

            var repoSemanas = new SemanaRepository(connectionString);
            _semanasService = new SemanasService(repoSemanas);

            // ===== Carpetas de plantillas y assets =====
            _templatesFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Templates");
            _assetsFolder = Path.Combine(AppContext.BaseDirectory, "UI", "Reportes", "Assets");

            // Fallback cuando corres en modo desarrollo (igual que otros reportes)
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

            // ===== Configurar sidebar de semana + cédula =====
            filtroSemanaEmpleado.Titulo = "Filtro Planilla por Empleado";
            filtroSemanaEmpleado.EtiquetaSemana = "Semana :";
            filtroSemanaEmpleado.EtiquetaCedula = "Cédula :";
            filtroSemanaEmpleado.TextoBoton = "Generar reporte";

            CargarSemanasEnSidebar();

            filtroSemanaEmpleado.GenerarReporte += FiltroSemanaEmpleado_GenerarReporte;

            // Inicializar WebView2 al cargar el form
            Load += FormReportePlanillaSemanalPorEmpleado_Load;
        }

        private void CargarSemanasEnSidebar()
        {
            try
            {
                var semanas = _semanasService.GetAllForCombo(); // Lista con propiedades Id, Semana (string o int)
                filtroSemanaEmpleado.DataSourceSemana = semanas;
                filtroSemanaEmpleado.DisplayMemberSemana = "Semana";
                filtroSemanaEmpleado.ValueMemberSemana = "Semana"; // se parsea a int en el control
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al cargar las semanas para el filtro:\n{ex.Message}",
                    "Planilla por Empleado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Inicializar WebView2 sin cargar reporte aún
        private async void FormReportePlanillaSemanalPorEmpleado_Load(object? sender, EventArgs e)
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
  <h3>Planilla Semanal por Empleado</h3>
  <p>Seleccione una <b>semana</b>, digite la <b>cédula</b> y presione <b>Generar reporte</b>.</p>
</body>
</html>";
                webView.CoreWebView2.NavigateToString(htmlInicio);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al inicializar el visor de reportes: {ex.Message}",
                    "Planilla por Empleado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Handler del sidebar: cuando el usuario pulsa "Generar"
        private async void FiltroSemanaEmpleado_GenerarReporte(
            object? sender,
            SemanaEmpleadoFiltroSidebar.GenerarReporteSemanaEmpleadoEventArgs e)
        {
            if (e.SemanaNumero is null)
            {
                MessageBox.Show(
                    "Debe seleccionar una semana válida.",
                    "Planilla por Empleado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(e.CedulaTexto))
            {
                MessageBox.Show(
                    "Debe digitar una cédula para filtrar al empleado.",
                    "Planilla por Empleado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            await CargarReporteAsync(e.SemanaNumero.Value, e.CedulaTexto.Trim());
        }

        private async Task CargarReporteAsync(int semana, string cedula)
        {
            try
            {
                // 1) Llamar al servicio SIN ConfigureAwait(false) para conservar el hilo de UI
                var vm = await _service.GenerarReportePorSemanaYEmpleadoAsync(semana, cedula);

                // 2) Branding para el layout base
                var cfg = Manga_Rica_P1.Program.Configuration;
                var company = cfg?["Brand:Company"] ?? "Manga Rica S.A.";
                var phones = cfg?["Brand:Phones"] ?? "";
                var address = cfg?["Brand:Address"] ?? "";
                var logoFile = cfg?["Brand:LogoFile"] ?? "Manga Rica Logo.jpeg";

                // 3) Renderizar la plantilla Razor (también SIN ConfigureAwait(false))
                var html = await _renderer.RenderAsync(
                    templateName: "PlanillaSemanalPorEmpleado.cshtml",
                    model: vm,
                    title: vm.Titulo,
                    footer: vm.PieDePagina,
                    virtualHost: _virtualHost,
                    company: company,
                    phones: phones,
                    address: address,
                    logoFile: logoFile
                );

                // 4) Navegar SIEMPRE en el hilo de UI
                if (webView != null && webView.CoreWebView2 != null)
                {
                    if (webView.InvokeRequired)
                    {
                        webView.Invoke(new Action(() =>
                            webView.CoreWebView2.NavigateToString(html)
                        ));
                    }
                    else
                    {
                        webView.CoreWebView2.NavigateToString(html);
                    }
                }

                _vm = vm;
                _titulo = string.IsNullOrWhiteSpace(vm.Titulo)
                    ? $"Planilla_Semanal_Empleado_{semana}_{cedula}"
                    : vm.Titulo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al generar el reporte de planilla semanal por empleado: {ex.Message}",
                    "Planilla por Empleado",
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
                    "Primero genere el reporte para alguna semana y cédula.",
                    "Exportar Excel",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var vm = _vm;

            // Proyección a filas para el DataTable
            var rows = vm.Lineas.Select(l => new Row
            {
                Semana = l.Semana,
                FechaInicio = l.FechaInicio,
                FechaFin = l.FechaFin,
                Departamento = l.Departamento,

                Carne = l.Carne,
                Cedula = l.Cedula,
                Empleado = l.Empleado,

                HorasNormales = l.HorasNormales,
                HorasExtras = l.HorasExtras,
                HorasDobles = l.HorasDobles,
                Feriados = l.Feriados,

                SalarioHora = l.SalarioHora,
                Soda = l.Soda,
                Uniforme = l.Uniforme,

                SalarioBruto = l.SalarioBruto,
                SalarioNeto = l.SalarioNeto
            });

            var table = ReportTableBuilder.From(
                rows,
                new ReportTableBuilder.Column<Row>("Semana", r => r.Semana, typeof(int)),
                new ReportTableBuilder.Column<Row>("FechaInicio", r => r.FechaInicio, typeof(DateTime)),
                new ReportTableBuilder.Column<Row>("FechaFin", r => r.FechaFin, typeof(DateTime)),
                new ReportTableBuilder.Column<Row>("Departamento", r => r.Departamento, typeof(string)),
                new ReportTableBuilder.Column<Row>("Carne", r => r.Carne, typeof(long)),
                new ReportTableBuilder.Column<Row>("Cedula", r => r.Cedula, typeof(string)),
                new ReportTableBuilder.Column<Row>("Empleado", r => r.Empleado, typeof(string)),
                new ReportTableBuilder.Column<Row>("HorasNormales", r => r.HorasNormales, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("HorasExtras", r => r.HorasExtras, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("HorasDobles", r => r.HorasDobles, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("Feriados", r => r.Feriados, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("SalarioHora", r => r.SalarioHora, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("Soda", r => r.Soda, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("Uniforme", r => r.Uniforme, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("SalarioBruto", r => r.SalarioBruto, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("SalarioNeto", r => r.SalarioNeto, typeof(decimal))
            );

            var cfg = Manga_Rica_P1.Program.Configuration;
            var company = cfg?["Brand:Company"] ?? string.Empty;
            var phones = cfg?["Brand:Phones"] ?? string.Empty;
            var address = cfg?["Brand:Address"] ?? string.Empty;
            var title = _titulo ?? "Planilla Semanal por Empleado";
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
                groupByColumn: "Departamento", // en este reporte normalmente será 1 depto, pero se mantiene
                subtotalColumns: new[] { "SalarioBruto", "Soda", "Uniforme", "SalarioNeto" },
                moneyColumns: new[]
                {
                    "SalarioHora", "Soda", "Uniforme",
                    "SalarioBruto", "SalarioNeto"
                },
                decimalColumns: new[]
                {
                    "HorasNormales", "HorasExtras", "HorasDobles", "Feriados"
                },
                logoHeightPx: 42
            );
        }

        // Clase auxiliar para proyectar al DataTable
        private sealed class Row
        {
            public int Semana { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFin { get; set; }

            public string Departamento { get; set; } = "";

            public long Carne { get; set; }
            public string Cedula { get; set; } = "";
            public string Empleado { get; set; } = "";

            public decimal HorasNormales { get; set; }
            public decimal HorasExtras { get; set; }
            public decimal HorasDobles { get; set; }
            public decimal Feriados { get; set; }

            public decimal SalarioHora { get; set; }
            public decimal Soda { get; set; }
            public decimal Uniforme { get; set; }

            public decimal SalarioBruto { get; set; }
            public decimal SalarioNeto { get; set; }
        }
    }
}
