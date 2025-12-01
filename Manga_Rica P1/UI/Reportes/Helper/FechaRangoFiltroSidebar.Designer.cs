using System.Drawing;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes.Shared
{
    partial class FechaRangoFiltroSidebar
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitulo;
        private Label lblDesde;
        private Label lblHasta;
        private DateTimePicker dtpDesde;
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
            lblDesde = new Label();
            lblHasta = new Label();
            dtpDesde = new DateTimePicker();
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
            lblTitulo.Text = "Rango de Fechas";
            // 
            // lblDesde
            // 
            lblDesde.Dock = DockStyle.Top;
            lblDesde.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDesde.Location = new Point(0, 30);
            lblDesde.Name = "lblDesde";
            lblDesde.Padding = new Padding(8, 4, 8, 0);
            lblDesde.Size = new Size(200, 21);
            lblDesde.TabIndex = 1;
            lblDesde.Text = "Desde :";
            // 
            // dtpDesde
            // 
            dtpDesde.Dock = DockStyle.Top;
            dtpDesde.Format = DateTimePickerFormat.Short;
            dtpDesde.Location = new Point(0, 51);
            dtpDesde.Margin = new Padding(8, 3, 8, 3);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(200, 23);
            dtpDesde.TabIndex = 2;
            // 
            // lblHasta
            // 
            lblHasta.Dock = DockStyle.Top;
            lblHasta.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHasta.Location = new Point(0, 74);
            lblHasta.Name = "lblHasta";
            lblHasta.Padding = new Padding(8, 4, 8, 0);
            lblHasta.Size = new Size(200, 21);
            lblHasta.TabIndex = 3;
            lblHasta.Text = "Hasta :";
            // 
            // dtpHasta
            // 
            dtpHasta.Dock = DockStyle.Top;
            dtpHasta.Format = DateTimePickerFormat.Short;
            dtpHasta.Location = new Point(0, 95);
            dtpHasta.Margin = new Padding(8, 3, 8, 3);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(200, 23);
            dtpHasta.TabIndex = 4;
            // 
            // btnGenerar
            // 
            btnGenerar.Dock = DockStyle.Top;
            btnGenerar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGenerar.Location = new Point(0, 118);
            btnGenerar.Margin = new Padding(8);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(200, 30);
            btnGenerar.TabIndex = 5;
            btnGenerar.Text = "Generar reporte";
            btnGenerar.UseVisualStyleBackColor = true;
            btnGenerar.Click += btnGenerar_Click;
            // 
            // FechaRangoFiltroSidebar
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.Control;
            Controls.Add(btnGenerar);
            Controls.Add(dtpHasta);
            Controls.Add(lblHasta);
            Controls.Add(dtpDesde);
            Controls.Add(lblDesde);
            Controls.Add(lblTitulo);
            Name = "FechaRangoFiltroSidebar";
            Size = new Size(200, 200);
            ResumeLayout(false);
        }
    }
}
