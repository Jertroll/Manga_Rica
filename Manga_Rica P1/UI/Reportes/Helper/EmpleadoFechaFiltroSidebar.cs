using System;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes.Shared
{
    public partial class EmpleadoFechaFiltroSidebar : UserControl
    {
        public event EventHandler<FiltroEmpleadoFechasEventArgs>? GenerarReporte;

        public string CarneTexto
        {
            get => txtCarne.Text;
            set => txtCarne.Text = value;
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

        public EmpleadoFechaFiltroSidebar()
        {
            InitializeComponent();
            dtpDesde.Value = DateTime.Today;
            dtpHasta.Value = DateTime.Today;
        }

        private void btnGenerar_Click(object? sender, EventArgs e)
        {
            LanzarEvento();
        }

        private void txtCarne_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                LanzarEvento();
            }
        }

        private void LanzarEvento()
        {
            var texto = CarneTexto.Trim();
            long? carneNum = null;

            if (!string.IsNullOrEmpty(texto) && long.TryParse(texto, out var n))
                carneNum = n;

            var args = new FiltroEmpleadoFechasEventArgs(
                texto,
                carneNum,
                FechaDesde,
                FechaHasta);

            GenerarReporte?.Invoke(this, args);
        }

        // Args fuertemente tipados para el filtro
        public sealed class FiltroEmpleadoFechasEventArgs : EventArgs
        {
            public string CarneTexto { get; }
            public long? CarneNumero { get; }
            public DateTime FechaDesde { get; }
            public DateTime FechaHasta { get; }

            public FiltroEmpleadoFechasEventArgs(
                string carneTexto,
                long? carneNumero,
                DateTime fechaDesde,
                DateTime fechaHasta)
            {
                CarneTexto = carneTexto;
                CarneNumero = carneNumero;
                FechaDesde = fechaDesde;
                FechaHasta = fechaHasta;
            }
        }
    }
}
