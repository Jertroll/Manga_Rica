using System.Drawing;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Manga_Rica_P1.UI.Reportes.Shared;

namespace Manga_Rica_P1.UI.Reportes
{
    partial class FormReportePlanillaSemanal
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelSidebar;
        private Panel panelTop;
        private WebView2 webView;
        private Button btnExportPdf;
        private Button btnExportExcel;
        private Label lblHint;
        private SemanaFiltroSidebar filtroSemana;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReportePlanillaSemanal));
            panelSidebar = new Panel();
            filtroSemana = new SemanaFiltroSidebar();
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
            panelSidebar.Controls.Add(filtroSemana);
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Location = new Point(0, 0);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Padding = new Padding(0, 8, 0, 0);
            panelSidebar.Size = new Size(200, 600);
            panelSidebar.TabIndex = 0;
            // 
            // filtroSemana
            // 
            filtroSemana.BackColor = SystemColors.Control;
            filtroSemana.DataSource = null;
            filtroSemana.DisplayMember = "";
            filtroSemana.Dock = DockStyle.Top;
            filtroSemana.EtiquetaCampo = "Semana :";
            filtroSemana.Location = new Point(0, 8);
            filtroSemana.Name = "filtroSemana";
            filtroSemana.SelectedValue = null;
            filtroSemana.Size = new Size(200, 120);
            filtroSemana.TabIndex = 0;
            filtroSemana.TextoBoton = "Generar";
            filtroSemana.Titulo = "Filtro de Semana";
            filtroSemana.ValueMember = "";
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.WhiteSmoke;
            panelTop.Controls.Add(btnExportExcel);
            panelTop.Controls.Add(btnExportPdf);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(200, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(824, 40);
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
            lblHint.Location = new Point(200, 40);
            lblHint.Name = "lblHint";
            lblHint.Padding = new Padding(8);
            lblHint.Size = new Size(824, 40);
            lblHint.TabIndex = 2;
            lblHint.Text = "Seleccione una semana en el panel izquierdo y presione \"Generar reporte\".";
            // 
            // webView
            // 
            webView.AllowExternalDrop = false;
            webView.CreationProperties = null;
            webView.DefaultBackgroundColor = Color.White;
            webView.Dock = DockStyle.Fill;
            webView.Location = new Point(200, 80);
            webView.Name = "webView";
            webView.Size = new Size(824, 520);
            webView.TabIndex = 3;
            webView.ZoomFactor = 1D;
            // 
            // FormReportePlanillaSemanal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 600);
            Controls.Add(webView);
            Controls.Add(lblHint);
            Controls.Add(panelTop);
            Controls.Add(panelSidebar);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormReportePlanillaSemanal";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reporte: Planilla Semanal";
            panelSidebar.ResumeLayout(false);
            panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView).EndInit();
            ResumeLayout(false);
        }
    }
}
