using System;
using System.Drawing;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Helpers
{
    public static class PopupMenus
    {
        // Botón de ítem (acción opcional)
        private static Button MakeItem(string text, Action? onClick = null, Image? icon = null)
        {
            var b = new Button
            {
                Text = "   " + text,
                TextAlign = ContentAlignment.MiddleLeft,
                Image = icon,
                ImageAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.White,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Width = 220,
                Height = 36,
                Margin = new Padding(0),
                Padding = new Padding(10, 0, 8, 0),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
            b.FlatAppearance.MouseDownBackColor = Color.FromArgb(225, 225, 225);

            // Solo ejecuta la acción; el cierre del popup lo gestionamos en ShowPopup
            b.Click += (s, e) => onClick?.Invoke();
            return b;
        }

        // Contenedor vertical
        private static Control BuildPanel(params Button[] items)
        {
            var panel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                BackColor = Color.White,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            foreach (var it in items) panel.Controls.Add(it);
            return panel;
        }

        // Mostrar popup anclado a 'anchor'
        private static void ShowPopup(Control anchor, Control content, Point offset)
        {
            var host = new ToolStripControlHost(content)
            {
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                AutoSize = false,
                BackColor = Color.White,
                Size = content.PreferredSize
            };

            var dd = new ToolStripDropDown
            {
                Padding = Padding.Empty,
                AutoClose = true,
                DropShadowEnabled = true
            };
            dd.Items.Add(host);

            // Cerrar el popup cuando se clickea cualquier Button dentro del contenido
            void WireClose(Control c)
            {
                foreach (Control child in c.Controls)
                {
                    if (child is Button btn)
                        btn.Click += (s, e) => dd.Close(ToolStripDropDownCloseReason.ItemClicked);

                    if (child.HasChildren)
                        WireClose(child);
                }
            }
            WireClose(content);

            dd.Show(anchor, offset); // derecha del botón
            // si lo prefieres debajo: dd.Show(anchor, new Point(0, anchor.Height));
        }



        // ===== Público: menú Empleados =====
        public static void ShowEmpleadosMenu(
            Control anchor,
            Action? activos = null,
            Action? noActivos = null,
            Action? uniforme = null,
            Action? sodaGeneral = null,
            Action? ausencias = null)
        {
            var b1 = MakeItem("Activos", activos   /*, Properties.Resources.list_icon*/);
            var b2 = MakeItem("No Activos", noActivos     /*, Properties.Resources.add_icon*/);
            var b3 = MakeItem("Uniforme", uniforme  /*, Properties.Resources.import_icon*/);
            var b4 = MakeItem("Soda General", sodaGeneral  /*, Properties.Resources.import_icon*/);
            var b5 = MakeItem("Ausencias", ausencias  /*, Properties.Resources.import_icon*/);

            var panel = BuildPanel(b1, b2, b3, b4, b5);
            ShowPopup(anchor, panel, new Point(anchor.Width - 40, 20));
        }

        public static void ShowPlanillaMenu(
            Control anchor,
            Action? semana = null,
            Action? comprobante = null,
            Action? horasDiarias = null,
            Action? entradaYSalida = null,
            Action? horasSemanales = null,
            Action? controlSalidas = null,
            Action? cierreDiario = null
            )
        {
            var b1 = MakeItem("Semana", semana   /*, Properties.Resources.list_icon*/);
            var b2 = MakeItem("Combrobante", comprobante     /*, Properties.Resources.add_icon*/);
            var b3 = MakeItem("Horas Diarias", horasDiarias  /*, Properties.Resources.import_icon*/);
            var b4 = MakeItem("Entradas y Salidas", entradaYSalida  /*, Properties.Resources.import_icon*/);
            var b5 = MakeItem("Horas Semanales", horasSemanales  /*, Properties.Resources.import_icon*/);
            var b6 = MakeItem("Control Salidas", controlSalidas  /*, Properties.Resources.import_icon*/);
            var b7 = MakeItem("Cierre Diario", cierreDiario  /*, Properties.Resources.import_icon*/);



            var panel = BuildPanel(b1, b2, b3, b4, b5, b6, b7);
            ShowPopup(anchor, panel, new Point(anchor.Width - 40, 20));
        }
    }
}
