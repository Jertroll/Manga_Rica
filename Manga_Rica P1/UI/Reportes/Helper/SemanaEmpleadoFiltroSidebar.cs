using System;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes.Shared
{
    /// <summary>
    /// Sidebar de filtros para reportes por semana + cédula.
    /// </summary>
    public partial class SemanaEmpleadoFiltroSidebar : UserControl
    {
        /// <summary>
        /// Evento que el formulario escuchará cuando se pulse "Generar reporte".
        /// Entrega la semana seleccionada y el texto de cédula.
        /// </summary>
        public event EventHandler<GenerarReporteSemanaEmpleadoEventArgs>? GenerarReporte;

        /// <summary>Título del panel (barra verde superior).</summary>
        public string Titulo
        {
            get => lblTitulo.Text;
            set => lblTitulo.Text = value;
        }

        /// <summary>Texto de la etiqueta para la semana.</summary>
        public string EtiquetaSemana
        {
            get => lblSemana.Text;
            set => lblSemana.Text = value;
        }

        /// <summary>Texto de la etiqueta para la cédula.</summary>
        public string EtiquetaCedula
        {
            get => lblCarnet.Text;
            set => lblCarnet.Text = value;
        }

        /// <summary>Texto del botón principal.</summary>
        public string TextoBoton
        {
            get => btnGenerar.Text;
            set => btnGenerar.Text = value;
        }

        /// <summary>Texto actual en el campo de cédula.</summary>
        public string CedulaTexto
        {
            get => txtCedula.Text;
            set => txtCedula.Text = value;
        }

        /// <summary>
        /// DataSource del combo de semanas (igual patrón que otros sidebars).
        /// </summary>
        public object? DataSourceSemana
        {
            get => cboSemana.DataSource;
            set => cboSemana.DataSource = value;
        }

        public string DisplayMemberSemana
        {
            get => cboSemana.DisplayMember;
            set => cboSemana.DisplayMember = value;
        }

        public string ValueMemberSemana
        {
            get => cboSemana.ValueMember;
            set => cboSemana.ValueMember = value;
        }

        /// <summary>
        /// Devuelve la semana seleccionada como int? si se puede convertir.
        /// </summary>
        public int? SemanaSeleccionadaNumero
        {
            get
            {
                if (cboSemana.SelectedValue is int i)
                    return i;

                if (int.TryParse(cboSemana.SelectedValue?.ToString(), out var n))
                    return n;

                return null;
            }
        }

        public SemanaEmpleadoFiltroSidebar()
        {
            InitializeComponent();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            var semanaNum = SemanaSeleccionadaNumero;
            var cedula = txtCedula.Text.Trim();

            var args = new GenerarReporteSemanaEmpleadoEventArgs(semanaNum, cedula);
            GenerarReporte?.Invoke(this, args);
        }

        // Disparar también con Enter en el campo de cédula
        private void txtCedula_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnGenerar.PerformClick();
            }
        }

        /// <summary>
        /// Argumentos para el evento GenerarReporte: semana + cédula.
        /// </summary>
        public sealed class GenerarReporteSemanaEmpleadoEventArgs : EventArgs
        {
            public int? SemanaNumero { get; }
            public string CedulaTexto { get; }

            public GenerarReporteSemanaEmpleadoEventArgs(int? semanaNumero, string cedulaTexto)
            {
                SemanaNumero = semanaNumero;
                CedulaTexto = cedulaTexto;
            }
        }
    }
}
