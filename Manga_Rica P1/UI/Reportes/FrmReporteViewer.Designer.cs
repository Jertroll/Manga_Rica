// Nueva implementacion - Designer
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace Manga_Rica_P1.UI.Reportes
{
    partial class FrmReporteViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Controles (visibles también en el .cs)
        private Panel panelTop;
        private ComboBox cmbFormato;
        private Button btnRegenerar;
        private TextBox txtBuscar;
        private Button btnBuscar;
        private Button btnPrev;
        private Button btnNext;
        private WebView2 web;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Nueva implementacion (coincide con el flujo Async interno)
        /// </summary>
        private void InitializeComponent()
        {
            panelTop = new Panel();
            btnNext = new Button();
            btnPrev = new Button();
            btnBuscar = new Button();
            txtBuscar = new TextBox();
            btnRegenerar = new Button();
            cmbFormato = new ComboBox();
            web = new WebView2();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)web).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.White;
            panelTop.Controls.Add(btnNext);
            panelTop.Controls.Add(btnPrev);
            panelTop.Controls.Add(btnBuscar);
            panelTop.Controls.Add(txtBuscar);
            panelTop.Controls.Add(btnRegenerar);
            panelTop.Controls.Add(cmbFormato);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(8);
            panelTop.Size = new Size(1084, 44);
            panelTop.TabIndex = 1;
            // 
            // btnNext
            // 
            btnNext.Dock = DockStyle.Right;
            btnNext.Location = new Point(996, 8);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(40, 28);
            btnNext.TabIndex = 5;
            btnNext.Text = "▶";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // btnPrev
            // 
            btnPrev.Dock = DockStyle.Right;
            btnPrev.Location = new Point(1036, 8);
            btnPrev.Margin = new Padding(4, 0, 4, 0);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(40, 28);
            btnPrev.TabIndex = 4;
            btnPrev.Text = "◀";
            btnPrev.UseVisualStyleBackColor = true;
            btnPrev.Click += btnPrev_Click;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(802, 10);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(80, 23);
            btnBuscar.TabIndex = 3;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(230, 10);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Buscar (solo HTML)";
            txtBuscar.Size = new Size(554, 23);
            txtBuscar.TabIndex = 2;
            // 
            // btnRegenerar
            // 
            btnRegenerar.Location = new Point(129, 10);
            btnRegenerar.Margin = new Padding(8, 0, 0, 0);
            btnRegenerar.Name = "btnRegenerar";
            btnRegenerar.Size = new Size(90, 23);
            btnRegenerar.TabIndex = 1;
            btnRegenerar.Text = "Generar";
            btnRegenerar.UseVisualStyleBackColor = true;
            btnRegenerar.Click += btnRegenerar_Click;
            // 
            // cmbFormato
            // 
            cmbFormato.Dock = DockStyle.Left;
            cmbFormato.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFormato.Items.AddRange(new object[] { "HTML", "PDF" });
            cmbFormato.Location = new Point(8, 8);
            cmbFormato.Name = "cmbFormato";
            cmbFormato.Size = new Size(110, 23);
            cmbFormato.TabIndex = 0;
            // 
            // web
            // 
            web.AllowExternalDrop = true;
            web.CreationProperties = null;
            web.DefaultBackgroundColor = Color.White;
            web.Dock = DockStyle.Fill;
            web.Location = new Point(0, 44);
            web.Name = "web";
            web.Size = new Size(1084, 667);
            web.TabIndex = 6;
            web.ZoomFactor = 1D;
            // 
            // FrmReporteViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1084, 711);
            Controls.Add(web);
            Controls.Add(panelTop);
            KeyPreview = true;
            Name = "FrmReporteViewer";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Visor de Reportes";
            WindowState = FormWindowState.Maximized;
            KeyDown += FrmReporteViewer_KeyDown;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)web).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}
