using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Manga_Rica_P1.UI.Reportes.Shared;

namespace Manga_Rica_P1.UI.Reportes
{
    partial class FormReportePlanillaSemanalPorDepartamento
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelSidebar;
        private Panel panelTop;
        private WebView2 webView;
        private Button btnExportPdf;
        private Button btnExportExcel;
        private Label lblHint;
        private SemanaDepartamentoFiltroSidebar filtroSemanaDepartamento;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelSidebar = new System.Windows.Forms.Panel();
            filtroSemanaDepartamento = new SemanaDepartamentoFiltroSidebar();
            panelTop = new System.Windows.Forms.Panel();
            btnExportExcel = new System.Windows.Forms.Button();
            btnExportPdf = new System.Windows.Forms.Button();
            lblHint = new System.Windows.Forms.Label();
            webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            panelSidebar.SuspendLayout();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
            this.SuspendLayout();
            // 
            // panelSidebar
            // 
            panelSidebar.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            panelSidebar.Controls.Add(filtroSemanaDepartamento);
            panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            panelSidebar.Location = new System.Drawing.Point(0, 0);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            panelSidebar.Size = new System.Drawing.Size(200, 600);
            panelSidebar.TabIndex = 0;
            // 
            // filtroSemanaDepartamento
            // 
            filtroSemanaDepartamento.BackColor = System.Drawing.SystemColors.Control;
            filtroSemanaDepartamento.Dock = System.Windows.Forms.DockStyle.Top;
            filtroSemanaDepartamento.Location = new System.Drawing.Point(0, 8);
            filtroSemanaDepartamento.Name = "filtroSemanaDepartamento";
            filtroSemanaDepartamento.Size = new System.Drawing.Size(200, 170);
            filtroSemanaDepartamento.TabIndex = 0;
            // 
            // panelTop
            // 
            panelTop.BackColor = System.Drawing.Color.WhiteSmoke;
            panelTop.Controls.Add(btnExportExcel);
            panelTop.Controls.Add(btnExportPdf);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(200, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new System.Drawing.Size(824, 40);
            panelTop.TabIndex = 1;
            // 
            // btnExportExcel
            // 
            btnExportExcel.Location = new System.Drawing.Point(118, 8);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new System.Drawing.Size(100, 24);
            btnExportExcel.TabIndex = 1;
            btnExportExcel.Text = "Exportar Excel";
            btnExportExcel.UseVisualStyleBackColor = true;
            btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnExportPdf
            // 
            btnExportPdf.Location = new System.Drawing.Point(12, 8);
            btnExportPdf.Name = "btnExportPdf";
            btnExportPdf.Size = new System.Drawing.Size(100, 24);
            btnExportPdf.TabIndex = 0;
            btnExportPdf.Text = "Exportar PDF";
            btnExportPdf.UseVisualStyleBackColor = true;
            btnExportPdf.Click += new System.EventHandler(this.btnExportPdf_Click);
            // 
            // lblHint
            // 
            lblHint.Dock = System.Windows.Forms.DockStyle.Top;
            lblHint.Location = new System.Drawing.Point(200, 40);
            lblHint.Name = "lblHint";
            lblHint.Padding = new System.Windows.Forms.Padding(8);
            lblHint.Size = new System.Drawing.Size(824, 40);
            lblHint.TabIndex = 2;
            lblHint.Text = "Seleccione una semana y un departamento en el panel izquierdo y presione \"Generar reporte\".";
            // 
            // webView
            // 
            webView.AllowExternalDrop = false;
            webView.CreationProperties = null;
            webView.DefaultBackgroundColor = System.Drawing.Color.White;
            webView.Dock = System.Windows.Forms.DockStyle.Fill;
            webView.Location = new System.Drawing.Point(200, 80);
            webView.Name = "webView";
            webView.Size = new System.Drawing.Size(824, 520);
            webView.TabIndex = 3;
            webView.ZoomFactor = 1D;
            // 
            // FormReportePlanillaSemanalPorDepartamento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 600);
            this.Controls.Add(webView);
            this.Controls.Add(lblHint);
            this.Controls.Add(panelTop);
            this.Controls.Add(panelSidebar);
            this.Name = "FormReportePlanillaSemanalPorDepartamento";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reporte: Planilla Semanal por Departamento";
            panelSidebar.ResumeLayout(false);
            panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView).EndInit();
            this.ResumeLayout(false);
        }
    }
}
