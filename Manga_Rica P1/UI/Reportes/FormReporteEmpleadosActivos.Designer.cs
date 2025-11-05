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
            // Nueva implementacion
            this._btnExportar = new System.Windows.Forms.Button();
            this._web = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)(this._web)).BeginInit();
            this.SuspendLayout();
            // 
            // _btnExportar
            // 
            this._btnExportar.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnExportar.Location = new System.Drawing.Point(0, 0);
            this._btnExportar.Name = "_btnExportar";
            this._btnExportar.Size = new System.Drawing.Size(1100, 36);
            this._btnExportar.TabIndex = 0;
            this._btnExportar.Text = "Exportar PDF";
            this._btnExportar.UseVisualStyleBackColor = true;
            // Nota: la suscripción al evento Click se realiza en el .cs para evitar doble suscripción.
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
            this._web.TabIndex = 1;
            this._web.ZoomFactor = 1D;
            // 
            // FormReporteEmpleadosActivos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this._web);
            this.Controls.Add(this._btnExportar);
            this.Name = "FormReporteEmpleadosActivos";
            this.Text = "Reporte: Empleados Activos";
            // Nota: el evento Load se suscribe en el .cs (constructor) para evitar doble suscripción.
            ((System.ComponentModel.ISupportInitialize)(this._web)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        // Nueva implementacion
        private System.Windows.Forms.Button _btnExportar;
        private Microsoft.Web.WebView2.WinForms.WebView2 _web;
    }
}
