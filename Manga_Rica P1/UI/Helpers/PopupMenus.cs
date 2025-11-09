using System;
using System.Drawing;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Helpers
{
    public static class PopupMenus
    {
        // Referencias del menú padre y submenú actualmente visibles
        private static ToolStripDropDown? _openParent;
        private static ToolStripDropDown? _openSubmenu;

        // ---- Utilidades ------------------------------------------------------

        private static Button MakeItem(string text, Action? onClick = null, Image? icon = null, bool hasSubmenu = false)
        {
            var displayText = hasSubmenu ? $"   {text}    >" : $"   {text}";
            var b = new Button
            {
                Text = displayText,
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

            // Marca para indicar que este botón abre submenú y NO debe cerrar el padre en ese click
            if (hasSubmenu) b.Tag = "no-close";

            b.Click += (s, e) => onClick?.Invoke();
            return b;
        }

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

        private static void CloseIfOpen(ref ToolStripDropDown? dd)
        {
            try
            {
                if (dd is { IsDisposed: false, Visible: true })
                    dd.Close(ToolStripDropDownCloseReason.CloseCalled);
            }
            catch { /* noop */ }
            dd = null;
        }

        // ---- Núcleo de despliegue -------------------------------------------

        private static ToolStripDropDown ShowPopup(Control anchor, Control content, Point offset, Action? onCloseParent = null)
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

            // Solo queremos suprimir el cierre del padre en el click que abre el submenú.
            bool suppressCloseOnce = false;

            dd.Closing += (s, e) =>
            {
                // Si venimos del click del botón "no-close" (Uniforme), suprime este cierre una única vez.
                if (suppressCloseOnce &&
                    (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked ||
                     e.CloseReason == ToolStripDropDownCloseReason.AppClicked ||
                     e.CloseReason == ToolStripDropDownCloseReason.AppFocusChange))
                {
                    e.Cancel = true;      // mantiene abierto el padre SOLO por ese click
                }

                // Reset inmediato: cualquier cierre posterior (click fuera, Escape, etc.) debe proceder.
                suppressCloseOnce = false;
            };

            void WireClose(Control c)
            {
                foreach (Control child in c.Controls)
                {
                    if (child is Button btn)
                    {
                        // Si este botón es de submenú, marcar el "one-shot" justo antes del Click
                        btn.MouseDown += (s, e) =>
                        {
                            if (Equals(btn.Tag, "no-close"))
                                suppressCloseOnce = true;
                        };

                        btn.Click += (s, e) =>
                        {
                            // Cerrar el dropdown actual solo si NO es botón de submenú
                            if (!Equals(btn.Tag, "no-close"))
                            {
                                dd.Close(ToolStripDropDownCloseReason.ItemClicked);
                                onCloseParent?.Invoke();
                            }
                        };
                    }

                    if (child.HasChildren) WireClose(child);
                }
            }
            WireClose(content);

            dd.Show(anchor, offset);
            return dd;
        }

        // ---- Menú Empleados (con submenú Uniforme) --------------------------

        public static void ShowEmpleadosMenu(
            Control anchor,
            Action? activos = null,
            Action? noActivos = null,
            Action<Button, Action>? uniformeSubmenu = null, // (botón Uniforme, action para cerrar el padre)
            Action? sodaGeneral = null,
            Action? ausencias = null)
        {
            // Garantiza que no queden menús colgados si el usuario vuelve a abrir
            CloseIfOpen(ref _openSubmenu);
            CloseIfOpen(ref _openParent);

            var b1 = MakeItem("Activos", activos);
            var b2 = MakeItem("No Activos", noActivos);
            var b3 = MakeItem("Uniforme", null, null, hasSubmenu: true); // abre submenú
            var b4 = MakeItem("Soda General", sodaGeneral);
            var b5 = MakeItem("Ausencias", ausencias);

            var panel = BuildPanel(b1, b2, b3, b4, b5);

            // Abrimos y retenemos referencia del padre
            _openParent = ShowPopup(anchor, panel, new Point(anchor.Width - 40, 20));

            if (uniformeSubmenu != null)
            {
                b3.Click += (s, e) =>
                {
                    // Abrir/rehusar submenú a la derecha del botón Uniforme
                    uniformeSubmenu(
                        b3,
                        () => CloseIfOpen(ref _openParent)   // cómo cerrar el padre cuando se elija una opción
                    );
                };
            }

            // Si el padre se cierra por cualquier motivo, cierra el submenú también
            _openParent.Closed += (s, e) =>
            {
                CloseIfOpen(ref _openSubmenu);
            };
        }

        // ---- Submenú Uniforme -----------------------------------------------

        public static ToolStripDropDown ShowUniformeMenu(
            Control anchor,
            Action? general = null,
            Action? porArticulo = null,
            Action? porEmpleado = null,
            Action? onCloseParent = null)
        {
            // Antes de abrir uno nuevo, cerrar el anterior si estaba abierto
            CloseIfOpen(ref _openSubmenu);

            var b1 = MakeItem("General", general);
            var b2 = MakeItem("Por Artículo", porArticulo);
            var b3 = MakeItem("Por Empleado", porEmpleado);

            var panel = BuildPanel(b1, b2, b3);

            // Abrimos el submenú anclado a la derecha del botón Uniforme
            _openSubmenu = ShowPopup(anchor, panel, new Point(anchor.Width + 5, 0), onCloseParent);

            // Cuando el submenú se cierra (por click fuera, Escape o elección),
            // cerramos el padre también (a menos que ya esté cerrado).
            _openSubmenu.Closed += (s, e) =>
            {
                var reason = e.CloseReason;

                // En todos los casos queremos cerrar el padre (para volver al estado limpio).
                // Usamos CloseCalled para evitar que el padre cancele este cierre.
                if (_openParent is { IsDisposed: false, Visible: true })
                    _openParent.Close(ToolStripDropDownCloseReason.CloseCalled);

                _openSubmenu = null;
            };

            return _openSubmenu;
        }

        // ---- Menú Planilla (sin cambios) ------------------------------------

        public static void ShowPlanillaMenu(
            Control anchor,
            Action? semana = null,
            Action? comprobante = null,
            Action? horasDiarias = null,
            Action? entradaYSalida = null,
            Action? horasSemanales = null,
            Action? controlSalidas = null,
            Action? cierreDiario = null)
        {
            CloseIfOpen(ref _openSubmenu);
            CloseIfOpen(ref _openParent);

            var b1 = MakeItem("Semana", semana);
            var b2 = MakeItem("Combrobante", comprobante);
            var b3 = MakeItem("Horas Diarias", horasDiarias);
            var b4 = MakeItem("Entradas y Salidas", entradaYSalida);
            var b5 = MakeItem("Horas Semanales", horasSemanales);
            var b6 = MakeItem("Control Salidas", controlSalidas);
            var b7 = MakeItem("Cierre Diario", cierreDiario);

            var panel = BuildPanel(b1, b2, b3, b4, b5, b6, b7);
            _openParent = ShowPopup(anchor, panel, new Point(anchor.Width - 40, 20));
        }
    }
}
