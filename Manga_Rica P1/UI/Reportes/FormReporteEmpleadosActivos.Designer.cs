// Nueva implementacion
namespace MangaRica.UI.Forms
{
    partial class FormReporteEmpleadosActivos
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            // Nueva implementacion
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. 
        /// No se puede modificar el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReporteEmpleadosActivos));
            _btnExportarPdf = new Button();
            _web = new Microsoft.Web.WebView2.WinForms.WebView2();
            panelSuperior = new FlowLayoutPanel();
            _btnExportarExcel = new Button();
            ((System.ComponentModel.ISupportInitialize)_web).BeginInit();
            panelSuperior.SuspendLayout();
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
            // _web
            // 
            _web.AllowExternalDrop = true;
            _web.CreationProperties = null;
            _web.DefaultBackgroundColor = Color.White;
            _web.Location = new Point(0, 36);
            _web.Name = "_web";
            _web.Size = new Size(1100, 664);
            _web.TabIndex = 1;
            _web.ZoomFactor = 1D;
            // 
            // panelSuperior
            // 
            panelSuperior.BackColor = SystemColors.AppWorkspace;
            panelSuperior.Controls.Add(_btnExportarPdf);
            panelSuperior.Controls.Add(_btnExportarExcel);
            panelSuperior.Location = new Point(0, 0);
            panelSuperior.Name = "panelSuperior";
            panelSuperior.Size = new Size(1100, 36);
            panelSuperior.TabIndex = 2;
            // 
            // _btnExportarExcel
            // 
            _btnExportarExcel.Location = new Point(117, 3);
            _btnExportarExcel.Name = "_btnExportarExcel";
            _btnExportarExcel.Size = new Size(108, 27);
            _btnExportarExcel.TabIndex = 3;
            _btnExportarExcel.Text = "Exportar Excel";
            _btnExportarExcel.UseVisualStyleBackColor = true;
            _btnExportarExcel.Click += _btnExportarExcel_Click;
            // 
            // FormReporteEmpleadosActivos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 700);
            Controls.Add(panelSuperior);
            Controls.Add(_web);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormReporteEmpleadosActivos";
            Text = "Reporte: Empleados Activos";
            ((System.ComponentModel.ISupportInitialize)_web).EndInit();
            panelSuperior.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        // Nueva implementacion
        private System.Windows.Forms.Button _btnExportarPdf;
        private Microsoft.Web.WebView2.WinForms.WebView2 _web;
        private FlowLayoutPanel panelSuperior;
        private Button _btnExportarExcel;
    }
}
