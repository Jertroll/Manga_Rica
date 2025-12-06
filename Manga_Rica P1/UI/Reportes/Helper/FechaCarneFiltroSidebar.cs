using System;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes.Shared
{
    public partial class FechaCarneFiltroSidebar : UserControl
    {
        /// <summary>
        /// Evento que el formulario escuchará cuando se pulse "Generar reporte".
        /// Entrega la fecha (normalizada a Date) y el carné (long).
        /// </summary>
        public event EventHandler<GenerarReporteFechaCarneEventArgs>? GenerarReporte;

        public FechaCarneFiltroSidebar()
        {
            InitializeComponent();
            dtpFecha.Value = DateTime.Today;
        }

        // --- Propiedades de UI configurables desde el form --------------------

        public string Titulo
        {
            get => lblTitulo.Text;
            set => lblTitulo.Text = value;
        }

        public string EtiquetaFecha
        {
            get => lblFecha.Text;
            set => lblFecha.Text = value;
        }

        public string EtiquetaCarne
        {
            get => lblCarne.Text;
            set => lblCarne.Text = value;
        }

        public string TextoBoton
        {
            get => btnGenerar.Text;
            set => btnGenerar.Text = value;
        }

        public DateTime Fecha
        {
            get => dtpFecha.Value.Date;
            set => dtpFecha.Value = value;
        }

        /// <summary>
        /// Carné actual del textbox (null si no es un número válido).
        /// </summary>
        public long? Carne
        {
            get
            {
                var txt = txtCarne.Text.Trim();
                if (long.TryParse(txt, out var carne) && carne > 0)
                    return carne;
                return null;
            }
            set
            {
                txtCarne.Text = value?.ToString() ?? string.Empty;
            }
        }

        // --- Lógica del botón -------------------------------------------------

        private void btnGenerar_Click(object? sender, EventArgs e)
        {
            var fecha = dtpFecha.Value.Date;
            var textoCarne = txtCarne.Text.Trim();

            if (!long.TryParse(textoCarne, out var carne) || carne <= 0)
            {
                MessageBox.Show(
                    "Debe ingresar un carné válido (número entero mayor que cero).",
                    "Filtro por carnet",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCarne.Focus();
                txtCarne.SelectAll();
                return;
            }

            var args = new GenerarReporteFechaCarneEventArgs(fecha, carne);
            GenerarReporte?.Invoke(this, args);
        }

        // --- EventArgs para el filtro ----------------------------------------

        public sealed class GenerarReporteFechaCarneEventArgs : EventArgs
        {
            public DateTime Fecha { get; }
            public long Carne { get; }

            public GenerarReporteFechaCarneEventArgs(DateTime fecha, long carne)
            {
                Fecha = fecha;
                Carne = carne;
            }
        }


    }
}
