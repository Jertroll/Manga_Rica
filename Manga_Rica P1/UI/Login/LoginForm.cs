using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Manga_Rica_P1.BLL.AutentificacionService;

namespace Manga_Rica_P1.UI.Login
{
    public partial class LoginForm : Form
    {

        private readonly AutentificacionService _auth;

        // ============
        // PALETA (igual a Form1)
        // ============
        private readonly Color Verde1 = Color.FromArgb(79, 170, 38);   // verde brillante
        private readonly Color Verde2 = Color.FromArgb(36, 128, 36);   // verde oscuro
        private readonly Color VerdeClaro = Color.FromArgb(219, 242, 191);
        private readonly Color Amarillo = Color.FromArgb(252, 198, 25);
        private readonly Color VerdeTexto = Color.FromArgb(33, 60, 21);
        private readonly Color RojoSalir = Color.FromArgb(200, 50, 50);

        // Panel “tarjeta” del login (debe existir en el Designer con el nombre panel1)
        // Controles esperados: comboBox1 (usuario), txtPassword, btnLogin, btnSalir, picLogo…
        public LoginForm(AutentificacionService auth)
        {
            InitializeComponent();
            _auth = auth;                
            ConfigurarUI();

            this.Load += LoginForm_Load;
        }

        private void ConfigurarUI()
        {
            // Comodidad de uso
            AcceptButton = btnLogin;
            CancelButton = btnSalir;
            DoubleBuffered = true;
            StartPosition = FormStartPosition.CenterScreen;

            // Contraseña oculta y placeholder
            if (txtPassword != null)
            {
                txtPassword.UseSystemPasswordChar = true;
                txtPassword.PlaceholderText = "*******";
            }

            // Estética del botón principal (igual que en Form1)
            if (btnLogin != null)
            {
                btnLogin.FlatStyle = FlatStyle.Flat;
                btnLogin.FlatAppearance.BorderSize = 0;
                btnLogin.BackColor = Amarillo;
                btnLogin.ForeColor = VerdeTexto;
            }

            // Botón Salir en rojo (igual que en Form1)
            if (btnSalir != null)
            {
                btnSalir.FlatStyle = FlatStyle.Flat;
                btnSalir.FlatAppearance.BorderSize = 0;
                btnSalir.BackColor = RojoSalir;
                btnSalir.ForeColor = Color.White;
                btnSalir.DialogResult = DialogResult.Cancel;
            }



        }

        // =============================
        //  FONDO DEGRADADO (igual a Form1)
        // =============================
        protected override void OnPaint(PaintEventArgs e)
        {
            using var lg = new LinearGradientBrush(ClientRectangle, Verde1, Verde2, 45f);
            e.Graphics.FillRectangle(lg, ClientRectangle);
            base.OnPaint(e);
        }

        // =============================
        //  EVENTOS
        // =============================
        private void LoginForm_Load(object? sender, EventArgs e)
        {
            CargarUsuariosEnCombo();
            if (comboBox1.Items.Count > 0 && comboBox1.CanFocus) comboBox1.Focus();
            int radio = 20; // radio de las esquinas
            var path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radio, radio), 180, 90);
            path.AddArc(new Rectangle(btnLogin.Width - radio, 0, radio, radio), 270, 90);
            path.AddArc(new Rectangle(btnLogin.Width - radio, btnLogin.Height - radio, radio, radio), 0, 90);
            path.AddArc(new Rectangle(0, btnLogin.Height - radio, radio, radio), 90, 90);
            path.CloseFigure();

            btnLogin.Region = new Region(path);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }



        private void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;
            try
            {
                var usuario = comboBox1?.Text?.Trim();
                var pass = txtPassword?.Text ?? string.Empty;

                if (string.IsNullOrWhiteSpace(usuario))
                {
                    MessageBox.Show("Ingrese el usuario.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    comboBox1?.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(pass))
                {
                    MessageBox.Show("Ingrese la contraseña.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPassword?.Focus();
                    return;
                }

                var (ok, msg, user) = _auth.Login(usuario, pass);

                if (!ok)
                {
                    MessageBox.Show(msg, "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword?.SelectAll();
                    txtPassword?.Focus();
                    return;
                }

                // Éxito
                DialogResult = DialogResult.OK;
                Close();
            }
            finally
            {
                btnLogin.Enabled = true;

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtPassword != null)
            {
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (btnLogin != null && txtPassword != null)
                btnLogin.Enabled = txtPassword.TextLength > 0;
        }

        private void cardPanel_Paint_1(object sender, PaintEventArgs e)
        {

        }


        // =============================
        //  PANEL CON BORDE REDONDEADO Y FONDO BLANCO
        //  (equivalente a tu panel1_Paint de Form1, adaptado)
        // =============================
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (sender is not Panel pnl) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Fondo BLANCO dentro del contorno redondeado
            var rect = pnl.ClientRectangle;
            rect.Inflate(-1, -1);
            int radius = 20;

            using var path = BuildRoundedRectangle(rect, radius);
            using var fill = new SolidBrush(Color.White);
            using var pen = new Pen(Color.Black, 2);

            e.Graphics.FillPath(fill, path);
            e.Graphics.DrawPath(pen, path);
        }

        // =============================
        //  HELPERS GRÁFICOS
        // =============================
        private static GraphicsPath BuildRoundedRectangle(Rectangle r, int radius)
        {
            int d = radius * 2;
            var gp = new GraphicsPath();
            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }

        private void picLogo_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }




        //Metodo para cargar los usuarios en el comboBox
        private void CargarUsuariosEnCombo()
        {
            try
            {
                var userNames = _auth.ObtenerUsernames(); // trae la lista de strings

                comboBox1.BeginUpdate();
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(userNames.Cast<object>().ToArray());
                comboBox1.EndUpdate();

                comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;

                if (comboBox1.Items.Count > 0)
                    comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los usuarios.\n" + ex.Message,
                    "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
