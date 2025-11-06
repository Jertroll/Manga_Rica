using System;
using System.Drawing;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Helpers
{
    public static class PopupMenus
    {
        // Variable estática para rastrear los menús abiertos
        private static ToolStripDropDown? _currentMainMenu;
        private static ToolStripDropDown? _currentSubMenu;

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

        // Botón de ítem con indicador de submenú (flecha derecha)
        private static Button MakeItemWithSubmenu(string text, Action? onHover = null, Image? icon = null)
        {
            var b = new Button
            {
                Text = "   " + text + "     ▶",
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

            // Ejecuta la acción en hover o click
            if (onHover != null)
            {
                b.MouseEnter += (s, e) => onHover.Invoke();
                b.Click += (s, e) => onHover.Invoke();
            }
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
        private static ToolStripDropDown ShowPopup(Control anchor, Control content, Point offset, bool isMainMenu = true)
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
                AutoClose = true, // Siempre true para que se cierre al hacer clic fuera
                DropShadowEnabled = true
            };
            dd.Items.Add(host);

            // Cerrar el popup cuando se clickea cualquier Button dentro del contenido
            void WireClose(Control c)
            {
                foreach (Control child in c.Controls)
                {
                    if (child is Button btn)
                    {
                        // Solo cerrar si no es un botón con submenú
                        if (!btn.Text.Contains("▶"))
                        {
                            btn.Click += (s, e) =>
                            {
                                // Cerrar ambos menús (principal y submenú)
                                CloseAllMenus();
                            };
                        }
                    }

                    if (child.HasChildren)
                        WireClose(child);
                }
            }
            WireClose(content);

            // Registrar el menú según sea principal o submenú
            if (isMainMenu)
            {
                // Cerrar cualquier menú previo antes de abrir uno nuevo
                CloseAllMenus();
                _currentMainMenu = dd;
                
                // Cuando se cierra el menú principal, cerrar también el submenú
                dd.Closed += (s, e) =>
                {
                    if (_currentSubMenu != null && !_currentSubMenu.IsDisposed)
                    {
                        _currentSubMenu.Close();
                    }
                    _currentMainMenu = null;
                };
            }
            else
            {
                // Cerrar submenú anterior si existe
                if (_currentSubMenu != null && !_currentSubMenu.IsDisposed)
                {
                    _currentSubMenu.Close();
                }
                _currentSubMenu = dd;
                
                dd.Closed += (s, e) => { _currentSubMenu = null; };
            }

            dd.Show(anchor, offset);
            return dd;
        }

        // Método auxiliar para cerrar todos los menús
        private static void CloseAllMenus()
        {
            if (_currentSubMenu != null && !_currentSubMenu.IsDisposed)
            {
                _currentSubMenu.Close();
                _currentSubMenu = null;
            }
            if (_currentMainMenu != null && !_currentMainMenu.IsDisposed)
            {
                _currentMainMenu.Close();
                _currentMainMenu = null;
            }
        }

        // ===== Público: menú Uniforme (submenú anidado) =====
        // CONFIGURACIÓN DE POSICIÓN: El parámetro Point en ShowPopup controla dónde aparece el submenú
        // - anchor.Width = a la derecha del botón
        // - 0 = alineado con la parte superior del botón
        // Puedes ajustar estos valores para cambiar la posición
        public static void ShowUniformeMenu(
            Control anchor,
            Action? general = null,
            Action? porArticulo = null,
            Action? porEmpleado = null)
        {
            var b1 = MakeItem("General", general);
            var b2 = MakeItem("Por Artículo", porArticulo);
            var b3 = MakeItem("Por Empleado", porEmpleado);

            var panel = BuildPanel(b1, b2, b3);
            
            // ⚙️ CONFIGURACIÓN DE POSICIÓN DEL SUBMENÚ:
            // new Point(anchor.Width, 0) = a la derecha del botón, alineado arriba
            // Alternativas:
            // - new Point(anchor.Width, -10) = un poco más arriba
            // - new Point(anchor.Width + 5, 0) = más separado a la derecha
            // - new Point(anchor.Width, anchor.Height / 2) = centrado verticalmente
            ShowPopup(anchor, panel, new Point(anchor.Width + 5, 0), isMainMenu: false);
        }

        // ===== Público: menú Empleados =====
        public static void ShowEmpleadosMenu(
            Control anchor,
            Action? activos = null,
            Action? noActivos = null,
            Action? uniformeGeneral = null,
            Action? uniformePorArticulo = null,
            Action? uniformePorEmpleado = null,
            Action? sodaGeneral = null,
            Action? ausencias = null)
        {
            var b1 = MakeItem("Activos", activos);
            var b2 = MakeItem("No Activos", noActivos);
            
            // Botón con submenú para Uniforme
            Button b3 = null!;
            b3 = MakeItemWithSubmenu("Uniforme", () => 
            {
                ShowUniformeMenu(b3, uniformeGeneral, uniformePorArticulo, uniformePorEmpleado);
            });
            
            var b4 = MakeItem("Soda General", sodaGeneral);
            var b5 = MakeItem("Ausencias", ausencias);

            var panel = BuildPanel(b1, b2, b3, b4, b5);
            ShowPopup(anchor, panel, new Point(anchor.Width - 40, 20), isMainMenu: true);
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
            var b1 = MakeItem("Semana", semana);
            var b2 = MakeItem("Combrobante", comprobante);
            var b3 = MakeItem("Horas Diarias", horasDiarias);
            var b4 = MakeItem("Entradas y Salidas", entradaYSalida);
            var b5 = MakeItem("Horas Semanales", horasSemanales);
            var b6 = MakeItem("Control Salidas", controlSalidas);
            var b7 = MakeItem("Cierre Diario", cierreDiario);

            var panel = BuildPanel(b1, b2, b3, b4, b5, b6, b7);
            ShowPopup(anchor, panel, new Point(anchor.Width - 40, 20), isMainMenu: true);
        }
    }
}
