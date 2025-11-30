namespace Manga_Rica_P1.UI.Reportes.Shared
{
    partial class EmpleadoFiltroSidebar
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblCampo;
        private System.Windows.Forms.TextBox txtCarne;
        private System.Windows.Forms.Button btnGenerar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblCampo = new Label();
            txtCarne = new TextBox();
            btnGenerar = new Button();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.Green;
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI Black", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Padding = new Padding(8, 8, 8, 4);
            lblTitulo.Size = new Size(180, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Filtro de Empleado";
            // 
            // lblCampo
            // 
            lblCampo.Dock = DockStyle.Top;
            lblCampo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCampo.Location = new Point(0, 30);
            lblCampo.Name = "lblCampo";
            lblCampo.Padding = new Padding(8, 4, 8, 0);
            lblCampo.Size = new Size(180, 21);
            lblCampo.TabIndex = 1;
            lblCampo.Text = "Carne :";
            // 
            // txtCarne
            // 
            txtCarne.Dock = DockStyle.Top;
            txtCarne.Location = new Point(0, 51);
            txtCarne.Margin = new Padding(8, 3, 8, 3);
            txtCarne.Name = "txtCarne";
            txtCarne.Size = new Size(180, 23);
            txtCarne.TabIndex = 2;
            txtCarne.KeyDown += txtCarne_KeyDown;
            // 
            // btnGenerar
            // 
            btnGenerar.Dock = DockStyle.Top;
            btnGenerar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGenerar.Location = new Point(0, 74);
            btnGenerar.Margin = new Padding(8);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(180, 30);
            btnGenerar.TabIndex = 3;
            btnGenerar.Text = "Generar reporte";
            btnGenerar.UseVisualStyleBackColor = true;
            btnGenerar.Click += btnGenerar_Click;
            // 
            // EmpleadoFiltroSidebar
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.Control;
            Controls.Add(btnGenerar);
            Controls.Add(txtCarne);
            Controls.Add(lblCampo);
            Controls.Add(lblTitulo);
            Name = "EmpleadoFiltroSidebar";
            Size = new Size(180, 200);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
