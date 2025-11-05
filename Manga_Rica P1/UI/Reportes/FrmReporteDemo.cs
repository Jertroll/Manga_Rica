// UI/Reportes/FrmReporteDemo.cs
using FastReport;
using FastReport.Data;
using FastReport.Export.Html;                 // Nueva implementacion
using FastReport.Export.PdfSimple;
using FastReport.Utils;
using Manga_Rica_P1.BLL.Reportes;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes
{
    public partial class FrmReporteDemo : Form
    {
        private readonly ReporteEmpleadosDemoService _svc = new ReporteEmpleadosDemoService();
        private WebView2 _viewer;                 
        private string? _currentPath;             

        private ComboBox _cmbFormato;             
        private TextBox _txtBuscar;               
        private Button _btnBuscar;                

        public FrmReporteDemo()
        {
            InitializeComponent();
            this.Text = "Demo FastReport (Open Source) - WinForms .NET 8";
            this.Width = 1000;
            this.Height = 750;

            // ===== Barra superior =====
            var top = new Panel { Dock = DockStyle.Top, Height = 44, Padding = new Padding(8) };
            this.Controls.Add(top);

            var btnGenerar = new Button { Text = "Generar y ver", Width = 130, Dock = DockStyle.Left, Margin = new Padding(0, 0, 8, 0) };
            btnGenerar.Click += async (s, e) => await GenerarYVerAsync();                    // Nueva implementacion
            top.Controls.Add(btnGenerar);

            _cmbFormato = new ComboBox { Width = 100, Dock = DockStyle.Left };
            _cmbFormato.Items.AddRange(new object[] { "PDF", "HTML" });
            _cmbFormato.SelectedIndex = 0; // por defecto PDF
            top.Controls.Add(_cmbFormato);                                                   // Nueva implementacion

            // Controles de búsqueda (para HTML)
            _txtBuscar = new TextBox { PlaceholderText = "Buscar (solo HTML)...", Dock = DockStyle.Fill, Margin = new Padding(8, 0, 8, 0) };
            top.Controls.Add(_txtBuscar);                                                    // Nueva implementacion

            _btnBuscar = new Button { Text = "Buscar", Width = 90, Dock = DockStyle.Right };
            _btnBuscar.Click += async (s, e) => await BuscarEnHtmlAsync(_txtBuscar.Text);    // Nueva implementacion
            top.Controls.Add(_btnBuscar);

            // ===== Visor WebView2 =====
            _viewer = new WebView2 { Dock = DockStyle.Fill };
            this.Controls.Add(_viewer);
        }

        private async System.Threading.Tasks.Task GenerarYVerAsync()
        {
            try
            {
                var dt = _svc.ConstruirTablaDemo(120); // Nueva implementacion: 120 filas para probar paginado

                using var report = new Report();

                // Registrar datos tabulares
                report.RegisterData(dt, "Empleados");
                report.GetDataSource("Empleados").Enabled = true;

                // ===== Construir plantilla por código =====
                var page = new ReportPage();
                report.Pages.Add(page);
                page.CreateUniqueName();

                var pageHeader = new PageHeaderBand { Height = Units.Centimeters * 1.0f };
                page.Bands.Add(pageHeader);

                var titulo = new TextObject
                {
                    Bounds = new System.Drawing.RectangleF(0, 0, Units.Centimeters * 19.0f, Units.Centimeters * 1.0f),
                    Text = "Empleados Activos - Demo",
                    HorzAlign = HorzAlign.Center,
                    Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold)
                };
                pageHeader.Objects.Add(titulo);

                var header = new ColumnHeaderBand { Height = Units.Centimeters * 0.8f };
                page.Bands.Add(header);
                header.Objects.Add(MakeHeader("Id", 0.5f, 2.0f));
                header.Objects.Add(MakeHeader("Nombre", 2.7f, 7.0f));
                header.Objects.Add(MakeHeader("Departamento", 10.0f, 5.0f));
                header.Objects.Add(MakeHeader("Salario", 15.3f, 3.2f));

                var data = new DataBand
                {
                    DataSource = report.GetDataSource("Empleados"),
                    Height = Units.Centimeters * 0.7f
                };
                page.Bands.Add(data);

                data.Objects.Add(MakeCell("[Empleados.Id]", 0.5f, 2.0f));
                data.Objects.Add(MakeCell("[Empleados.Nombre]", 2.7f, 7.0f));
                data.Objects.Add(MakeCell("[Empleados.Departamento]", 10.0f, 5.0f));
                data.Objects.Add(MakeCell("[Empleados.Salario]", 15.3f, 3.2f, HorzAlign.Right));

                // Pie + total
                var footer = new ReportSummaryBand { Height = Units.Centimeters * 1.0f };
                page.Bands.Add(footer);

                var totalLbl = new TextObject
                {
                    Bounds = Cm(10.0f, 0.2f, 5.0f, 0.7f),
                    Text = "Total planilla:",
                    HorzAlign = HorzAlign.Right,
                    Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold)
                };
                footer.Objects.Add(totalLbl);

                var totalSalario = new Total
                {
                    Name = "TotalSalario",
                    Expression = "[Empleados.Salario]",
                    Evaluator = data,
                    TotalType = TotalType.Sum,
                    PrintOn = footer
                };
                report.Dictionary.Totals.Add(totalSalario);

                var totalVal = new TextObject
                {
                    Bounds = Cm(15.3f, 0.2f, 3.2f, 0.7f),
                    Text = "[TotalSalario]",
                    HorzAlign = HorzAlign.Right,
                    Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold)
                };
                footer.Objects.Add(totalVal);

                // ===== Preparar y exportar según formato =====
                report.Prepare();

                if (_cmbFormato.SelectedItem?.ToString() == "HTML")
                {
                    // Nueva implementacion: exportar a HTML (texto real, permite búsqueda)
                    var htmlPath = Path.Combine(Path.GetTempPath(), $"Empleados_{DateTime.Now:yyyyMMdd_HHmmss}.html");
                    using (var html = new HTMLExport { SinglePage = false, Navigator = true })
                        report.Export(html, htmlPath);

                    _currentPath = htmlPath;

                    if (_viewer.CoreWebView2 is null)
                        await _viewer.EnsureCoreWebView2Async();

                    _viewer.Source = new Uri(_currentPath);
                }
                else
                {
                    // PDF (PDFSimple = raster; no admite búsqueda de texto)
                    var pdfPath = Path.Combine(Path.GetTempPath(), $"Empleados_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
                    using (var pdf = new PDFSimpleExport())
                        report.Export(pdf, pdfPath);

                    _currentPath = pdfPath;

                    if (_viewer.CoreWebView2 is null)
                        await _viewer.EnsureCoreWebView2Async();

                    _viewer.Source = new Uri(_currentPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error generando reporte");
            }
        }

        /// <summary>
        /// Nueva implementacion:
        /// Búsqueda simple dentro del HTML cargado usando window.find.
        /// (Sólo funciona si el formato actual es HTML; en PDFSimple no hay texto real.)
        /// </summary>
        private async System.Threading.Tasks.Task BuscarEnHtmlAsync(string term)
        {
            if (_viewer.CoreWebView2 is null)
                await _viewer.EnsureCoreWebView2Async();

            // Si no estamos mostrando HTML, avisar.
            if (!(_currentPath?.EndsWith(".html", StringComparison.OrdinalIgnoreCase) ?? false))
            {
                MessageBox.Show("La búsqueda funciona cuando el formato es HTML. Cambia el selector a HTML y vuelve a generar.",
                                "Buscar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Escapar comillas simples para la cadena JS
            var safe = (term ?? string.Empty).Replace("\\", "\\\\").Replace("'", "\\'");

            // Ejecuta la búsqueda incremental (repite para encontrar la siguiente coincidencia)
            await _viewer.CoreWebView2.ExecuteScriptAsync($"window.find('{safe}', false, false, true, false, false, false);");
        }

        // ===== Helpers =====
        private static TextObject MakeHeader(string text, float xCm, float wCm) => new TextObject
        {
            Bounds = Cm(xCm, 0, wCm, 0.8f),
            Text = text,
            FillColor = System.Drawing.Color.FromArgb(230, 230, 230),
            Border = new Border { Lines = BorderLines.All },
            HorzAlign = HorzAlign.Center,
            VertAlign = VertAlign.Center,
            Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold)
        };

        private static TextObject MakeCell(string expr, float xCm, float wCm, HorzAlign align = HorzAlign.Left) => new TextObject
        {
            Bounds = Cm(xCm, 0, wCm, 0.7f),
            Text = expr,
            Border = new Border { Lines = BorderLines.Left | BorderLines.Right | BorderLines.Bottom },
            HorzAlign = align,
            VertAlign = VertAlign.Center,
            Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Regular)
        };

        private static System.Drawing.RectangleF Cm(float x, float y, float w, float h) =>
            new System.Drawing.RectangleF(Units.Centimeters * x, Units.Centimeters * y, Units.Centimeters * w, Units.Centimeters * h);
    }
}
