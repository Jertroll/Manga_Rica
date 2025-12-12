using System.Drawing;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes.Shared
{
    partial class SemanaEmpleadoFiltroSidebar
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitulo;
        private Label lblSemana;
        private ComboBox cboSemana;
        private Label lblCarnet;
        private TextBox txtCedula;
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
            lblSemana = new Label();
            cboSemana = new ComboBox();
            lblCarnet = new Label();
            txtCedula = new TextBox();
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
            lblTitulo.Text = "Filtro Planilla por Empleado";
            // 
            // lblSemana
            // 
            lblSemana.Dock = DockStyle.Top;
            lblSemana.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSemana.Location = new Point(0, 30);
            lblSemana.Name = "lblSemana";
            lblSemana.Padding = new Padding(8, 4, 8, 0);
            lblSemana.Size = new Size(200, 21);
            lblSemana.TabIndex = 1;
            lblSemana.Text = "Semana :";
            // 
            // cboSemana
            // 
            cboSemana.Dock = DockStyle.Top;
            cboSemana.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSemana.FormattingEnabled = true;
            cboSemana.Location = new Point(0, 51);
            cboSemana.Margin = new Padding(8, 3, 8, 3);
            cboSemana.Name = "cboSemana";
            cboSemana.Size = new Size(200, 23);
            cboSemana.TabIndex = 2;
            // 
            // lblCarnet
            // 
            lblCarnet.Dock = DockStyle.Top;
            lblCarnet.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCarnet.Location = new Point(0, 74);
            lblCarnet.Name = "lblCarnet";
            lblCarnet.Padding = new Padding(8, 8, 8, 0);
            lblCarnet.Size = new Size(200, 24);
            lblCarnet.TabIndex = 3;
            lblCarnet.Text = "Carnet :";
            // 
            // txtCedula
            // 
            txtCedula.Dock = DockStyle.Top;
            txtCedula.Location = new Point(0, 98);
            txtCedula.Margin = new Padding(8, 3, 8, 3);
            txtCedula.Name = "txtCedula";
            txtCedula.Size = new Size(200, 23);
            txtCedula.TabIndex = 4;
            txtCedula.KeyDown += txtCedula_KeyDown;
            // 
            // btnGenerar
            // 
            btnGenerar.Dock = DockStyle.Top;
            btnGenerar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGenerar.Location = new Point(0, 121);
            btnGenerar.Margin = new Padding(8);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(200, 30);
            btnGenerar.TabIndex = 5;
            btnGenerar.Text = "Generar reporte";
            btnGenerar.UseVisualStyleBackColor = true;
            btnGenerar.Click += btnGenerar_Click;
            // 
            // SemanaEmpleadoFiltroSidebar
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.Control;
            Controls.Add(btnGenerar);
            Controls.Add(txtCedula);
            Controls.Add(lblCarnet);
            Controls.Add(cboSemana);
            Controls.Add(lblSemana);
            Controls.Add(lblTitulo);
            Name = "SemanaEmpleadoFiltroSidebar";
            Size = new Size(200, 200);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
