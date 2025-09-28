using System;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Hora
{
    public partial class RegistroEntradaYSalida : Form
    {
        // Cache de datos para precarga en ambos tabs
        private long? _carne;
        private string _nombre = string.Empty;
        private string _ape1 = string.Empty;
        private string _ape2 = string.Empty;

        public RegistroEntradaYSalida()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            // Cuando cambias de pestaña, re-aplica los datos a los controles visibles
            tabControl1.SelectedIndexChanged += (_, __) => ApplyPrefillToBothTabs();
        }

        /// <summary>
        /// Precarga datos del empleado y los aplica a ambos tabs (Entrada y Salida).
        /// </summary>
        public void PrefillEmpleado(long carne, string nombre, string ape1, string ape2)
        {
            _carne = carne;
            _nombre = nombre ?? string.Empty;
            _ape1 = ape1 ?? string.Empty;
            _ape2 = ape2 ?? string.Empty;

            ApplyPrefillToBothTabs();
        }

        // Aplica la cache a los controles de Entrada y Salida
        private void ApplyPrefillToBothTabs()
        {
            if (_carne == null) return;

            // --- Tab Entrada ---
            if (textBoxCarneEntrada != null) textBoxCarneEntrada.Text = _carne.Value.ToString();
            if (textBoxNombreEntrada != null) textBoxNombreEntrada.Text = _nombre;
            if (textBoxApellido1Entrada != null) textBoxApellido1Entrada.Text = _ape1;


            // --- Tab Salida ---
            if (textBoxCarneSalida != null) textBoxCarneSalida.Text = _carne.Value.ToString();
            if (textBoxNombreSalida != null) textBoxNombreSalida.Text = _nombre;
            if (textBoxApellidoSalida != null) textBoxApellidoSalida.Text = _ape1;

        }

        private void tabPageEntrada_Click(object sender, EventArgs e)
        {

        }
    }
}
