using System.Drawing;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Manga_Rica_P1.UI.Reportes.Shared;

namespace Manga_Rica_P1.UI.Reportes
{
    partial class FormReporteEntradasSalidas
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelSidebar;
        private Panel panelTop;
        private WebView2 webView;
        private Button btnExportPdf;
        private Button btnExportExcel;
        private Label lblHint;
        private EmpleadoFechaFiltroSidebar empleadoFechaFiltro;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReporteEntradasSalidas));
            panelSidebar = new Panel();
            empleadoFechaFiltro = new EmpleadoFechaFiltroSidebar();
            panelTop = new Panel();
            btnExportExcel = new Button();
            btnExportPdf = new Button();
            lblHint = new Label();
            webView = new WebView2();
            panelSidebar.SuspendLayout();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
            SuspendLayout();
            // 
            // panelSidebar
            // 
            panelSidebar.BackColor = Color.FromArgb(245, 245, 245);
            panelSidebar.Controls.Add(empleadoFechaFiltro);
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Location = new Point(0, 0);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Padding = new Padding(0, 8, 0, 0);
            panelSidebar.Size = new Size(220, 600);
            panelSidebar.TabIndex = 0;
            // 
            // empleadoFechaFiltro
            // 
            empleadoFechaFiltro.BackColor = SystemColors.Control;
            empleadoFechaFiltro.CarneTexto = "";
            empleadoFechaFiltro.Dock = DockStyle.Top;
            empleadoFechaFiltro.FechaDesde = new DateTime(2025, 12, 5, 0, 0, 0, 0);
            empleadoFechaFiltro.FechaHasta = new DateTime(2025, 12, 5, 0, 0, 0, 0);
            empleadoFechaFiltro.Location = new Point(0, 8);
            empleadoFechaFiltro.Name = "empleadoFechaFiltro";
            empleadoFechaFiltro.Size = new Size(220, 205);
            empleadoFechaFiltro.TabIndex = 0;
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.WhiteSmoke;
            panelTop.Controls.Add(btnExportExcel);
            panelTop.Controls.Add(btnExportPdf);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(220, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(804, 40);
            panelTop.TabIndex = 1;
            // 
            // btnExportExcel
            // 
            btnExportExcel.Location = new Point(118, 8);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(100, 24);
            btnExportExcel.TabIndex = 1;
            btnExportExcel.Text = "Exportar Excel";
            btnExportExcel.UseVisualStyleBackColor = true;
            btnExportExcel.Click += btnExportExcel_Click;
            // 
            // btnExportPdf
            // 
            btnExportPdf.Location = new Point(12, 8);
            btnExportPdf.Name = "btnExportPdf";
            btnExportPdf.Size = new Size(100, 24);
            btnExportPdf.TabIndex = 0;
            btnExportPdf.Text = "Exportar PDF";
            btnExportPdf.UseVisualStyleBackColor = true;
            btnExportPdf.Click += btnExportPdf_Click;
            // 
            // lblHint
            // 
            lblHint.Dock = DockStyle.Top;
            lblHint.Location = new Point(220, 40);
            lblHint.Name = "lblHint";
            lblHint.Padding = new Padding(8);
            lblHint.Size = new Size(804, 40);
            lblHint.TabIndex = 2;
            lblHint.Text = "Seleccione un rango de fechas y, opcionalmente, un carné en el panel izquierdo y presione \"Generar reporte\".";
            // 
            // webView
            // 
            webView.AllowExternalDrop = false;
            webView.CreationProperties = null;
            webView.DefaultBackgroundColor = Color.White;
            webView.Dock = DockStyle.Fill;
            webView.Location = new Point(220, 80);
            webView.Name = "webView";
            webView.Size = new Size(804, 520);
            webView.TabIndex = 3;
            webView.ZoomFactor = 1D;
            // 
            // FormReporteEntradasSalidas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 600);
            Controls.Add(webView);
            Controls.Add(lblHint);
            Controls.Add(panelTop);
            Controls.Add(panelSidebar);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormReporteEntradasSalidas";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reporte: Entradas y Salidas";
            panelSidebar.ResumeLayout(false);
            panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView).EndInit();
            ResumeLayout(false);
        }
    }
}
