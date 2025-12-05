using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes.Shared
{
    partial class SemanaDepartamentoFiltroSidebar
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitulo;
        private Label lblSemana;
        private ComboBox cboSemana;
        private Label lblDepartamento;
        private ComboBox cboDepartamento;
        private Button btnGenerar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitulo = new System.Windows.Forms.Label();
            lblSemana = new System.Windows.Forms.Label();
            cboSemana = new System.Windows.Forms.ComboBox();
            lblDepartamento = new System.Windows.Forms.Label();
            cboDepartamento = new System.Windows.Forms.ComboBox();
            btnGenerar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = System.Drawing.Color.Green;
            lblTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            lblTitulo.Font = new System.Drawing.Font(
                "Segoe UI Black",
                9.75F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                0);
            lblTitulo.ForeColor = System.Drawing.Color.White;
            lblTitulo.Location = new System.Drawing.Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Padding = new System.Windows.Forms.Padding(8, 8, 8, 4);
            lblTitulo.Size = new System.Drawing.Size(200, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Filtro Planilla por Departamento";
            // 
            // lblSemana
            // 
            lblSemana.Dock = System.Windows.Forms.DockStyle.Top;
            lblSemana.Font = new System.Drawing.Font(
                "Segoe UI",
                9F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                0);
            lblSemana.Location = new System.Drawing.Point(0, 30);
            lblSemana.Name = "lblSemana";
            lblSemana.Padding = new System.Windows.Forms.Padding(8, 4, 8, 0);
            lblSemana.Size = new System.Drawing.Size(200, 21);
            lblSemana.TabIndex = 1;
            lblSemana.Text = "Semana :";
            // 
            // cboSemana
            // 
            cboSemana.Dock = System.Windows.Forms.DockStyle.Top;
            cboSemana.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboSemana.FormattingEnabled = true;
            cboSemana.Location = new System.Drawing.Point(0, 51);
            cboSemana.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            cboSemana.Name = "cboSemana";
            cboSemana.Size = new System.Drawing.Size(200, 23);
            cboSemana.TabIndex = 2;
            // 
            // lblDepartamento
            // 
            lblDepartamento.Dock = System.Windows.Forms.DockStyle.Top;
            lblDepartamento.Font = new System.Drawing.Font(
                "Segoe UI",
                9F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                0);
            lblDepartamento.Location = new System.Drawing.Point(0, 74);
            lblDepartamento.Name = "lblDepartamento";
            lblDepartamento.Padding = new System.Windows.Forms.Padding(8, 8, 8, 0);
            lblDepartamento.Size = new System.Drawing.Size(200, 24);
            lblDepartamento.TabIndex = 3;
            lblDepartamento.Text = "Departamento :";
            // 
            // cboDepartamento
            // 
            cboDepartamento.Dock = System.Windows.Forms.DockStyle.Top;
            cboDepartamento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboDepartamento.FormattingEnabled = true;
            cboDepartamento.Location = new System.Drawing.Point(0, 98);
            cboDepartamento.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            cboDepartamento.Name = "cboDepartamento";
            cboDepartamento.Size = new System.Drawing.Size(200, 23);
            cboDepartamento.TabIndex = 4;
            // 
            // btnGenerar
            // 
            btnGenerar.Dock = System.Windows.Forms.DockStyle.Top;
            btnGenerar.Font = new System.Drawing.Font(
                "Segoe UI",
                9.75F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                0);
            btnGenerar.Location = new System.Drawing.Point(0, 121);
            btnGenerar.Margin = new System.Windows.Forms.Padding(8);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new System.Drawing.Size(200, 30);
            btnGenerar.TabIndex = 5;
            btnGenerar.Text = "Generar reporte";
            btnGenerar.UseVisualStyleBackColor = true;
            btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            // 
            // SemanaDepartamentoFiltroSidebar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(btnGenerar);
            this.Controls.Add(cboDepartamento);
            this.Controls.Add(lblDepartamento);
            this.Controls.Add(cboSemana);
            this.Controls.Add(lblSemana);
            this.Controls.Add(lblTitulo);
            this.Name = "SemanaDepartamentoFiltroSidebar";
            this.Size = new System.Drawing.Size(200, 200);
            this.ResumeLayout(false);
        }
    }
}
