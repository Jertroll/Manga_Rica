namespace Manga_Rica_P1.UI.Reportes
{
    partial class FormReporteUniformesGeneral
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReporteUniformesGeneral));
            _btnExportarPdf = new Button();
            _btnExportarExcel = new Button();
            panelSuperior = new FlowLayoutPanel();
            _web = new Microsoft.Web.WebView2.WinForms.WebView2();
            panelSuperior.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_web).BeginInit();
            SuspendLayout();
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
            // panelSuperior
            // 
            panelSuperior.BackColor = SystemColors.AppWorkspace;
            panelSuperior.Controls.Add(_btnExportarPdf);
            panelSuperior.Controls.Add(_btnExportarExcel);
            panelSuperior.Dock = DockStyle.Top;
            panelSuperior.Location = new Point(0, 0);
            panelSuperior.Name = "panelSuperior";
            panelSuperior.Size = new Size(1100, 36);
            panelSuperior.TabIndex = 2;
            // 
            // _web
            // 
            _web.AllowExternalDrop = true;
            _web.CreationProperties = null;
            _web.DefaultBackgroundColor = Color.White;
            _web.Dock = DockStyle.Fill;
            _web.Location = new Point(0, 36);
            _web.Name = "_web";
            _web.Size = new Size(1100, 664);
            _web.TabIndex = 3;
            _web.ZoomFactor = 1D;
            // 
            // FormReporteUniformesGeneral
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 700);
            Controls.Add(_web);
            Controls.Add(panelSuperior);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormReporteUniformesGeneral";
            Text = "Reporte: Uniformes General";
            panelSuperior.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_web).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button _btnExportarPdf;
        private System.Windows.Forms.Button _btnExportarExcel;
        private System.Windows.Forms.FlowLayoutPanel panelSuperior;
        private Microsoft.Web.WebView2.WinForms.WebView2 _web;
    }
}
