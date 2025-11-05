// Nueva implementacion
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastReport;
using FastReport.Export.Html;
using FastReport.Export.PdfSimple;
using Microsoft.Web.WebView2.WinForms;

namespace Manga_Rica_P1.UI.Reportes
{
    public enum ViewerFormat { Html, Pdf } // se usa también por el caller

    public sealed partial class FrmReporteViewer : Form
    {
        private string? _currentPath;      // archivo temporal actualmente mostrado
        private Report? _sourceReport;     // referencia al Report a renderizar (opcional)
        private ViewerFormat _format = ViewerFormat.Html;

        // Pequeño timer para “de-bounce” del resize y evitar llamar FitToWidth muchas veces
        private System.Windows.Forms.Timer _resizeTimer;

        public FrmReporteViewer()
        {
            InitializeComponent();

            // Por si el diseñador no lo puso
            this.KeyPreview = true;

            // Ajuste al redimensionar ventana (con debounce)
            _resizeTimer = new System.Windows.Forms.Timer { Interval = 120 };
            _resizeTimer.Tick += async (s, e) =>
            {
                _resizeTimer.Stop();
                if (IsHtmlShown())
                    await FitToWidthAsync();
            };

            this.Resize += (s, e) =>
            {
                if (IsHtmlShown())
                    _resizeTimer.Start();
            };
        }

        // =================== API pública ===================

        /// <summary>
        /// Carga un Report (FastReport) y lo muestra exportado en el formato elegido.
        /// </summary>
        public async Task LoadFromReportAsync(Report report, ViewerFormat format = ViewerFormat.Html)
        {
            _sourceReport = report ?? throw new ArgumentNullException(nameof(report));
            _format = format;
            // Mantén sincronizado el combo con el formato pedido (0=HTML, 1=PDF)
            if (cmbFormato.Items.Count >= 2)
                cmbFormato.SelectedIndex = (format == ViewerFormat.Html) ? 0 : 1;

            await RegenerarAsync();
        }

        /// <summary>
        /// Muestra un archivo ya existente (html/pdf) en el visor.
        /// </summary>
        public async Task LoadFileAsync(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                throw new FileNotFoundException("No se encontró el archivo a cargar.", path);

            _sourceReport = null;
            _currentPath = path;
            await EnsureWebViewAsync();
            web.Source = new Uri(path);

            if (IsHtmlPath(path))
                await FitToWidthAsync();
            else
                web.ZoomFactor = 1.0; // PDF: zoom normal
        }

        // =================== Eventos de UI ===================

        private async void btnRegenerar_Click(object? sender, EventArgs e)
        {
            await RegenerarAsync();
        }

        private async void btnBuscar_Click(object? sender, EventArgs e)
        {
            await FindAsync(txtBuscar.Text, backward: false);
        }

        private async void btnPrev_Click(object? sender, EventArgs e)
        {
            await FindAsync(txtBuscar.Text, backward: true);
        }

        private async void btnNext_Click(object? sender, EventArgs e)
        {
            await FindAsync(txtBuscar.Text, backward: false);
        }

        // Atajos: F3 siguiente / Shift+F3 anterior (solo HTML)
        private async void FrmReporteViewer_KeyDown(object? sender, KeyEventArgs e)
        {
            if (!IsHtmlShown()) return;

            if (e.KeyCode == Keys.F3 && !e.Shift)
            {
                await FindAsync(txtBuscar.Text, backward: false);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F3 && e.Shift)
            {
                await FindAsync(txtBuscar.Text, backward: true);
                e.Handled = true;
            }
        }

        // =================== Lógica interna ===================

        private async Task RegenerarAsync()
        {
            // Leer formato de la UI (0 = HTML, 1 = PDF)
            _format = (cmbFormato.SelectedIndex == 0) ? ViewerFormat.Html : ViewerFormat.Pdf;

            if (_sourceReport is null)
            {
                // Si no hay Report (p.ej. abriste un archivo externo), no hay nada que regenerar
                return;
            }

            // Exportar a archivo temporal
            var ext = (_format == ViewerFormat.Html) ? "html" : "pdf";
            string tmp = Path.Combine(Path.GetTempPath(),
                $"Rep_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}.{ext}");

            try
            {
                _sourceReport.Prepare();

                if (_format == ViewerFormat.Html)
                {
                    using var html = new HTMLExport
                    {
                        SinglePage = false,
                        Navigator = true,
                        EmbedPictures = true
                    };
                    _sourceReport.Export(html, tmp);

                    // Inyectar un poco de CSS para “sticky header” (la primera fila de cada tabla)
                    try
                    {
                        var markup = File.ReadAllText(tmp);
                        markup = markup.Replace("</head>", @"
<style>
  /* Hace pegajosa la primera fila de cada tabla exportada por FastReport */
  table tr:first-child { position: sticky; top: 0; z-index: 3; background: #e6f4ea; }
  table { border-collapse: collapse; }
</style>
</head>");
                        File.WriteAllText(tmp, markup);
                    }
                    catch { /* no bloquear por errores de I/O */ }
                }
                else
                {
                    using var pdf = new PDFSimpleExport();
                    _sourceReport.Export(pdf, tmp);
                }

                _currentPath = tmp;

                await EnsureWebViewAsync();
                web.Source = new Uri(tmp);

                // Pequeña pausa para que la navegación cargue y podamos medir ancho
                await Task.Delay(60);

                if (IsHtmlShown())
                    await FitToWidthAsync();
                else
                    web.ZoomFactor = 1.0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al generar reporte",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task EnsureWebViewAsync()
        {
            if (web.CoreWebView2 is null)
                await web.EnsureCoreWebView2Async();
        }

        private bool IsHtmlShown() => IsHtmlPath(_currentPath);
        private static bool IsHtmlPath(string? p) =>
            p != null && p.EndsWith(".html", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Ajusta el zoom del WebView2 para que el contenido HTML quepa al ancho visible.
        /// </summary>
        private async Task FitToWidthAsync()
        {
            if (!IsHtmlShown()) return;

            await EnsureWebViewAsync();
            if (web.CoreWebView2 is null) return;

            try
            {
                // Ancho real del documento (HTML exportado por FastReport suele usar anchos fijos)
                var raw = await web.CoreWebView2.ExecuteScriptAsync(
                    "document.documentElement.scrollWidth.toString()");
                raw = raw?.Trim('"');

                if (double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var docWidth)
                    && docWidth > 0)
                {
                    // Ancho interno del control (viewport)
                    var viewWidth = Math.Max(1, web.ClientSize.Width);
                    var zoom = Math.Max(0.10, viewWidth / docWidth);
                    web.ZoomFactor = zoom;
                }
            }
            catch
            {
                // Si falla el script, ignora silenciosamente
            }
        }

        /// <summary>
        /// Búsqueda incremental dentro del HTML cargado usando window.find.
        /// (Solo HTML; en PDF usa Ctrl+F nativo del visor del navegador)
        /// </summary>
        private async Task FindAsync(string? term, bool backward)
        {
            if (!IsHtmlShown())
            {
                MessageBox.Show("La búsqueda integrada funciona en HTML. En PDF usa Ctrl+F del visor.",
                                "Buscar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            await EnsureWebViewAsync();
            if (web.CoreWebView2 is null) return;

            var safe = (term ?? string.Empty).Replace("\\", "\\\\").Replace("'", "\\'");
            var js = $"window.find('{safe}', false, {(backward ? "true" : "false")}, true, false, false, false);";
            await web.CoreWebView2.ExecuteScriptAsync(js);
        }

        // Limpia temporales al cerrar (opcional)
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_currentPath) && File.Exists(_currentPath))
                    File.Delete(_currentPath);
            }
            catch { /* ignorar */ }

            base.OnFormClosed(e);
        }
    }
}
