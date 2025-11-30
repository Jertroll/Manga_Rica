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
            this._btnExportarPdf = new System.Windows.Forms.Button();
            this._btnExportarExcel = new System.Windows.Forms.Button();
            this.panelSuperior = new System.Windows.Forms.FlowLayoutPanel();
            this._web = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)(this._web)).BeginInit();
            this.panelSuperior.SuspendLayout();
            this.SuspendLayout();
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
            // panelSuperior
            // 
            this.panelSuperior.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panelSuperior.Controls.Add(this._btnExportarPdf);
            this.panelSuperior.Controls.Add(this._btnExportarExcel);
            this.panelSuperior.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSuperior.Location = new System.Drawing.Point(0, 0);
            this.panelSuperior.Name = "panelSuperior";
            this.panelSuperior.Size = new System.Drawing.Size(1100, 36);
            this.panelSuperior.TabIndex = 2;
            // 
            // _web
            // 
            this._web.AllowExternalDrop = true;
            this._web.CreationProperties = null;
            this._web.DefaultBackgroundColor = System.Drawing.Color.White;
            this._web.Dock = System.Windows.Forms.DockStyle.Fill;
            this._web.Location = new System.Drawing.Point(0, 36);
            this._web.Name = "_web";
            this._web.Size = new System.Drawing.Size(1100, 664);
            this._web.TabIndex = 3;
            this._web.ZoomFactor = 1D;
            // 
            // FormReporteUniformesGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this._web);
            this.Controls.Add(this.panelSuperior);
            this.Name = "FormReporteUniformesGeneral";
            this.Text = "Reporte: Uniformes General";
            ((System.ComponentModel.ISupportInitialize)(this._web)).EndInit();
            this.panelSuperior.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button _btnExportarPdf;
        private System.Windows.Forms.Button _btnExportarExcel;
        private System.Windows.Forms.FlowLayoutPanel panelSuperior;
        private Microsoft.Web.WebView2.WinForms.WebView2 _web;
    }
}
