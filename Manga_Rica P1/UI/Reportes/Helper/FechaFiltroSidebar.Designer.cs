using System.Drawing;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes.Shared
{
    partial class FechaFiltroSidebar
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitulo;
        private Label lblFecha;
        private DateTimePicker dtpFecha;
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
            lblFecha = new Label();
            dtpFecha = new DateTimePicker();
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
            lblTitulo.Text = "Filtro de Fecha";
            // 
            // lblFecha
            // 
            lblFecha.Dock = DockStyle.Top;
            lblFecha.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFecha.Location = new Point(0, 30);
            lblFecha.Name = "lblFecha";
            lblFecha.Padding = new Padding(8, 4, 8, 0);
            lblFecha.Size = new Size(200, 21);
            lblFecha.TabIndex = 1;
            lblFecha.Text = "Fecha :";
            // 
            // dtpFecha
            // 
            dtpFecha.Dock = DockStyle.Top;
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.Location = new Point(0, 51);
            dtpFecha.Margin = new Padding(8, 3, 8, 3);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(200, 23);
            dtpFecha.TabIndex = 2;
            // 
            // btnGenerar
            // 
            btnGenerar.Dock = DockStyle.Top;
            btnGenerar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGenerar.Location = new Point(0, 74);
            btnGenerar.Margin = new Padding(8);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(200, 30);
            btnGenerar.TabIndex = 3;
            btnGenerar.Text = "Generar reporte";
            btnGenerar.UseVisualStyleBackColor = true;
            btnGenerar.Click += btnGenerar_Click;
            // 
            // FechaFiltroSidebar
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.Control;
            Controls.Add(btnGenerar);
            Controls.Add(dtpFecha);
            Controls.Add(lblFecha);
            Controls.Add(lblTitulo);
            Name = "FechaFiltroSidebar";
            Size = new Size(200, 160);
            ResumeLayout(false);
        }
    }
}
