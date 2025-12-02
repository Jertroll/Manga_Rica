using System.Drawing;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes.Shared
{
    partial class EmpleadoFechaFiltroSidebar
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitulo;
        private Label lblCarne;
        private TextBox txtCarne;
        private Label lblDesde;
        private DateTimePicker dtpDesde;
        private Label lblHasta;
        private DateTimePicker dtpHasta;
        private Button btnGenerar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblCarne = new Label();
            txtCarne = new TextBox();
            lblDesde = new Label();
            dtpDesde = new DateTimePicker();
            lblHasta = new Label();
            dtpHasta = new DateTimePicker();
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
            lblTitulo.Size = new Size(200, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Filtro Soda por empleado";
            // 
            // lblCarne
            // 
            lblCarne.Dock = DockStyle.Top;
            lblCarne.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCarne.Location = new Point(0, 30);
            lblCarne.Name = "lblCarne";
            lblCarne.Padding = new Padding(8, 6, 8, 0);
            lblCarne.Size = new Size(200, 22);
            lblCarne.TabIndex = 1;
            lblCarne.Text = "Carne :";
            // 
            // txtCarne
            // 
            txtCarne.Dock = DockStyle.Top;
            txtCarne.Location = new Point(0, 52);
            txtCarne.Margin = new Padding(8, 3, 8, 3);
            txtCarne.Name = "txtCarne";
            txtCarne.Size = new Size(200, 23);
            txtCarne.TabIndex = 2;
            txtCarne.KeyDown += txtCarne_KeyDown;
            // 
            // lblDesde
            // 
            lblDesde.Dock = DockStyle.Top;
            lblDesde.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDesde.Location = new Point(0, 75);
            lblDesde.Name = "lblDesde";
            lblDesde.Padding = new Padding(8, 10, 8, 0);
            lblDesde.Size = new Size(200, 24);
            lblDesde.TabIndex = 3;
            lblDesde.Text = "Desde :";
            // 
            // dtpDesde
            // 
            dtpDesde.Dock = DockStyle.Top;
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(0, 99);
            dtpDesde.Margin = new Padding(8, 3, 8, 3);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(200, 23);
            dtpDesde.TabIndex = 4;
            // 
            // lblHasta
            // 
            lblHasta.Dock = DockStyle.Top;
            lblHasta.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHasta.Location = new Point(0, 122);
            lblHasta.Name = "lblHasta";
            lblHasta.Padding = new Padding(8, 10, 8, 0);
            lblHasta.Size = new Size(200, 24);
            lblHasta.TabIndex = 5;
            lblHasta.Text = "Hasta :";
            // 
            // dtpHasta
            // 
            dtpHasta.Dock = DockStyle.Top;
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(0, 146);
            dtpHasta.Margin = new Padding(8, 3, 8, 3);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(200, 23);
            dtpHasta.TabIndex = 6;
            // 
            // btnGenerar
            // 
            btnGenerar.Dock = DockStyle.Top;
            btnGenerar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGenerar.Location = new Point(0, 169);
            btnGenerar.Margin = new Padding(8);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(200, 32);
            btnGenerar.TabIndex = 7;
            btnGenerar.Text = "Generar reporte";
            btnGenerar.UseVisualStyleBackColor = true;
            btnGenerar.Click += btnGenerar_Click;
            // 
            // EmpleadoFechaFiltroSidebar
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.Control;
            Controls.Add(btnGenerar);
            Controls.Add(dtpHasta);
            Controls.Add(lblHasta);
            Controls.Add(dtpDesde);
            Controls.Add(lblDesde);
            Controls.Add(txtCarne);
            Controls.Add(lblCarne);
            Controls.Add(lblTitulo);
            Name = "EmpleadoFechaFiltroSidebar";
            Size = new Size(200, 220);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
