 using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Manga_Rica_P1
{
    public partial class Form1 : Form
    {
        // Paleta inspirada en el logo
        readonly Color Verde1 = Color.FromArgb(79, 170, 38);   // verde brillante
        readonly Color Verde2 = Color.FromArgb(36, 128, 36);   // verde oscuro
        readonly Color VerdeClaro = Color.FromArgb(219, 242, 191);
        readonly Color Amarillo = Color.FromArgb(252, 198, 25);

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true; // reduce flicker
            CenterToScreen();
            txtPassword.UseSystemPasswordChar = true;

            // Estética del botón
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.BackColor = Amarillo;
            btnLogin.ForeColor = Color.FromArgb(33, 60, 21);

            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.FlatAppearance.BorderSize = 0;

            btnSalir.BackColor = Color.FromArgb(200, 50, 50);


            // Placeholder simple
            txtPassword.PlaceholderText = "*******";
        }






        private static GraphicsPath RoundedRect(Rectangle r, int radius)
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

        // === Eventos ===
        private void btnLogin_Click(object sender, EventArgs e)
        {
            var pass = txtPassword.Text;


            if (string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Ingrese la contraseña.", "Manga Rica", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // TODO: reemplazar por tu autenticación real (BD / API)


        }

        private void btnSalir_Click(object sender, EventArgs e) => Close();

        private void cardPanel_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }

        private void picLogo_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Suavizar bordes al dibujar
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Rectángulo del panel, un poco más pequeño para que el borde no se corte
            var r = panel1.ClientRectangle;
            r.Inflate(-1, -1);

            int radio = 20; // Radio de las esquinas redondeadas

            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                // Esquinas redondeadas (orden: arriba-izquierda, arriba-derecha, abajo-derecha, abajo-izquierda)
                path.AddArc(r.X, r.Y, radio, radio, 180, 90);
                path.AddArc(r.Right - radio, r.Y, radio, radio, 270, 90);
                path.AddArc(r.Right - radio, r.Bottom - radio, radio, radio, 0, 90);
                path.AddArc(r.X, r.Bottom - radio, radio, radio, 90, 90);
                path.CloseFigure();

                // Fondo blanco
                using (var brush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillPath(brush, path);
                }

                // Borde negro
                using (var pen = new Pen(Color.Black, 2))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private void labelTituloPrincipal_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
