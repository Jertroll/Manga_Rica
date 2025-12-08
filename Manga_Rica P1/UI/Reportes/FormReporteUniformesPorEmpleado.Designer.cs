namespace Manga_Rica_P1.UI.Reportes
{
    partial class FormReporteUniformesPorEmpleado
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.FlowLayoutPanel panelBotones;
        private System.Windows.Forms.Button _btnExportarPdf;
        private System.Windows.Forms.Button _btnExportarExcel;
        private Microsoft.Web.WebView2.WinForms.WebView2 _web;
        private Manga_Rica_P1.UI.Reportes.Shared.EmpleadoFiltroSidebar empleadoFiltroSidebar1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReporteUniformesPorEmpleado));
            panelLeft = new Panel();
            empleadoFiltroSidebar1 = new Manga_Rica_P1.UI.Reportes.Shared.EmpleadoFiltroSidebar();
            panelTop = new Panel();
            panelBotones = new FlowLayoutPanel();
            _btnExportarPdf = new Button();
            _btnExportarExcel = new Button();
            _web = new Microsoft.Web.WebView2.WinForms.WebView2();
            panelLeft.SuspendLayout();
            panelTop.SuspendLayout();
            panelBotones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_web).BeginInit();
            SuspendLayout();
            // 
            // panelLeft
            // 
            panelLeft.BackColor = SystemColors.ControlLight;
            panelLeft.Controls.Add(empleadoFiltroSidebar1);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Name = "panelLeft";
            panelLeft.Padding = new Padding(4);
            panelLeft.Size = new Size(190, 700);
            panelLeft.TabIndex = 0;
            // 
            // empleadoFiltroSidebar1
            // 
            empleadoFiltroSidebar1.BackColor = SystemColors.Control;
            empleadoFiltroSidebar1.CarneTexto = "";
            empleadoFiltroSidebar1.Dock = DockStyle.Top;
            empleadoFiltroSidebar1.EtiquetaCampo = "Carne :";
            empleadoFiltroSidebar1.Location = new Point(4, 4);
            empleadoFiltroSidebar1.Name = "empleadoFiltroSidebar1";
            empleadoFiltroSidebar1.Size = new Size(182, 150);
            empleadoFiltroSidebar1.TabIndex = 0;
            empleadoFiltroSidebar1.TextoBoton = "Generar reporte";
            // 
            // panelTop
            // 
            panelTop.Controls.Add(panelBotones);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(190, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(910, 40);
            panelTop.TabIndex = 1;
            // 
            // panelBotones
            // 
            panelBotones.BackColor = SystemColors.AppWorkspace;
            panelBotones.Controls.Add(_btnExportarPdf);
            panelBotones.Controls.Add(_btnExportarExcel);
            panelBotones.Dock = DockStyle.Fill;
            panelBotones.Location = new Point(0, 0);
            panelBotones.Name = "panelBotones";
            panelBotones.Size = new Size(910, 40);
            panelBotones.TabIndex = 0;
            // 
            // _btnExportarPdf
            // 
            _btnExportarPdf.Location = new Point(3, 3);
            _btnExportarPdf.Name = "_btnExportarPdf";
            _btnExportarPdf.Size = new Size(108, 27);
            _btnExportarPdf.TabIndex = 0;
            _btnExportarPdf.Text = "Exportar PDF";
            _btnExportarPdf.UseVisualStyleBackColor = true;
            _btnExportarPdf.Click += _btnExportarPdf_Click;
            // 
            // _btnExportarExcel
            // 
            _btnExportarExcel.Location = new Point(117, 3);
            _btnExportarExcel.Name = "_btnExportarExcel";
            _btnExportarExcel.Size = new Size(108, 27);
            _btnExportarExcel.TabIndex = 1;
            _btnExportarExcel.Text = "Exportar Excel";
            _btnExportarExcel.UseVisualStyleBackColor = true;
            _btnExportarExcel.Click += _btnExportarExcel_Click;
            // 
            // _web
            // 
            _web.AllowExternalDrop = true;
            _web.CreationProperties = null;
            _web.DefaultBackgroundColor = Color.White;
            _web.Dock = DockStyle.Fill;
            _web.Location = new Point(190, 40);
            _web.Name = "_web";
            _web.Size = new Size(910, 660);
            _web.TabIndex = 2;
            _web.ZoomFactor = 1D;
            // 
            // FormReporteUniformesPorEmpleado
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 700);
            Controls.Add(_web);
            Controls.Add(panelTop);
            Controls.Add(panelLeft);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormReporteUniformesPorEmpleado";
            Text = "Reporte de Uniformes por Empleado";
            panelLeft.ResumeLayout(false);
            panelTop.ResumeLayout(false);
            panelBotones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_web).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}
