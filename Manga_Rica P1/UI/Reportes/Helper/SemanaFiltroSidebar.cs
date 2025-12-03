using System;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes.Shared
{
    /// <summary>
    /// Sidebar para seleccionar una semana y disparar la generación del reporte.
    /// El formulario será el encargado de llenar el ComboBox con las semanas disponibles.
    /// </summary>
    public partial class SemanaFiltroSidebar : UserControl
    {
        /// <summary>
        /// Evento que se dispara al hacer clic en "Generar".
        /// El form recibirá el texto y el número de semana (si se pudo parsear).
        /// </summary>
        public event EventHandler<GenerarReporteSemanaEventArgs>? GenerarReporte;

        public string Titulo
        {
            get => lblTitulo.Text;
            set => lblTitulo.Text = value;
        }

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

        public object? DataSource
        {
            get => cboSemana.DataSource;
            set => cboSemana.DataSource = value;
        }

  
        public string DisplayMember
        {
            get => cboSemana.DisplayMember;
            set => cboSemana.DisplayMember = value;
        }

        public string ValueMember
        {
            get => cboSemana.ValueMember;
            set => cboSemana.ValueMember = value;
        }


        public object? SelectedValue
        {
            get => cboSemana.SelectedValue;
            set => cboSemana.SelectedValue = value;
        }

        /// <summary>
        /// Texto mostrado actualmente en el ComboBox.
        /// </summary>
        public string SelectedText => cboSemana.Text;

        public SemanaFiltroSidebar()
        {
            InitializeComponent();
            // Valores por defecto (puedes cambiarlos desde el form si quieres)
            lblTitulo.Text = "Filtro de Semana";
            lblCampo.Text = "Semana :";
            btnGenerar.Text = "Generar";
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            var texto = cboSemana.Text?.Trim() ?? string.Empty;
            int? semanaNumero = null;

            // Intentar obtener el número de semana desde SelectedValue
            if (cboSemana.SelectedValue != null &&
                int.TryParse(cboSemana.SelectedValue.ToString(), out var num))
            {
                semanaNumero = num;
            }
            // Fallback: intentar parsear el texto visible
            else if (int.TryParse(texto, out var num2))
            {
                semanaNumero = num2;
            }

            var args = new GenerarReporteSemanaEventArgs(texto, semanaNumero);
            GenerarReporte?.Invoke(this, args);
        }

        // Permitir disparar con Enter también
        private void cboSemana_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnGenerar.PerformClick();
            }
        }


        public sealed class GenerarReporteSemanaEventArgs : EventArgs
        {
     
            public string SemanaTexto { get; }

            public int? SemanaNumero { get; }

            public GenerarReporteSemanaEventArgs(string semanaTexto, int? semanaNumero)
            {
                SemanaTexto = semanaTexto;
                SemanaNumero = semanaNumero;
            }
        }
    }
}
