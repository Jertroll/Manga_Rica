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
            this.panelLeft = new System.Windows.Forms.Panel();
            this.empleadoFiltroSidebar1 = new Manga_Rica_P1.UI.Reportes.Shared.EmpleadoFiltroSidebar();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelBotones = new System.Windows.Forms.FlowLayoutPanel();
            this._btnExportarPdf = new System.Windows.Forms.Button();
            this._btnExportarExcel = new System.Windows.Forms.Button();
            this._web = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.panelLeft.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelBotones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._web)).BeginInit();
            this.SuspendLayout();
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelLeft.Controls.Add(this.empleadoFiltroSidebar1);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Padding = new System.Windows.Forms.Padding(4);
            this.panelLeft.Size = new System.Drawing.Size(190, 700);
            this.panelLeft.TabIndex = 0;
            // 
            // empleadoFiltroSidebar1
            // 
            this.empleadoFiltroSidebar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.empleadoFiltroSidebar1.Location = new System.Drawing.Point(4, 4);
            this.empleadoFiltroSidebar1.Name = "empleadoFiltroSidebar1";
            this.empleadoFiltroSidebar1.Size = new System.Drawing.Size(182, 150);
            this.empleadoFiltroSidebar1.TabIndex = 0;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panelBotones);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(190, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(910, 40);
            this.panelTop.TabIndex = 1;
            // 
            // panelBotones
            // 
            this.panelBotones.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panelBotones.Controls.Add(this._btnExportarPdf);
            this.panelBotones.Controls.Add(this._btnExportarExcel);
            this.panelBotones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBotones.Location = new System.Drawing.Point(0, 0);
            this.panelBotones.Name = "panelBotones";
            this.panelBotones.Size = new System.Drawing.Size(910, 40);
            this.panelBotones.TabIndex = 0;
            // 
            // _btnExportarPdf
            // 
            this._btnExportarPdf.Location = new System.Drawing.Point(3, 3);
            this._btnExportarPdf.Name = "_btnExportarPdf";
            this._btnExportarPdf.Size = new System.Drawing.Size(108, 27);
            this._btnExportarPdf.TabIndex = 0;
            this._btnExportarPdf.Text = "Exportar PDF";
            this._btnExportarPdf.UseVisualStyleBackColor = true;
            this._btnExportarPdf.Click += new System.EventHandler(this._btnExportarPdf_Click);
            // 
            // _btnExportarExcel
            // 
            this._btnExportarExcel.Location = new System.Drawing.Point(117, 3);
            this._btnExportarExcel.Name = "_btnExportarExcel";
            this._btnExportarExcel.Size = new System.Drawing.Size(108, 27);
            this._btnExportarExcel.TabIndex = 1;
            this._btnExportarExcel.Text = "Exportar Excel";
            this._btnExportarExcel.UseVisualStyleBackColor = true;
            this._btnExportarExcel.Click += new System.EventHandler(this._btnExportarExcel_Click);
            // 
            // _web
            // 
            this._web.AllowExternalDrop = true;
            this._web.CreationProperties = null;
            this._web.DefaultBackgroundColor = System.Drawing.Color.White;
            this._web.Dock = System.Windows.Forms.DockStyle.Fill;
            this._web.Location = new System.Drawing.Point(190, 40);
            this._web.Name = "_web";
            this._web.Size = new System.Drawing.Size(910, 660);
            this._web.TabIndex = 2;
            this._web.ZoomFactor = 1D;
            // 
            // FormReporteUniformesPorEmpleado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this._web);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelLeft);
            this.Name = "FormReporteUniformesPorEmpleado";
            this.Text = "Reporte de Uniformes por Empleado";
            this.panelLeft.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelBotones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._web)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
    }
}
