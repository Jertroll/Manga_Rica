using System;
using System.Drawing;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes.Shared
{
    partial class SemanaFiltroSidebar
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitulo;
        private Label lblCampo;
        private ComboBox cboSemana;
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
            lblCampo = new Label();
            cboSemana = new ComboBox();
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
            lblTitulo.Text = "Filtro de Semana";
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
            lblCampo.Text = "Semana :";
            // 
            // cboSemana
            // 
            cboSemana.Dock = DockStyle.Top;
            cboSemana.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSemana.FormattingEnabled = true;
            cboSemana.Location = new Point(0, 51);
            cboSemana.Margin = new Padding(8, 3, 8, 3);
            cboSemana.Name = "cboSemana";
            cboSemana.Size = new Size(180, 23);
            cboSemana.TabIndex = 2;
            cboSemana.KeyDown += cboSemana_KeyDown;
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
            // SemanaFiltroSidebar
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.Control;
            Controls.Add(btnGenerar);
            Controls.Add(cboSemana);
            Controls.Add(lblCampo);
            Controls.Add(lblTitulo);
            Name = "SemanaFiltroSidebar";
            Size = new Size(180, 200);
            ResumeLayout(false);
        }
    }
}
