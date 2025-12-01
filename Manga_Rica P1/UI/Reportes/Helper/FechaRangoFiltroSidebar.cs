using System;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes.Shared
{
    public partial class FechaRangoFiltroSidebar : UserControl
    {
        /// <summary>
        /// Evento que el formulario escuchará cuando se pulse "Generar reporte".
        /// Entrega las fechas Desde y Hasta ya normalizadas a Date (sin hora).
        /// </summary>
        public event EventHandler<BuscarPorRangoFechasEventArgs>? BuscarPorRango;

        public string TextoBoton
        {
            get => btnGenerar.Text;
            set => btnGenerar.Text = value;
        }

        public DateTime FechaDesde
        {
            get => dtpDesde.Value.Date;
            set => dtpDesde.Value = value;
        }

        public DateTime FechaHasta
        {
            get => dtpHasta.Value.Date;
            set => dtpHasta.Value = value;
        }

        public FechaRangoFiltroSidebar()
        {
            InitializeComponent();
            // Por comodidad, iniciamos con el día de hoy en ambos
            dtpDesde.Value = DateTime.Today;
            dtpHasta.Value = DateTime.Today;
        }

        private void btnGenerar_Click(object? sender, EventArgs e)
        {
            var desde = dtpDesde.Value.Date;
            var hasta = dtpHasta.Value.Date;

            // Validación básica: no permitir rangos invertidos
            if (hasta < desde)
            {
                MessageBox.Show(
                    "La fecha 'Hasta' no puede ser menor que la fecha 'Desde'.",
                    "Rango de fechas inválido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var args = new BuscarPorRangoFechasEventArgs(desde, hasta);
            BuscarPorRango?.Invoke(this, args);
        }

        /// <summary>
        /// Argumentos del evento de búsqueda por rango de fechas.
        /// </summary>
        public sealed class BuscarPorRangoFechasEventArgs : EventArgs
        {
            public DateTime Desde { get; }
            public DateTime Hasta { get; }

            public BuscarPorRangoFechasEventArgs(DateTime desde, DateTime hasta)
            {
                Desde = desde;
                Hasta = hasta;
            }
        }
    }
}
