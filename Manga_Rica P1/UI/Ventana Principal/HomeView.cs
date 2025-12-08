using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Ventana_Principal
{
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void linkLabelManual_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // Ajusta la ruta si tu PDF está en otra carpeta
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string pdfPath = Path.Combine(baseDir, "Manual_Usuario.pdf");

                if (!File.Exists(pdfPath))
                {
                    MessageBox.Show(
                        $"No se encontró el archivo:\n{pdfPath}",
                        "Manual de usuario",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = pdfPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo abrir el manual de usuario.\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


    }
}
