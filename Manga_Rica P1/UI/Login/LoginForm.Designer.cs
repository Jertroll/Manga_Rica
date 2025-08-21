using System.Drawing;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Login
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel cardPanel;
        private PictureBox picLogo;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnSalir;
        private LinkLabel lnkOlvido;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            cardPanel = new Panel();
            panel1 = new Panel();
            label3 = new Label();
            comboBox1 = new ComboBox();
            label2 = new Label();
            label1 = new Label();
            txtPassword = new TextBox();
            lnkOlvido = new LinkLabel();
            btnLogin = new Button();
            btnSalir = new Button();
            picLogo = new PictureBox();
            cardPanel.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();
            // 
            // cardPanel
            // 
            cardPanel.BackColor = Color.FromArgb(97, 245, 39);
            cardPanel.Controls.Add(panel1);
            cardPanel.Controls.Add(btnSalir);
            cardPanel.Controls.Add(picLogo);
            cardPanel.Dock = DockStyle.Fill;
            cardPanel.Location = new Point(0, 0);
            cardPanel.Name = "cardPanel";
            cardPanel.Size = new Size(744, 380);
            cardPanel.TabIndex = 0;
            cardPanel.Paint += cardPanel_Paint_1;
            // 
            // panel1
            // 
            panel1.Controls.Add(label3);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(txtPassword);
            panel1.Controls.Add(lnkOlvido);
            panel1.Controls.Add(btnLogin);
            panel1.Location = new Point(402, 27);
            panel1.Name = "panel1";
            panel1.Size = new Size(333, 327);
            panel1.TabIndex = 9;
            panel1.Paint += panel1_Paint;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.White;
            label3.Location = new Point(18, 83);
            label3.Name = "label3";
            label3.Size = new Size(47, 15);
            label3.TabIndex = 8;
            label3.Text = "Usuario";
            label3.Click += label3_Click;
            // 
            // comboBox1
            // 
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox1.Font = new Font("Segoe UI", 11F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(18, 101);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(293, 28);
            comboBox1.TabIndex = 7;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.White;
            label2.Location = new Point(18, 161);
            label2.Name = "label2";
            label2.Size = new Size(67, 15);
            label2.TabIndex = 6;
            label2.Text = "Contraseña";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.White;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.Location = new Point(16, 15);
            label1.Name = "label1";
            label1.Size = new Size(295, 21);
            label1.TabIndex = 5;
            label1.Text = "Bienvenido al Sistema de Manga Rica";
            label1.Click += label1_Click;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 11F);
            txtPassword.Location = new Point(18, 179);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(293, 27);
            txtPassword.TabIndex = 1;
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // lnkOlvido
            // 
            lnkOlvido.AutoSize = true;
            lnkOlvido.BackColor = Color.White;
            lnkOlvido.Location = new Point(18, 218);
            lnkOlvido.Name = "lnkOlvido";
            lnkOlvido.Size = new Size(128, 15);
            lnkOlvido.TabIndex = 3;
            lnkOlvido.TabStop = true;
            lnkOlvido.Text = "¿Olvidó su contraseña?";
            // 
            // btnLogin
            // 
            btnLogin.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnLogin.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnLogin.Location = new Point(7, 257);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(320, 42);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "Ingresar";
            btnLogin.Click += btnLogin_Click;
            // 
            // btnSalir
            // 
            btnSalir.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSalir.Location = new Point(20, 619);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(80, 28);
            btnSalir.TabIndex = 1;
            btnSalir.Text = "Salir";
            btnSalir.Click += btnSalir_Click;
            // 
            // picLogo
            // 
            picLogo.Image = Properties.Resources.Manga_Rica__Logo;
            picLogo.Location = new Point(-31, -5);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(445, 385);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 0;
            picLogo.TabStop = false;
            picLogo.Click += picLogo_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(744, 380);
            Controls.Add(cardPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Manga Rica - Login";
            Load += LoginForm_Load;
            cardPanel.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ResumeLayout(false);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private Label label1;
        private Label label2;
        private ComboBox comboBox1;
        private Label label3;
        private Panel panel1;
    }
}
