namespace Manga_Rica_P1.UI.Ventana_Principal
{
    partial class HomeView
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeView));
            labelTitulo = new Label();
            label1 = new Label();
            labelMensaje = new Label();
            label2 = new Label();
            linkLabelManual = new LinkLabel();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            pictureBox1 = new PictureBox();
            pictureBoxUser = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxUser).BeginInit();
            SuspendLayout();
            // 
            // labelTitulo
            // 
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTitulo.ForeColor = Color.Green;
            labelTitulo.Location = new Point(536, 22);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(179, 32);
            labelTitulo.TabIndex = 0;
            labelTitulo.Text = "Modulo Home";
            // 
            // label1
            // 
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.TabIndex = 0;
            // 
            // labelMensaje
            // 
            labelMensaje.AutoSize = true;
            labelMensaje.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelMensaje.ForeColor = Color.Green;
            labelMensaje.Location = new Point(20, 91);
            labelMensaje.Name = "labelMensaje";
            labelMensaje.Size = new Size(410, 21);
            labelMensaje.TabIndex = 1;
            labelMensaje.Text = "Bienvenido al sistema de planilla de Manga Rica S.A.";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Green;
            label2.Location = new Point(20, 523);
            label2.Name = "label2";
            label2.Size = new Size(277, 15);
            label2.TabIndex = 2;
            label2.Text = "Presiona link para descargar el manual de usuario";
            // 
            // linkLabelManual
            // 
            linkLabelManual.AutoSize = true;
            linkLabelManual.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            linkLabelManual.Location = new Point(20, 538);
            linkLabelManual.Name = "linkLabelManual";
            linkLabelManual.Size = new Size(131, 17);
            linkLabelManual.TabIndex = 3;
            linkLabelManual.TabStop = true;
            linkLabelManual.Text = "Manual_Usuario.pdf";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(20, 577);
            label3.Name = "label3";
            label3.Size = new Size(268, 15);
            label3.TabIndex = 4;
            label3.Text = "Consultas con el administrador de informatica...";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(20, 606);
            label4.Name = "label4";
            label4.Size = new Size(162, 15);
            label4.TabIndex = 5;
            label4.Text = "Contacto: +506 8707 - 4731";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(20, 621);
            label5.Name = "label5";
            label5.Size = new Size(198, 15);
            label5.TabIndex = 6;
            label5.Text = "Email: erodriguez@mangarica.com";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(454, 88);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(650, 427);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            // 
            // pictureBoxUser
            // 
            pictureBoxUser.Image = (Image)resources.GetObject("pictureBoxUser.Image");
            pictureBoxUser.Location = new Point(127, 164);
            pictureBoxUser.Name = "pictureBoxUser";
            pictureBoxUser.Size = new Size(189, 171);
            pictureBoxUser.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxUser.TabIndex = 8;
            pictureBoxUser.TabStop = false;
            // 
            // HomeView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pictureBoxUser);
            Controls.Add(pictureBox1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(linkLabelManual);
            Controls.Add(label2);
            Controls.Add(labelMensaje);
            Controls.Add(label1);
            Controls.Add(labelTitulo);
            Name = "HomeView";
            Size = new Size(1125, 678);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxUser).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTitulo;
        private Label label1;
        private Label labelMensaje;
        private Label label2;
        private LinkLabel linkLabelManual;
        private Label label3;
        private Label label4;
        private Label label5;
        private PictureBox pictureBox1;
        private PictureBox pictureBoxUser;
    }
}
