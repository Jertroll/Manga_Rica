using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.CierreDiario
{
    public partial class CierreDiario : Form
    {
        public CierreDiario()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void buttonCierreDiario_Click(object sender, EventArgs e)
        {
            // Lógica para manejar el cierre diario
            MessageBox.Show("Cierre diario realizado.");
        }
    }
}
