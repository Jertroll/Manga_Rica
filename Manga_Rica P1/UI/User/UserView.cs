using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.User
{
    public partial class UserView : UserControl
    {
        public UserView()
        {
            InitializeComponent();
            Dock = DockStyle.Fill; // se ajusta automáticamente al panel principal
        }

        // Método de ejemplo para cargar datos de prueba
        private void UserView_Load(object sender, EventArgs e)
        {
            var tabla = new DataTable();
            tabla.Columns.Add("Id", typeof(int));
            tabla.Columns.Add("Nombre", typeof(string));
            tabla.Columns.Add("Usuario", typeof(string));
            tabla.Columns.Add("Rol", typeof(string));

            tabla.Rows.Add(1, "María Pérez", "mperez", "Admin");
            tabla.Rows.Add(2, "Juan Soto", "jsoto", "Empleado");
            tabla.Rows.Add(3, "Laura Vargas", "lvargas", "Supervisor");

            dataGridUsuarios.DataSource = tabla;
        }
    }
}
