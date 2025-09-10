using System.Drawing;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Helpers
{
    public static class GridStyler
    {
        public static void ApplyDefault(DataGridView g)
        {
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.BackgroundColor = Color.White;
            g.BorderStyle = BorderStyle.None;
            g.ReadOnly = true;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = true;
            g.AllowUserToAddRows = false;
            g.AllowUserToDeleteRows = false;
            g.RowHeadersVisible = false;

            // === ENCABEZADOS ===
            g.EnableHeadersVisualStyles = false;
            var hdr = g.ColumnHeadersDefaultCellStyle;
            hdr.BackColor = Color.FromArgb(50, 130, 56);
            hdr.ForeColor = Color.White;
            hdr.Alignment = DataGridViewContentAlignment.MiddleCenter;
            hdr.Font = new Font("Segoe UI Semibold", 11.25f, FontStyle.Bold);

            // 🔑 Clave: selección igual al color normal, así "no cambia"
            hdr.SelectionBackColor = hdr.BackColor;
            hdr.SelectionForeColor = hdr.ForeColor;

            // === FILAS ===
            var rows = g.RowsDefaultCellStyle;
            rows.BackColor = Color.FromArgb(255, 255, 255); // gris suave
            rows.ForeColor = Color.Black;
            rows.SelectionBackColor = Color.FromArgb(224, 224, 224);
            rows.SelectionForeColor = Color.Black;
            rows.Font = new Font("Segoe UI", 10f, FontStyle.Regular);

            // Alternar filas (opcional)
            //g.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
        }

    }
}
