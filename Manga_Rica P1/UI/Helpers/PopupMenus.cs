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
                // Si venimos del click del botón "no-close" (submenú), suprime este cierre una única vez.
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

        // =====================================================================
        //  MENÚ EMPLEADOS (con submenú Uniforme)
        // =====================================================================

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


            var panel = BuildPanel(b1, b2, b3, b4);

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
            var b3 = MakeItem("Por Empleado", porEmpleado);

            var panel = BuildPanel(b1, b3);

            // Abrimos el submenú anclado a la derecha del botón Uniforme
            _openSubmenu = ShowPopup(anchor, panel, new Point(anchor.Width + 5, 0), onCloseParent);

            // Cuando el submenú se cierra (por click fuera, Escape o elección),
            // cerramos el padre también (a menos que ya esté cerrado).
            _openSubmenu.Closed += (s, e) =>
            {
                // En todos los casos queremos cerrar el padre (para volver al estado limpio).
                if (_openParent is { IsDisposed: false, Visible: true })
                    _openParent.Close(ToolStripDropDownCloseReason.CloseCalled);

                _openSubmenu = null;
            };

            return _openSubmenu;
        }

        // =====================================================================
        //  SUBMENÚS DE PLANILLA
        // =====================================================================

        // --- Semana -----------------------------------------------------------

        public static ToolStripDropDown ShowPlanillaSemanaMenu(
    Control anchor,
    Action? general = null,
    Action? porDepartamento = null,
    Action? porCedula = null,
    Action? porEmpleado = null,
    Action? onCloseParent = null)
        {
            CloseIfOpen(ref _openSubmenu);

            var b1 = MakeItem("General", general);
            var b2 = MakeItem("Por Departamento", porDepartamento);
            var b3 = MakeItem("Por Cedula", porCedula);

            var panel = BuildPanel(b1, b2, b3);

            _openSubmenu = ShowPopup(anchor, panel, new Point(anchor.Width + 5, 0), onCloseParent);

            _openSubmenu.Closed += (s, e) =>
            {
                if (_openParent is { IsDisposed: false, Visible: true })
                    _openParent.Close(ToolStripDropDownCloseReason.CloseCalled);

                _openSubmenu = null;
            };

            return _openSubmenu;
        }


        // --- Horas Diarias ----------------------------------------------------

        public static ToolStripDropDown ShowPlanillaHorasDiariasMenu(
            Control anchor,
            Action? general = null,
            Action? porCarnet = null,
            Action? onCloseParent = null)
        {
            CloseIfOpen(ref _openSubmenu);

            var b1 = MakeItem("General", general);
            var b2 = MakeItem("Por Carnet", porCarnet);

            var panel = BuildPanel(b1, b2);

            _openSubmenu = ShowPopup(anchor, panel, new Point(anchor.Width + 5, 0), onCloseParent);

            _openSubmenu.Closed += (s, e) =>
            {
                if (_openParent is { IsDisposed: false, Visible: true })
                    _openParent.Close(ToolStripDropDownCloseReason.CloseCalled);

                _openSubmenu = null;
            };

            return _openSubmenu;
        }


        // --- Entradas y Salidas ----------------------------------------------

        public static ToolStripDropDown ShowPlanillaEntradasSalidasMenu(
            Control anchor,
            Action? general = null,
            Action? porCarnet = null,
            Action? quincenal = null,
            Action? onCloseParent = null)
        {
            CloseIfOpen(ref _openSubmenu);

            var b1 = MakeItem("General", general);


            var panel = BuildPanel(b1);

            _openSubmenu = ShowPopup(anchor, panel, new Point(anchor.Width + 5, 0), onCloseParent);

            _openSubmenu.Closed += (s, e) =>
            {
                if (_openParent is { IsDisposed: false, Visible: true })
                    _openParent.Close(ToolStripDropDownCloseReason.CloseCalled);

                _openSubmenu = null;
            };

            return _openSubmenu;
        }

        // =====================================================================
        //  MENÚ PLANILLA (padre)
        // =====================================================================

        public static void ShowPlanillaMenu(
    Control anchor,
    Action? semana = null,
    Action<Button, Action>? semanaSubmenu = null,          // submenú Semana
    Action? comprobante = null,
    Action? horasDiarias = null,
    Action<Button, Action>? horasDiariasSubmenu = null,    // submenú Horas Diarias
    Action? entradaYSalida = null,
    Action<Button, Action>? entradaYSalidaSubmenu = null,  // submenú Entradas y Salidas
    Action? horasSemanales = null,
    Action? controlSalidas = null,                         // <- se ignoran
    Action? cierreDiario = null)                           // <- se ignoran
        {
            CloseIfOpen(ref _openSubmenu);
            CloseIfOpen(ref _openParent);

            bool subSemana = semanaSubmenu != null;
            bool subHoras = horasDiariasSubmenu != null;
            bool subEyS = entradaYSalidaSubmenu != null;

            var b1 = MakeItem(
                "Semana",
                subSemana ? null : semana,
                null,
                hasSubmenu: subSemana);

            var b2 = MakeItem("Combrobante", comprobante);

            var b3 = MakeItem(
                "Horas Diarias",
                subHoras ? null : horasDiarias,
                null,
                hasSubmenu: subHoras);

            var b4 = MakeItem(
                "Entradas y Salidas",
                subEyS ? null : entradaYSalida,
                null,
                hasSubmenu: subEyS);

            var b5 = MakeItem("Horas Semanales", horasSemanales);

            // 🔴 Eliminados del menú visual:
            // var b6 = MakeItem("Control Salidas", controlSalidas);
            // var b7 = MakeItem("Cierre Diario", cierreDiario);

            // Solo se agregan hasta Horas Semanales
            var panel = BuildPanel(b1, b2, b3, b4, b5);

            _openParent = ShowPopup(anchor, panel, new Point(anchor.Width - 40, 20));

            // Submenú de Semana
            if (semanaSubmenu != null)
            {
                b1.Click += (s, e) =>
                {
                    semanaSubmenu(
                        b1,
                        () => CloseIfOpen(ref _openParent)
                    );
                };
            }

            // Submenú de Horas Diarias
            if (horasDiariasSubmenu != null)
            {
                b3.Click += (s, e) =>
                {
                    horasDiariasSubmenu(
                        b3,
                        () => CloseIfOpen(ref _openParent)
                    );
                };
            }

            // Submenú de Entradas y Salidas
            if (entradaYSalidaSubmenu != null)
            {
                b4.Click += (s, e) =>
                {
                    entradaYSalidaSubmenu(
                        b4,
                        () => CloseIfOpen(ref _openParent)
                    );
                };
            }

            // Si el padre se cierra, cerramos cualquier submenú
            _openParent.Closed += (s, e) =>
            {
                CloseIfOpen(ref _openSubmenu);
            };
        }

    }
}
