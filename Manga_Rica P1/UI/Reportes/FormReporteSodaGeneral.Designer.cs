using Microsoft.Web.WebView2.WinForms;
using Manga_Rica_P1.UI.Reportes.Shared;

namespace Manga_Rica_P1.UI.Reportes
{
    partial class FormReporteSodaGeneral
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelLeft;
        private FechaRangoFiltroSidebar fechaRangoFiltroSidebar1;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.FlowLayoutPanel panelBotones;
        private System.Windows.Forms.Button _btnExportarPdf;
        private System.Windows.Forms.Button _btnExportarExcel;
        private WebView2 _web;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReporteSodaGeneral));
            panelLeft = new Panel();
            fechaRangoFiltroSidebar1 = new FechaRangoFiltroSidebar();
            panelTop = new Panel();
            panelBotones = new FlowLayoutPanel();
            _btnExportarPdf = new Button();
            _btnExportarExcel = new Button();
            _web = new WebView2();
            panelLeft.SuspendLayout();
            panelTop.SuspendLayout();
            panelBotones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_web).BeginInit();
            SuspendLayout();
            // 
            // panelLeft
            // 
            panelLeft.BackColor = SystemColors.ControlLight;
            panelLeft.Controls.Add(fechaRangoFiltroSidebar1);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Name = "panelLeft";
            panelLeft.Padding = new Padding(4);
            panelLeft.Size = new Size(210, 700);
            panelLeft.TabIndex = 0;
            // 
            // fechaRangoFiltroSidebar1
            // 
            fechaRangoFiltroSidebar1.BackColor = SystemColors.Control;
            fechaRangoFiltroSidebar1.Dock = DockStyle.Top;
            fechaRangoFiltroSidebar1.FechaDesde = new DateTime(2025, 12, 7, 0, 0, 0, 0);
            fechaRangoFiltroSidebar1.FechaHasta = new DateTime(2025, 12, 7, 0, 0, 0, 0);
            fechaRangoFiltroSidebar1.Location = new Point(4, 4);
            fechaRangoFiltroSidebar1.Name = "fechaRangoFiltroSidebar1";
            fechaRangoFiltroSidebar1.Size = new Size(202, 200);
            fechaRangoFiltroSidebar1.TabIndex = 0;
            fechaRangoFiltroSidebar1.TextoBoton = "Generar reporte";
            // 
            // panelTop
            // 
            panelTop.Controls.Add(panelBotones);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(210, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(890, 40);
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
            panelBotones.Size = new Size(890, 40);
            panelBotones.TabIndex = 0;
            // 
            // _btnExportarPdf
            // 
            _btnExportarPdf.Location = new Point(3, 3);
            _btnExportarPdf.Name = "_btnExportarPdf";
            _btnExportarPdf.Size = new Size(110, 27);
            _btnExportarPdf.TabIndex = 0;
            _btnExportarPdf.Text = "Exportar PDF";
            _btnExportarPdf.UseVisualStyleBackColor = true;
            _btnExportarPdf.Click += _btnExportarPdf_Click;
            // 
            // _btnExportarExcel
            // 
            _btnExportarExcel.Location = new Point(119, 3);
            _btnExportarExcel.Name = "_btnExportarExcel";
            _btnExportarExcel.Size = new Size(110, 27);
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
            _web.Location = new Point(210, 40);
            _web.Name = "_web";
            _web.Size = new Size(890, 660);
            _web.TabIndex = 2;
            _web.ZoomFactor = 1D;
            // 
            // FormReporteSodaGeneral
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 700);
            Controls.Add(_web);
            Controls.Add(panelTop);
            Controls.Add(panelLeft);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormReporteSodaGeneral";
            Text = "Reporte de Soda General";
            panelLeft.ResumeLayout(false);
            panelTop.ResumeLayout(false);
            panelBotones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_web).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}
