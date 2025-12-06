using System;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes.Shared
{
    /// <summary>
    /// Sidebar simple con un DateTimePicker para seleccionar UN día
    /// y un botón para generar el reporte.
    /// </summary>
    public partial class FechaFiltroSidebar : UserControl
    {
        /// <summary>
        /// Evento que el formulario escuchará cuando se pulse "Generar".
        /// Entrega la fecha ya normalizada a Date (sin hora).
        /// </summary>
        public event EventHandler<BuscarPorFechaEventArgs>? BuscarPorFecha;

        public string Titulo
        {
            get => lblTitulo.Text;
            set => lblTitulo.Text = value;
        }

        public string EtiquetaCampo
        {
            get => lblFecha.Text;
            set => lblFecha.Text = value;
        }

        public string TextoBoton
        {
            get => btnGenerar.Text;
            set => btnGenerar.Text = value;
        }

        /// <summary>
        /// Fecha seleccionada en el control (solo parte de día).
        /// </summary>
        public DateTime Fecha
        {
            get => dtpFecha.Value.Date;
            set => dtpFecha.Value = value;
        }

        public FechaFiltroSidebar()
        {
            InitializeComponent();
            dtpFecha.Value = DateTime.Today;
        }

        private void btnGenerar_Click(object? sender, EventArgs e)
        {
            var fecha = dtpFecha.Value.Date;
            var args = new BuscarPorFechaEventArgs(fecha);
            BuscarPorFecha?.Invoke(this, args);
        }

        /// <summary>
        /// Argumentos del evento de búsqueda por fecha.
        /// </summary>
        public sealed class BuscarPorFechaEventArgs : EventArgs
        {
            public DateTime Fecha { get; }

            public BuscarPorFechaEventArgs(DateTime fecha)
            {
                Fecha = fecha.Date;
            }
        }
    }
}
