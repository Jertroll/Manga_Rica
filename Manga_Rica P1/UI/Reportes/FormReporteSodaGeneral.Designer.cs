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
            panelLeft = new System.Windows.Forms.Panel();
            fechaRangoFiltroSidebar1 = new FechaRangoFiltroSidebar();
            panelTop = new System.Windows.Forms.Panel();
            panelBotones = new System.Windows.Forms.FlowLayoutPanel();
            _btnExportarPdf = new System.Windows.Forms.Button();
            _btnExportarExcel = new System.Windows.Forms.Button();
            _web = new WebView2();
            panelLeft.SuspendLayout();
            panelTop.SuspendLayout();
            panelBotones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_web).BeginInit();
            SuspendLayout();
            // 
            // panelLeft
            // 
            panelLeft.BackColor = System.Drawing.SystemColors.ControlLight;
            panelLeft.Controls.Add(fechaRangoFiltroSidebar1);
            panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            panelLeft.Location = new System.Drawing.Point(0, 0);
            panelLeft.Name = "panelLeft";
            panelLeft.Padding = new System.Windows.Forms.Padding(4);
            panelLeft.Size = new System.Drawing.Size(210, 700);
            panelLeft.TabIndex = 0;
            // 
            // fechaRangoFiltroSidebar1
            // 
            fechaRangoFiltroSidebar1.Dock = System.Windows.Forms.DockStyle.Top;
            fechaRangoFiltroSidebar1.Location = new System.Drawing.Point(4, 4);
            fechaRangoFiltroSidebar1.Name = "fechaRangoFiltroSidebar1";
            fechaRangoFiltroSidebar1.Size = new System.Drawing.Size(202, 200);
            fechaRangoFiltroSidebar1.TabIndex = 0;
            // 
            // panelTop
            // 
            panelTop.Controls.Add(panelBotones);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(210, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new System.Drawing.Size(890, 40);
            panelTop.TabIndex = 1;
            // 
            // panelBotones
            // 
            panelBotones.BackColor = System.Drawing.SystemColors.AppWorkspace;
            panelBotones.Controls.Add(_btnExportarPdf);
            panelBotones.Controls.Add(_btnExportarExcel);
            panelBotones.Dock = System.Windows.Forms.DockStyle.Fill;
            panelBotones.Location = new System.Drawing.Point(0, 0);
            panelBotones.Name = "panelBotones";
            panelBotones.Size = new System.Drawing.Size(890, 40);
            panelBotones.TabIndex = 0;
            // 
            // _btnExportarPdf
            // 
            _btnExportarPdf.Location = new System.Drawing.Point(3, 3);
            _btnExportarPdf.Name = "_btnExportarPdf";
            _btnExportarPdf.Size = new System.Drawing.Size(110, 27);
            _btnExportarPdf.TabIndex = 0;
            _btnExportarPdf.Text = "Exportar PDF";
            _btnExportarPdf.UseVisualStyleBackColor = true;
            _btnExportarPdf.Click += _btnExportarPdf_Click;
            // 
            // _btnExportarExcel
            // 
            _btnExportarExcel.Location = new System.Drawing.Point(119, 3);
            _btnExportarExcel.Name = "_btnExportarExcel";
            _btnExportarExcel.Size = new System.Drawing.Size(110, 27);
            _btnExportarExcel.TabIndex = 1;
            _btnExportarExcel.Text = "Exportar Excel";
            _btnExportarExcel.UseVisualStyleBackColor = true;
            _btnExportarExcel.Click += _btnExportarExcel_Click;
            // 
            // _web
            // 
            _web.AllowExternalDrop = true;
            _web.CreationProperties = null;
            _web.DefaultBackgroundColor = System.Drawing.Color.White;
            _web.Dock = System.Windows.Forms.DockStyle.Fill;
            _web.Location = new System.Drawing.Point(210, 40);
            _web.Name = "_web";
            _web.Size = new System.Drawing.Size(890, 660);
            _web.TabIndex = 2;
            _web.ZoomFactor = 1D;
            // 
            // FormReporteSodaGeneral
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1100, 700);
            Controls.Add(_web);
            Controls.Add(panelTop);
            Controls.Add(panelLeft);
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
