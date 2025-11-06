
namespace Manga_Rica_P1.UI.Reportes
{
    partial class FormReporteEmpleadosInactivos
    {
        private System.ComponentModel.IContainer components = null;

        // Nueva implementacion: controles
        private Microsoft.Web.WebView2.WinForms.WebView2 _web;
        private System.Windows.Forms.Button _btnExportar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        // Nueva implementacion
        private void InitializeComponent()
        {
            _web = new Microsoft.Web.WebView2.WinForms.WebView2();
            _btnExportar = new Button();
            panelSuperior = new FlowLayoutPanel();
            _btnExportarExcel = new Button();
            ((System.ComponentModel.ISupportInitialize)_web).BeginInit();
            panelSuperior.SuspendLayout();
            SuspendLayout();
            // 
            // _web
            // 
            _web.AllowExternalDrop = true;
            _web.CreationProperties = null;
            _web.DefaultBackgroundColor = Color.White;
            _web.Location = new Point(0, 35);
            _web.Margin = new Padding(0);
            _web.Name = "_web";
            _web.Size = new Size(1084, 626);
            _web.TabIndex = 2;
            _web.ZoomFactor = 1D;
            // 
            // _btnExportar
            // 
            _btnExportar.Location = new Point(3, 3);
            _btnExportar.Name = "_btnExportar";
            _btnExportar.Size = new Size(120, 27);
            _btnExportar.TabIndex = 1;
            _btnExportar.Text = "Exportar a PDF";
            _btnExportar.UseVisualStyleBackColor = true;
            _btnExportar.Click += _btnExportar_Click;
            // 
            // panelSuperior
            // 
            panelSuperior.BackColor = SystemColors.AppWorkspace;
            panelSuperior.Controls.Add(_btnExportar);
            panelSuperior.Controls.Add(_btnExportarExcel);
            panelSuperior.Dock = DockStyle.Top;
            panelSuperior.Location = new Point(0, 0);
            panelSuperior.Margin = new Padding(0);
            panelSuperior.Name = "panelSuperior";
            panelSuperior.Size = new Size(1084, 35);
            panelSuperior.TabIndex = 3;
            // 
            // _btnExportarExcel
            // 
            _btnExportarExcel.Location = new Point(129, 3);
            _btnExportarExcel.Name = "_btnExportarExcel";
            _btnExportarExcel.Size = new Size(108, 27);
            _btnExportarExcel.TabIndex = 4;
            _btnExportarExcel.Text = "Exportar Excel";
            _btnExportarExcel.UseVisualStyleBackColor = true;
            _btnExportarExcel.Click += _btnExportarExcel_Click;
            // 
            // FormReporteEmpleadosInactivos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1084, 661);
            Controls.Add(panelSuperior);
            Controls.Add(_web);
            MinimumSize = new Size(800, 500);
            Name = "FormReporteEmpleadosInactivos";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reporte: Empleados No Activos";
            ((System.ComponentModel.ISupportInitialize)_web).EndInit();
            panelSuperior.ResumeLayout(false);
            ResumeLayout(false);
        }
        private FlowLayoutPanel panelSuperior;
        private Button _btnExportarExcel;
    }
}
