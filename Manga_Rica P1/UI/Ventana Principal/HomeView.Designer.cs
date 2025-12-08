namespace Manga_Rica_P1.UI.Ventana_Principal
{
    partial class HomeView
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeView));
            panelHeader = new Panel();
            labelTitulo = new Label();
            panelLeft = new Panel();
            panelContact = new Panel();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            panelManual = new Panel();
            linkLabelManual = new LinkLabel();
            label2 = new Label();
            pictureBoxUser = new PictureBox();
            labelMensaje = new Label();
            panelRight = new Panel();
            pictureBox1 = new PictureBox();
            panelHeader.SuspendLayout();
            panelLeft.SuspendLayout();
            panelContact.SuspendLayout();
            panelManual.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxUser).BeginInit();
            panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(labelTitulo);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1125, 60);
            panelHeader.TabIndex = 0;
            // 
            // labelTitulo
            // 
            labelTitulo.Dock = DockStyle.Fill;
            labelTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            labelTitulo.ForeColor = Color.FromArgb(0, 128, 0);
            labelTitulo.Location = new Point(0, 0);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(1125, 60);
            labelTitulo.TabIndex = 0;
            labelTitulo.Text = "Vista Home";
            labelTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelLeft
            // 
            panelLeft.BackColor = Color.FromArgb(248, 250, 252);
            panelLeft.Controls.Add(panelContact);
            panelLeft.Controls.Add(panelManual);
            panelLeft.Controls.Add(pictureBoxUser);
            panelLeft.Controls.Add(labelMensaje);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 60);
            panelLeft.Name = "panelLeft";
            panelLeft.Padding = new Padding(24);
            panelLeft.Size = new Size(360, 618);
            panelLeft.TabIndex = 1;
            // 
            // panelContact
            // 
            panelContact.Controls.Add(label5);
            panelContact.Controls.Add(label4);
            panelContact.Controls.Add(label3);
            panelContact.Dock = DockStyle.Bottom;
            panelContact.Location = new Point(24, 428);
            panelContact.Name = "panelContact";
            panelContact.Size = new Size(312, 126);
            panelContact.TabIndex = 4;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F);
            label5.ForeColor = Color.FromArgb(55, 65, 81);
            label5.Location = new Point(0, 72);
            label5.Name = "label5";
            label5.Size = new Size(192, 15);
            label5.TabIndex = 2;
            label5.Text = "Email: erodriguez@mangarica.com";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F);
            label4.ForeColor = Color.FromArgb(55, 65, 81);
            label4.Location = new Point(0, 51);
            label4.Name = "label4";
            label4.Size = new Size(150, 15);
            label4.TabIndex = 1;
            label4.Text = "Contacto: +506 8707 - 4731";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(55, 65, 81);
            label3.Location = new Point(0, 8);
            label3.Name = "label3";
            label3.Size = new Size(270, 15);
            label3.TabIndex = 0;
            label3.Text = "Consultas con el administrador de informática…";
            // 
            // panelManual
            // 
            panelManual.Controls.Add(linkLabelManual);
            panelManual.Controls.Add(label2);
            panelManual.Dock = DockStyle.Bottom;
            panelManual.Location = new Point(24, 554);
            panelManual.Name = "panelManual";
            panelManual.Size = new Size(312, 40);
            panelManual.TabIndex = 3;
            // 
            // linkLabelManual
            // 
            linkLabelManual.ActiveLinkColor = Color.FromArgb(21, 128, 61);
            linkLabelManual.AutoSize = true;
            linkLabelManual.Font = new Font("Segoe UI", 9.25F, FontStyle.Bold);
            linkLabelManual.LinkColor = Color.FromArgb(22, 163, 74);
            linkLabelManual.Location = new Point(0, 20);
            linkLabelManual.Name = "linkLabelManual";
            linkLabelManual.Size = new Size(131, 17);
            linkLabelManual.TabIndex = 1;
            linkLabelManual.TabStop = true;
            linkLabelManual.Text = "Manual_Usuario.pdf";
            linkLabelManual.VisitedLinkColor = Color.FromArgb(22, 163, 74);
            linkLabelManual.LinkClicked += linkLabelManual_LinkClicked;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(21, 128, 61);
            label2.Location = new Point(0, 2);
            label2.Name = "label2";
            label2.Size = new Size(306, 15);
            label2.TabIndex = 0;
            label2.Text = "Presiona el enlace para descargar el manual de usuario";
            // 
            // pictureBoxUser
            // 
            pictureBoxUser.Anchor = AnchorStyles.Top;
            pictureBoxUser.Image = (Image)resources.GetObject("pictureBoxUser.Image");
            pictureBoxUser.Location = new Point(96, 150);
            pictureBoxUser.Name = "pictureBoxUser";
            pictureBoxUser.Size = new Size(176, 160);
            pictureBoxUser.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxUser.TabIndex = 2;
            pictureBoxUser.TabStop = false;
            // 
            // labelMensaje
            // 
            labelMensaje.AutoSize = true;
            labelMensaje.Dock = DockStyle.Top;
            labelMensaje.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            labelMensaje.ForeColor = Color.FromArgb(21, 128, 61);
            labelMensaje.Location = new Point(24, 24);
            labelMensaje.MaximumSize = new Size(312, 0);
            labelMensaje.Name = "labelMensaje";
            labelMensaje.Size = new Size(272, 42);
            labelMensaje.TabIndex = 1;
            labelMensaje.Text = "Bienvenido al sistema de planilla de Manga Rica S.A.";
            // 
            // panelRight
            // 
            panelRight.BackColor = Color.White;
            panelRight.Controls.Add(pictureBox1);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(360, 60);
            panelRight.Name = "panelRight";
            panelRight.Padding = new Padding(40, 30, 40, 40);
            panelRight.Size = new Size(765, 618);
            panelRight.TabIndex = 2;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(40, 30);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(685, 548);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // HomeView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            Controls.Add(panelHeader);
            Name = "HomeView";
            Size = new Size(1125, 678);
            panelHeader.ResumeLayout(false);
            panelLeft.ResumeLayout(false);
            panelLeft.PerformLayout();
            panelContact.ResumeLayout(false);
            panelContact.PerformLayout();
            panelManual.ResumeLayout(false);
            panelManual.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxUser).EndInit();
            panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label labelTitulo;
        private Panel panelLeft;
        private Label labelMensaje;
        private PictureBox pictureBoxUser;
        private Panel panelManual;
        private Label label2;
        private LinkLabel linkLabelManual;
        private Panel panelContact;
        private Label label3;
        private Label label4;
        private Label label5;
        private Panel panelRight;
        private PictureBox pictureBox1;
    }
}
