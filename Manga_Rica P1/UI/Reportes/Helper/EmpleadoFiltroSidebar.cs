using System;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes.Shared
{
    public partial class EmpleadoFiltroSidebar : UserControl
    {
        // Evento que el formulario escuchará cuando se pulse "Generar reporte"
        public event EventHandler<BuscarEmpleadoEventArgs>? BuscarEmpleado;

        public string EtiquetaCampo
        {
            get => lblCampo.Text;
            set => lblCampo.Text = value;
        }

        public string TextoBoton
        {
            get => btnGenerar.Text;
            set => btnGenerar.Text = value;
        }

        public string CarneTexto
        {
            get => txtCarne.Text;
            set => txtCarne.Text = value;
        }

        public EmpleadoFiltroSidebar()
        {
            InitializeComponent();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            var texto = txtCarne.Text.Trim();
            var args = new BuscarEmpleadoEventArgs(texto);
            BuscarEmpleado?.Invoke(this, args);
        }

        // Disparar con Enter también
        private void txtCarne_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnGenerar.PerformClick();
            }
        }

        public sealed class BuscarEmpleadoEventArgs : EventArgs
        {
            public string CarneTexto { get; }
            public long? CarneNumero { get; }

            public BuscarEmpleadoEventArgs(string carneTexto)
            {
                CarneTexto = carneTexto;
                if (long.TryParse(carneTexto, out var num))
                    CarneNumero = num;
            }
        }
    }
}
