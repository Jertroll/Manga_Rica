using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Ventana_Principal
{
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();
        }

        private void Principal_Load(object sender, EventArgs e)
        {

        }

        bool menuDesplegado = false;
        private void menuConfTransicion_Tick(object sender, EventArgs e)
        {
            if (menuDesplegado == false)
            {

                // Reemplaza la línea problemática:
                // menuConfContenedor += 10;

                // Por ejemplo, si deseas aumentar la altura de un FlowLayoutPanel llamado menuConfContenedor:
                menuConfContenedor.Height += 10;
                if (menuConfContenedor.Height >= 230)
                {
                    menuConfTransicion.Stop();
                    menuDesplegado = true;

                }
            }
            else
            {
                menuConfContenedor.Height -= 10;
                if (menuConfContenedor.Height <= 46)
                {
                    menuConfTransicion.Stop();
                    menuDesplegado = false;
                }
            }
        }

        private void menuConfContenedor_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnConfiguraciones_Click(object sender, EventArgs e)
        {
            menuConfTransicion.Start();
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {

        }

        private void menuPlanillaTransicion_Tick(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanelSideBar_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
