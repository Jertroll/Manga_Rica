using Manga_Rica_P1.UI.Soda;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Uniforme
{
    public partial class Uniforme : Form
    {
        public Uniforme()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            using (var buscarForm = new BuscadorUniforme())
            {
                buscarForm.ShowDialog();
            }
        }
    }
}
