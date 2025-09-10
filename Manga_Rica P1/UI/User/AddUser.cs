using System;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.User
{
    public enum EditMode { Nuevo, Editar }

    public partial class AddUser : Form
    {
        // DTO simple para intercambio de datos con el llamador
        public class UsuarioResult
        {
            public int? Id { get; set; }                   // null en Nuevo
            public string Nombre { get; set; } = "";
            public string Clave { get; set; } = "";       // en Editar puede venir vacío (no cambiar)
            public string Perfil { get; set; } = "";       // Admin/Empleado/Supervisor...
            public DateTime FechaExpiracion { get; set; }
        }

        public UsuarioResult Resultado { get; private set; } = new UsuarioResult();
        private readonly EditMode _mode;

        // ===== Constructores públicos =====
        // NUEVO
        public AddUser() : this(EditMode.Nuevo, null) { }

        // EDITAR (precarga)
        public AddUser(UsuarioResult inicial) : this(EditMode.Editar, inicial) { }

        // ===== Ctor central =====
        private AddUser(EditMode mode, UsuarioResult? inicial)
        {
            InitializeComponent();
            _mode = mode;

            // Config modal/UX
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            this.AcceptButton = btnAgregar;
            this.CancelButton = btnCancelar;

            // Seguridad visual de contraseña
            textBoxClave.UseSystemPasswordChar = true;

            // Llenar combo de roles si hace falta (por si no lo hiciste en el designer)
            if (comboPerfil.Items.Count == 0)
            {
                comboPerfil.DropDownStyle = ComboBoxStyle.DropDownList;
                comboPerfil.Items.AddRange(new object[] { "Admin", "Empleado", "Supervisor" });
            }

            // UI según modo
            if (_mode == EditMode.Nuevo)
            {
                labelTitulo.Text = "Registro de Usuario";
                btnAgregar.Text = "Agregar";

                // valores por defecto
                dateTimePicker1.Value = DateTime.Today;
                if (comboPerfil.Items.Count > 0 && comboPerfil.SelectedIndex < 0)
                    comboPerfil.SelectedIndex = 0;
            }
            else // Editar
            {
                labelTitulo.Text = "Editar Usuario";
                btnAgregar.Text = "Guardar";

                // precargar datos
                var src = inicial ?? new UsuarioResult();
                Resultado = new UsuarioResult
                {
                    Id = src.Id,
                    Nombre = src.Nombre,
                    Clave = "", // no mostrar clave actual
                    Perfil = src.Perfil,
                    FechaExpiracion = src.FechaExpiracion == default ? DateTime.Today : src.FechaExpiracion
                };

                textBoxNombre.Text = Resultado.Nombre;
                dateTimePicker1.Value = Resultado.FechaExpiracion;

                // Seleccionar rol
                if (!string.IsNullOrWhiteSpace(Resultado.Perfil))
                {
                    int idx = comboPerfil.FindStringExact(Resultado.Perfil);
                    comboPerfil.SelectedIndex = idx >= 0 ? idx : 0;
                }
                else if (comboPerfil.Items.Count > 0)
                {
                    comboPerfil.SelectedIndex = 0;
                }
            }
        }

        // ===== Botones =====
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Validaciones mínimas (puedes migrar a ErrorProvider)
            if (string.IsNullOrWhiteSpace(textBoxNombre.Text))
            {
                MessageBox.Show("El nombre es requerido.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxNombre.Focus();
                return;
            }
            if (comboPerfil.SelectedItem is null)
            {
                MessageBox.Show("Seleccione un perfil.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboPerfil.DroppedDown = true;
                return;
            }
            if (_mode == EditMode.Nuevo && string.IsNullOrWhiteSpace(textBoxClave.Text))
            {
                MessageBox.Show("La contraseña es requerida.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxClave.Focus();
                return;
            }

            // Mapear al resultado
            Resultado.Nombre = textBoxNombre.Text.Trim();
            Resultado.Perfil = comboPerfil.SelectedItem?.ToString() ?? "";
            Resultado.FechaExpiracion = dateTimePicker1.Value.Date;

            // En Editar: solo cambiar clave si el usuario digitó una nueva
            if (!string.IsNullOrWhiteSpace(textBoxClave.Text))
                Resultado.Clave = textBoxClave.Text;

            this.DialogResult = DialogResult.OK; // cierra modal devolviendo OK
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; // cierra modal devolviendo Cancel
        }

        // ===== (Handlers vacíos del designer, si quieres conservarlos) =====
        private void label1_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }
        private void labelPerfil_Click(object sender, EventArgs e) { }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void labelClave_Click(object sender, EventArgs e) { }
    }
}
