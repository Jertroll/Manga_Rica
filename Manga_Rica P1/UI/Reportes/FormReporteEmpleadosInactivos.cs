using Manga_Rica_P1.DAL.Reports;          // EmpleadosReportRepository
using Manga_Rica_P1.UI.Reportes.Export;   // ReportExportHelper, ReportTableBuilder
using MangaRica.BLL;                      // ReportesEmpleadoInactivoService
using MangaRica.UI.Reports;               // HtmlReportRenderer
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MangaRica.ENTITY.ViewModels.Reports; // ReporteEmpleadosListaVm, DepartamentoGrupoVm, EmpleadoItemVm

namespace Manga_Rica_P1.UI.Reportes
{
    /// <summary>
    /// Form para visualizar el reporte de Empleados NO Activos y exportar a PDF/Excel.
    /// </summary>
    public partial class FormReporteEmpleadosInactivos : Form
    {
        private readonly ReportesEmpleadoInactivoService _service;
        private readonly HtmlReportRenderer _renderer;
        private readonly string _assetsFolder;
        private readonly string _templatesFolder;
        private readonly string _virtualHost;

        // Cache para exportaciones
        private ReporteEmpleadosListaVm? _vm;
        private string _titulo = "Empleados_Inactivos";

        public FormReporteEmpleadosInactivos(string connectionString, string virtualHost = "appassets")
        {
            InitializeComponent();

            Text = "Reporte: Empleados No Activos";
            Width = 1100;
            Height = 700;

            var repo = new EmpleadosReportRepository(connectionString);
            _service = new ReportesEmpleadoInactivoService(repo);

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
                    "Asegúrate de copiarlas al output en el .csproj:\n" +
                    "<Content Include=\"UI\\Reportes\\Templates\\**\\*.cshtml\">\n" +
                    "  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>\n</Content>");
            }

            Directory.CreateDirectory(_assetsFolder);
            _virtualHost = virtualHost;

            _renderer = new HtmlReportRenderer(_templatesFolder);

            Load += Form_Load;
        }

        // Render + WebView2
        private async void Form_Load(object? sender, EventArgs e)
        {
            try
            {
                var opts = new CoreWebView2EnvironmentOptions
                {
                    AdditionalBrowserArguments = "--disable-features=WebContentsForceDark --force-dark-mode=0"
                };
                var env = await CoreWebView2Environment.CreateAsync(null, null, opts);
                await _web.EnsureCoreWebView2Async(env);
                _web.DefaultBackgroundColor = System.Drawing.Color.White;

                _web.CoreWebView2.SetVirtualHostNameToFolderMapping(
                    _virtualHost, _assetsFolder, CoreWebView2HostResourceAccessKind.DenyCors);

                // VM de inactivos (tipo LISTA)
                ReporteEmpleadosListaVm vm = await _service.GetEmpleadosInactivosVmAsync();

                var cfg = Manga_Rica_P1.Program.Configuration;
                var company = cfg?["Brand:Company"] ?? "Manga Rica S.A.";
                var phones = cfg?["Brand:Phones"] ?? "";
                var address = cfg?["Brand:Address"] ?? "";
                var logoFile = cfg?["Brand:LogoFile"] ?? "logo.png";

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

                // Cache para exportación
                _vm = vm;
                _titulo = vm?.Titulo ?? _titulo;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar reporte: {ex.Message}\n\n{ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== Exportar PDF =====
        private async void _btnExportar_Click(object? sender, EventArgs e)
        {
            await ReportExportHelper.ExportWebView2ToPdfAsync(
                this, _web, ReportExportHelper.SanitizeFileName(_titulo));
        }

        // ===== Exportar Excel (VM -> DataTable -> XLSX/CSV) =====
        private async void _btnExportarExcel_Click(object? sender, EventArgs e)
        {
            // Unificar tipos: ambos son ReporteEmpleadosListaVm
            var vm = _vm ?? await _service.GetEmpleadosInactivosVmAsync();

            // 1) Filas tipadas a partir del VM (null-safe)
            var departamentos = vm.Departamentos ?? Enumerable.Empty<DepartamentoGrupoVm>();
            var rows = departamentos
                .SelectMany(d => (d.Empleados ?? Enumerable.Empty<EmpleadoItemVm>())
                    .Select(e => new Row
                    {
                        Departamento = d.Nombre ?? string.Empty,
                        Carne = e.Carne,
                        Apellido1 = e.Apellido1,
                        Apellido2 = e.Apellido2,
                        Nombre = e.Nombre,
                        Salario = e.Salario,
                        Puesto = e.Puesto,
                        FechaIngreso = e.FechaIngreso  // si tu VM lo tiene nullable, este Row también lo admite
                    }));

            // 2) Declarar columnas explícitas (nombres y tipos EXACTOS)
            var table = ReportTableBuilder.From(
                rows,
                new ReportTableBuilder.Column<Row>("Departamento", r => r.Departamento, typeof(string)),
                new ReportTableBuilder.Column<Row>("Carne", r => r.Carne, typeof(string)),
                new ReportTableBuilder.Column<Row>("Apellido 1", r => r.Apellido1, typeof(string)),
                new ReportTableBuilder.Column<Row>("Apellido 2", r => r.Apellido2, typeof(string)),
                new ReportTableBuilder.Column<Row>("Nombre", r => r.Nombre, typeof(string)),
                new ReportTableBuilder.Column<Row>("Salario", r => r.Salario, typeof(decimal)),
                new ReportTableBuilder.Column<Row>("Puesto", r => r.Puesto, typeof(string)),
                new ReportTableBuilder.Column<Row>("Fecha ingreso", r => r.FechaIngreso, typeof(DateTime))
            );

            // 3) Branding + logo
            var cfg = Manga_Rica_P1.Program.Configuration;
            var company = cfg["Brand:Company"] ?? string.Empty;
            var phones = cfg["Brand:Phones"] ?? string.Empty;
            var address = cfg["Brand:Address"] ?? string.Empty;
            var title = _titulo ?? "Reporte";
            var logoFile = cfg["Brand:LogoFile"];
            var logoPath = !string.IsNullOrWhiteSpace(logoFile) && Path.IsPathRooted(logoFile)
                            ? logoFile
                            : Path.Combine(_assetsFolder, logoFile ?? string.Empty);
            var logo = File.Exists(logoPath) ? logoPath : null;

            // 4) Exportar con plantilla genérica
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
                groupByColumn: "Departamento",          // agrupa por Departamento
                subtotalColumns: new[] { "Salario" },   // suma Salario por grupo y total
                moneyColumns: new[] { "Salario" },      // formato ₡ para Salario
                decimalColumns: Array.Empty<string>(),  // sin columnas decimales adicionales
                logoHeightPx: 42                        // alto del logo en px
            );
        }

        // POCO para tipar las filas de Excel
        private sealed class Row
        {
            public string Departamento { get; set; } = "";
            public string Carne { get; set; } = "";
            public string Apellido1 { get; set; } = "";
            public string Apellido2 { get; set; } = "";
            public string Nombre { get; set; } = "";
            public decimal Salario { get; set; }
            public string Puesto { get; set; } = "";
            public DateTime? FechaIngreso { get; set; }
        }

        private void _web_Click(object sender, EventArgs e) { }
    }
}
