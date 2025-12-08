using Microsoft.Web.WebView2.WinForms;

namespace Manga_Rica_P1.UI.Reportes
{
    partial class FormReporteHorasSemanales
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Manga_Rica_P1.UI.Reportes.Shared.FechaRangoFiltroSidebar fechaRangoFiltro;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Panel panelSidebar;

        /// <summary>
        /// Limpiar recursos.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReporteHorasSemanales));
            panelSidebar = new Panel();
            fechaRangoFiltro = new Manga_Rica_P1.UI.Reportes.Shared.FechaRangoFiltroSidebar();
            panelRight = new Panel();
            webView = new WebView2();
            panelTop = new Panel();
            btnExportExcel = new Button();
            btnExportPdf = new Button();
            panelSidebar.SuspendLayout();
            panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
            panelTop.SuspendLayout();
            SuspendLayout();
            // 
            // panelSidebar
            // 
            panelSidebar.BackColor = Color.WhiteSmoke;
            panelSidebar.Controls.Add(fechaRangoFiltro);
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Location = new Point(0, 0);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Padding = new Padding(4);
            panelSidebar.Size = new Size(230, 561);
            panelSidebar.TabIndex = 0;
            // 
            // fechaRangoFiltro
            // 
            fechaRangoFiltro.BackColor = SystemColors.Control;
            fechaRangoFiltro.Dock = DockStyle.Fill;
            fechaRangoFiltro.FechaDesde = new DateTime(2025, 12, 7, 0, 0, 0, 0);
            fechaRangoFiltro.FechaHasta = new DateTime(2025, 12, 7, 0, 0, 0, 0);
            fechaRangoFiltro.Location = new Point(4, 4);
            fechaRangoFiltro.Name = "fechaRangoFiltro";
            fechaRangoFiltro.Size = new Size(222, 553);
            fechaRangoFiltro.TabIndex = 0;
            fechaRangoFiltro.TextoBoton = "Generar reporte";
            // 
            // panelRight
            // 
            panelRight.Controls.Add(webView);
            panelRight.Controls.Add(panelTop);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(230, 0);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(654, 561);
            panelRight.TabIndex = 1;
            // 
            // webView
            // 
            webView.AllowExternalDrop = true;
            webView.CreationProperties = null;
            webView.DefaultBackgroundColor = Color.White;
            webView.Dock = DockStyle.Fill;
            webView.Location = new Point(0, 40);
            webView.Name = "webView";
            webView.Size = new Size(654, 521);
            webView.TabIndex = 1;
            webView.ZoomFactor = 1D;
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.White;
            panelTop.BorderStyle = BorderStyle.FixedSingle;
            panelTop.Controls.Add(btnExportExcel);
            panelTop.Controls.Add(btnExportPdf);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(654, 40);
            panelTop.TabIndex = 0;
            // 
            // btnExportExcel
            // 
            btnExportExcel.Anchor = AnchorStyles.Left;
            btnExportExcel.FlatStyle = FlatStyle.Flat;
            btnExportExcel.Location = new Point(104, 7);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(90, 26);
            btnExportExcel.TabIndex = 1;
            btnExportExcel.Text = "Exportar Excel";
            btnExportExcel.UseVisualStyleBackColor = true;
            btnExportExcel.Click += btnExportExcel_Click;
            // 
            // btnExportPdf
            // 
            btnExportPdf.Anchor = AnchorStyles.Left;
            btnExportPdf.FlatStyle = FlatStyle.Flat;
            btnExportPdf.Location = new Point(8, 7);
            btnExportPdf.Name = "btnExportPdf";
            btnExportPdf.Size = new Size(90, 26);
            btnExportPdf.TabIndex = 0;
            btnExportPdf.Text = "Exportar PDF";
            btnExportPdf.UseVisualStyleBackColor = true;
            btnExportPdf.Click += btnExportPdf_Click;
            // 
            // FormReporteHorasSemanales
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 561);
            Controls.Add(panelRight);
            Controls.Add(panelSidebar);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormReporteHorasSemanales";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reporte: Horas Semanales";
            panelSidebar.ResumeLayout(false);
            panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView).EndInit();
            panelTop.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
    }
}
