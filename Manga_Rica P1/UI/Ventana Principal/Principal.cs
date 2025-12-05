using Manga_Rica_P1.BLL;
using Manga_Rica_P1.BLL.AutentificacionService;
using Manga_Rica_P1.BLL.Pagos;
using Manga_Rica_P1.BLL.Session;
using Manga_Rica_P1.DAL;
using Manga_Rica_P1.UI.Articulos;
using Manga_Rica_P1.UI.Departamentos;
using Manga_Rica_P1.UI.Helpers;
using Manga_Rica_P1.UI.Reportes;
using Manga_Rica_P1.UI.Solicitudes;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Ventana_Principal
{
    public partial class Principal : Form
    {
        // Un controlador por menú
        private Dictionary<string, MenuDesplegable> _menus = new();
        private Label? lblUsuario;

        private readonly IAppSession _session;

        private readonly UsuariosService _usuariosService;
        private readonly DepartamentosService _departamentosService;
        private readonly SemanasService _semanasService;
        private readonly ArticulosService _articulosService;
        private readonly SolicitudesService _solicitudesService;
        private readonly EmpleadosService _empleadoService;
        private readonly HorasService _horasService;
        private readonly SodaService _sodaService;
        private readonly DeduccionesService _deduccionesService;
        private readonly CierreDiarioService _cierreService;
        private readonly ActivarPagosService _activarPagosService;
        private readonly PagosService _PagosService;
        private readonly AutentificacionService _auth;
        private readonly PuestosService _puestosService;

        public Principal(
            IAppSession session,
            AutentificacionService auth,
            UsuariosService usuariosService,
            DepartamentosService departamentosService,
            SemanasService semanasService,
            ArticulosService articulosService,
            SolicitudesService solicitudesService,
            EmpleadosService empleadosService,
            HorasService horasService,
            SodaService sodaService,
            DeduccionesService deduccionesService,
            CierreDiarioService cierreService,
            ActivarPagosService activarPagosService,
            PagosService pagosService,
            PuestosService puestosService)
        {
            InitializeComponent();
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _auth = auth ?? throw new ArgumentNullException(nameof(auth));
            _usuariosService = usuariosService ?? throw new ArgumentNullException(nameof(usuariosService));
            _departamentosService = departamentosService ?? throw new ArgumentNullException(nameof(departamentosService));
            _semanasService = semanasService ?? throw new ArgumentNullException(nameof(semanasService));
            _articulosService = articulosService ?? throw new ArgumentNullException(nameof(articulosService));
            _solicitudesService = solicitudesService ?? throw new ArgumentNullException(nameof(solicitudesService));
            _empleadoService = empleadosService ?? throw new ArgumentNullException(nameof(empleadosService));
            _horasService = horasService ?? throw new ArgumentNullException(nameof(horasService));
            _sodaService = sodaService ?? throw new ArgumentNullException(nameof(sodaService));
            _deduccionesService = deduccionesService ?? throw new ArgumentNullException(nameof(deduccionesService));
            _cierreService = cierreService ?? throw new ArgumentNullException(nameof(cierreService));
            _activarPagosService = activarPagosService ?? throw new ArgumentNullException(nameof(activarPagosService));
            _PagosService = pagosService ?? throw new ArgumentNullException(nameof(pagosService));
            _puestosService = puestosService ?? throw new ArgumentNullException(nameof(puestosService));

            // Evita recálculos de layout mientras reacomodamos todo
            this.SuspendLayout();

            // ====== 1) Crear host del sidebar (contenedor izquierdo) ======
            var sideBarHost = new Panel
            {
                Width = flowLayoutPanelSideBar.Width,
                Dock = DockStyle.Left,
                BackColor = flowLayoutPanelSideBar.BackColor,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };

            flowLayoutPanelSideBar.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelSideBar.WrapContents = false;
            flowLayoutPanelSideBar.AutoScroll = false;
            flowLayoutPanelSideBar.Dock = DockStyle.Fill;
            flowLayoutPanelSideBar.Margin = Padding.Empty;

            botonSalirContenedor.BackColor = flowLayoutPanelSideBar.BackColor;
            botonSalirContenedor.Padding = new Padding(6, 8, 6, 8);
            botonSalirContenedor.Margin = Padding.Empty;

            // Estética del botón Salir
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.Cursor = Cursors.Hand;
            btnSalir.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnSalir.Margin = new Padding(4);

            // Estética similar para Cerrar sesión
            buttonCerrarSesion.FlatStyle = FlatStyle.Flat;
            buttonCerrarSesion.FlatAppearance.BorderSize = 0;
            buttonCerrarSesion.Cursor = Cursors.Hand;
            buttonCerrarSesion.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            buttonCerrarSesion.Margin = new Padding(4);

            // ====== 3) Re-parenting: mover piezas del sidebar al host ======

            // Sacamos el panel de "Cerrar sesión" del FlowLayout
            flowLayoutPanelSideBar.Controls.Remove(panel8);

            this.Controls.Remove(flowLayoutPanelSideBar);
            this.Controls.Remove(botonSalirContenedor);

            // Orden IMPORTANTE para Dock = Bottom:
            // 1) Flow (Fill)
            // 2) Cerrar sesión (Bottom → queda encima de Salir)
            // 3) Salir (Bottom → último, pegado al borde inferior)
            sideBarHost.Controls.Add(flowLayoutPanelSideBar);  // Fill principal

            panel8.Dock = DockStyle.Bottom;
            panel8.Margin = Padding.Empty;
            sideBarHost.Controls.Add(panel8);                  // Cerrar sesión

            botonSalirContenedor.Dock = DockStyle.Bottom;
            botonSalirContenedor.Margin = Padding.Empty;
            sideBarHost.Controls.Add(botonSalirContenedor);
            sideBarHost.Controls.Add(flowLayoutPanelSideBar);  // Fill
            sideBarHost.Controls.Add(botonSalirContenedor);    // Bottom

            // ====== 4) Asegurar hermandad de contenedores (mismo padre) ======
            if (panelPrincipal.Parent != this)
                panelPrincipal.Parent = this;

            if (!this.Controls.Contains(panelPrincipal))
                this.Controls.Add(panelPrincipal);
            if (!this.Controls.Contains(sideBarHost))
                this.Controls.Add(sideBarHost);
            if (!this.Controls.Contains(panel1))
                this.Controls.Add(panel1);

            // ====== 5) Docking correcto de cada zona ======
            panel1.Dock = DockStyle.Top;           // barra superior
            sideBarHost.Dock = DockStyle.Left;     // barra lateral
            panelPrincipal.Dock = DockStyle.Fill;  // contenido

            // Mantener ancho fijo del sidebar
            sideBarHost.Width = 198;
            sideBarHost.MinimumSize = new Size(198, 0);

            // Márgenes/padding limpios en el contenedor central
            panelPrincipal.Margin = Padding.Empty;
            panelPrincipal.Padding = Padding.Empty;

            // ====== 6) Z-order ======
            this.Controls.SetChildIndex(panelPrincipal, 0);
            this.Controls.SetChildIndex(sideBarHost, 1);
            this.Controls.SetChildIndex(panel1, 2);

            // Reactivar layout
            this.ResumeLayout();

            // ====== 7) Lógica existente ======
            ColocarLabelUsuario();
            ActualizarUsuario();
            _session.UserChanged += (_, __) => ActualizarUsuario();
            this.DoubleBuffered = true; // menos flicker

            InicializarMenus();
            WireUpHeaderClicks();
            AplicarEstilosHover();
            MostrarHome();
        }

        private void InicializarMenus()
        {
            // altoMin = 46 según tu layout actual (alto del “header”)
            _menus = new Dictionary<string, MenuDesplegable>
            {
                { "conf",       new MenuDesplegable(menuConfContenedor,        altoMin: 46) },
                { "planilla",   new MenuDesplegable(menuPlanillaContenedor,    altoMin: 46) },
                { "deducc",     new MenuDesplegable(menuDeduccionesContenedor, altoMin: 46) },
                { "pagos",      new MenuDesplegable(menuPagosContenedor,       altoMin: 46) },
                { "reportes",   new MenuDesplegable(menuReportesContenedor,    altoMin: 46) },
            };
        }

        // Conecta los botones de cabecera a ToggleMenu
        private void WireUpHeaderClicks()
        {
            btnConfiguraciones.Click -= btnConfiguraciones_Click;
            btnConfiguraciones.Click += (s, e) => ToggleMenu("conf");

            btnPlanilla.Click += (s, e) => ToggleMenu("planilla");
            btnDeducciones.Click += (s, e) => ToggleMenu("deducc");
            btnPagosPrincipal.Click += (s, e) => ToggleMenu("pagos");
            btnReporte.Click += (s, e) => ToggleMenu("reportes");
        }

        private void btnConfiguraciones_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        // “Accordion”: abre uno y cierra los demás
        private void ToggleMenu(string clave)
        {
            if (!_menus.ContainsKey(clave)) return;

            foreach (var kv in _menus.Where(kv => kv.Key != clave))
                kv.Value.Colapsar();

            _menus[clave].Toggle();
        }

        private void ExpandirSolo(string clave)
        {
            foreach (var kv in _menus) kv.Value.Colapsar();
            if (_menus.TryGetValue(clave, out var m)) m.Expandir();
        }

        // Limpieza
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            foreach (var m in _menus.Values) m.Dispose();
            base.OnFormClosed(e);
        }

        private void AplicarEstilosHover()
        {
            ConfigurarHoverBoton(btnConfiguraciones, Color.FromArgb(76, 175, 80), Color.FromArgb(56, 142, 60), Color.White);
            ConfigurarHoverBoton(btnPlanilla, Color.FromArgb(76, 175, 80), Color.FromArgb(56, 142, 60), Color.White);
            ConfigurarHoverBoton(btnDeducciones, Color.FromArgb(76, 175, 80), Color.FromArgb(56, 142, 60), Color.White);
            ConfigurarHoverBoton(btnPagosPrincipal, Color.FromArgb(76, 175, 80), Color.FromArgb(56, 142, 60), Color.White);
            ConfigurarHoverBoton(btnReporte, Color.FromArgb(76, 175, 80), Color.FromArgb(56, 142, 60), Color.White);

            // VERDES (submenús)
            var hoverGreen = Color.FromArgb(139, 195, 74);
            var downGreen = Color.FromArgb(104, 159, 56);

            ConfigurarHoverBoton(btnUsuarios, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnDepartamentos, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnPuestos, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnSemanas, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnArticulos, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnEmpleado, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnCierreDiario, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnSolicitudesPlanilla, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnSoda, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnUniforme, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnReporteEmpleadoDeducciones, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnReporteGneralDeducciones, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnActivarPagos, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnPagosSubmenu, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnEmpleadosReporte, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnPlanillaReportes, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnSodaReportes, hoverGreen, downGreen);
        }

        private void ConfigurarHoverBoton(Button btn, Color hoverBack, Color downBack, Color? hoverFore = null)
        {
            btn.UseVisualStyleBackColor = false;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = hoverBack;
            btn.FlatAppearance.MouseDownBackColor = downBack;
            btn.Cursor = Cursors.Hand;

            if (hoverFore.HasValue)
            {
                var normalFore = btn.ForeColor;
                btn.MouseEnter += (s, e) => ((Button)s).ForeColor = hoverFore.Value;
                btn.MouseLeave += (s, e) => ((Button)s).ForeColor = normalFore;
            }
        }

        private void ColocarLabelUsuario()
        {
            var panelUsuario = new Panel
            {
                AutoSize = true,
                Dock = DockStyle.Right,
                Padding = new Padding(0, 5, 12, 0),
                BackColor = Color.Transparent
            };

            var picUser = new PictureBox
            {
                Size = new Size(16, 16),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Properties.Resources.usuarioNegro,
                Location = new Point(0, 9)
            };

            lblUsuario = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(picUser.Right + 5, 9),
                Text = "Usuario: —"
            };

            panelUsuario.Controls.Add(picUser);
            panelUsuario.Controls.Add(lblUsuario);
            panel1.Controls.Add(panelUsuario);
        }

        private void ActualizarUsuario()
        {
            var u = _session.CurrentUser;
            if (lblUsuario is not null)
            {
                lblUsuario.Text = u is null ? "Usuario: —"
                    : $"Usuario: {u.Username}";
            }
        }

        private void lblUsuario_Click(object sender, EventArgs e) { }
        private void Principal_Load(object sender, EventArgs e) { }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ===================== SUBMENÚ EMPLEADOS (REPORTES) =====================

        private void btnEmpleadosReporte_Click(object? sender, EventArgs e)
        {
            PopupMenus.ShowEmpleadosMenu(
                btnEmpleadosReporte,
                activos: MostrarReporteEmpleadosActivos,
                noActivos: MostrarReporteEmpleadosInactivos,
                uniformeSubmenu: (btnUniforme, closeParent) =>
                {
                    PopupMenus.ShowUniformeMenu(
                        btnUniforme,
                        general: () => { closeParent(); MostrarReporteUniformeGeneral(); },
                        porEmpleado: () => { closeParent(); MostrarReporteUniformePorEmpleado(); },
                        onCloseParent: closeParent
                    );
                },
                sodaGeneral: MostrarReporteSodaGeneral
            );
        }

        //Funciones para abrir los reportes
        private void MostrarReporteEmpleadosActivos()
        {
            try
            {
                var cs = Program.Configuration?.GetConnectionString("MangaRicaDb")
                            ?? throw new InvalidOperationException("Cadena de conexión 'MangaRicaDb' no configurada");
                var vhost = Program.Configuration?["WebView2:VirtualHost"] ?? "appassets";

                using var dlg = new MangaRica.UI.Forms.FormReporteEmpleadosActivos(cs, vhost);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el reporte: {ex.Message}",
                    "Reporte de Empleados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarReporteEmpleadosInactivos()
        {
            try
            {
                var cs = Program.Configuration?.GetConnectionString("MangaRicaDb")
                            ?? throw new InvalidOperationException("Cadena de conexión 'MangaRicaDb' no configurada");
                var vhost = Program.Configuration?["WebView2:VirtualHost"] ?? "appassets";

                using var dlg = new Manga_Rica_P1.UI.Reportes.FormReporteEmpleadosInactivos(cs, vhost);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el reporte: {ex.Message}",
                    "Reporte de Empleados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarReporteSodaGeneral()
        {
            try
            {
                var cs = Program.Configuration?.GetConnectionString("MangaRicaDb")
                         ?? throw new InvalidOperationException("Cadena de conexión 'MangaRicaDb' no configurada");

                var vhost = Program.Configuration?["WebView2:VirtualHost"] ?? "appassets";

                using var dlg = new FormReporteSodaGeneral(cs, vhost);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al abrir el reporte de soda general:\n{ex.Message}",
                    "Reporte de Soda",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void MostrarReporteUniformeGeneral()
        {
            try
            {
                var cs = Program.Configuration?.GetConnectionString("MangaRicaDb")
                         ?? throw new InvalidOperationException(
                             "Cadena de conexión 'MangaRicaDb' no configurada");

                var vhost = Program.Configuration?["WebView2:VirtualHost"] ?? "appassets";

                using var dlg = new FormReporteUniformesGeneral(cs, vhost);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al abrir el reporte de uniformes generales:\n{ex.Message}",
                    "Reporte de Uniformes",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void MostrarReporteUniformePorEmpleado()
        {
            try
            {
                var cs = Program.Configuration?.GetConnectionString("MangaRicaDb")
                         ?? throw new InvalidOperationException("Cadena de conexión 'MangaRicaDb' no configurada");
                var vhost = Program.Configuration?["WebView2:VirtualHost"] ?? "appassets";

                using var dlg = new FormReporteUniformesPorEmpleado(cs, vhost);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al abrir el reporte de uniformes por empleado:\n{ex.Message}",
                    "Reporte de Uniformes",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ===================== HOME =====================

        private void MostrarHome()
        {
            panelPrincipal.SuspendLayout();

            foreach (Control c in panelPrincipal.Controls)
                c.Dispose();
            panelPrincipal.Controls.Clear();

            var home = new HomeView
            {
                Dock = DockStyle.Fill
            };

            panelPrincipal.Controls.Add(home);
            panelPrincipal.ResumeLayout();
        }

        // ===================== SUBMENÚ PLANILLA (REPORTES) =====================

        private void btnPlanillaReportes_Click(object sender, EventArgs e)
        {
            PopupMenus.ShowPlanillaMenu(
                btnPlanillaReportes,
                semanaSubmenu: (btnSemana, closeParent) =>
                {
                    PopupMenus.ShowPlanillaSemanaMenu(
                        btnSemana,
                        general: () =>
                        {
                            closeParent();
                            MostrarPlanillaSemanaGeneral();
                        },
                        porDepartamento: () =>
                        {
                            closeParent();
                            MostrarPlanillaSemanaPorDepartamento();
                        },
                        porCedula: () =>
                        {
                            closeParent();
                            MostrarPlanillaSemanaPorEmpleado();
                        },
                        onCloseParent: closeParent
                    );
                },
                comprobante: () =>
                {
                    // Al hacer clic en "Comprobante" en el menú de Planilla,
                    // abrimos el nuevo módulo de Comprobantes de Pago.
                    MostrarReporteComprobantesPago();
                },
                horasDiariasSubmenu: (btnHoras, closeParent) =>
                {
                    PopupMenus.ShowPlanillaHorasDiariasMenu(
                        btnHoras,
                        general: () =>
                        {
                            closeParent();
                            // TODO: lógica "Horas Diarias General"
                        },
                        porDepartamento: () =>
                        {
                            closeParent();
                            // TODO: lógica "Horas Diarias por Departamento"
                        },
                        onCloseParent: closeParent
                    );
                },
                entradaYSalidaSubmenu: (btnES, closeParent) =>
                {
                    PopupMenus.ShowPlanillaEntradasSalidasMenu(
                        btnES,
                        general: () =>
                        {
                            closeParent();
                            // TODO: lógica "Entradas y Salidas General"
                        },
                        porCarnet: () =>
                        {
                            closeParent();
                            // TODO: lógica "Entradas y Salidas por Carnet"
                        },
                        quincenal: () =>
                        {
                            closeParent();
                            // TODO: lógica "Entradas y Salidas Quincenal"
                        },
                        onCloseParent: closeParent
                    );
                }
            );
        }



        // --- Stubs para los nuevos reportes de Planilla (puedes cambiarlos luego) ---

        private void MostrarPlanillaSemanaGeneral()
        {
            try
            {
                var cfg = Program.Configuration
                          ?? throw new InvalidOperationException("Configuracion no inicializada.");

                var cs = cfg.GetConnectionString("MangaRicaDb")
                         ?? throw new InvalidOperationException(
                             "Cadena de conexión 'MangaRicaDb' no configurada.");

                var vhost = cfg["WebView2:VirtualHost"] ?? "appassets";

                using (var dlg = new Manga_Rica_P1.UI.Reportes.FormReportePlanillaSemanal(cs, vhost))
                {
                    dlg.StartPosition = FormStartPosition.CenterParent;
                    dlg.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al abrir el reporte de Planilla Semanal (General):\n{ex.Message}",
                    "Planilla Semanal",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void MostrarPlanillaSemanaPorEmpleado()
        {
            try
            {
                var cfg = Program.Configuration
                          ?? throw new InvalidOperationException("Configuración no inicializada.");

                var cs = cfg.GetConnectionString("MangaRicaDb")
                         ?? throw new InvalidOperationException(
                             "Cadena de conexión 'MangaRicaDb' no configurada.");

                var vhost = cfg["WebView2:VirtualHost"] ?? "appassets";

                using (var dlg = new FormReportePlanillaSemanalPorEmpleado(cs, vhost))
                {
                    dlg.StartPosition = FormStartPosition.CenterParent;
                    dlg.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al abrir el reporte de Planilla Semanal por Empleado:\n{ex.Message}",
                    "Planilla Semanal por Empleado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void MostrarReporteComprobantesPago()
        {
            try
            {
                var cfg = Program.Configuration
                          ?? throw new InvalidOperationException("Configuración no inicializada.");

                var cs = cfg.GetConnectionString("MangaRicaDb")
                         ?? throw new InvalidOperationException(
                             "Cadena de conexión 'MangaRicaDb' no configurada.");

                var vhost = cfg["WebView2:VirtualHost"] ?? "appassets";

                using (var dlg = new FormReporteComprobantesPago(cs, vhost))
                {
                    dlg.StartPosition = FormStartPosition.CenterParent;
                    dlg.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al abrir el reporte de Comprobantes de Pago:\n{ex.Message}",
                    "Comprobantes de Pago",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private void MostrarPlanillaSemanaPorDepartamento()
        {
            try
            {
                var cfg = Program.Configuration
                          ?? throw new InvalidOperationException("Configuración no inicializada.");

                var cs = cfg.GetConnectionString("MangaRicaDb")
                         ?? throw new InvalidOperationException(
                             "Cadena de conexión 'MangaRicaDb' no configurada.");

                var vhost = cfg["WebView2:VirtualHost"] ?? "appassets";

                using (var dlg = new FormReportePlanillaSemanalPorDepartamento(cs, vhost))
                {
                    dlg.StartPosition = FormStartPosition.CenterParent;
                    dlg.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al abrir el reporte de Planilla Semanal por Departamento:\n{ex.Message}",
                    "Planilla Semanal por Departamento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private void MostrarHorasDiariasGeneral()
        {
            MessageBox.Show("Reporte de Planilla - Horas Diarias (General) - En desarrollo",
                "Planilla - Horas Diarias", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MostrarHorasDiariasPorDepartamento()
        {
            MessageBox.Show("Reporte de Planilla - Horas Diarias (Por Departamento) - En desarrollo",
                "Planilla - Horas Diarias", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MostrarEntradasSalidasGeneral()
        {
            MessageBox.Show("Reporte de Entradas y Salidas (General) - En desarrollo",
                "Entradas y Salidas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MostrarEntradasSalidasPorCarnet()
        {
            MessageBox.Show("Reporte de Entradas y Salidas (Por Carnet) - En desarrollo",
                "Entradas y Salidas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MostrarEntradasSalidasQuincenal()
        {
            MessageBox.Show("Reporte de Entradas y Salidas (Quincenal) - En desarrollo",
                "Entradas y Salidas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ===================== VISTAS DE CONFIGURACIONES =====================

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            vistaUsuario();
        }

        private void vistaUsuario()
        {
            var existente = panelPrincipal.Controls.OfType<Manga_Rica_P1.UI.User.UserView>().FirstOrDefault();
            if (existente is not null)
            {
                existente.BringToFront();
                return;
            }

            panelPrincipal.SuspendLayout();
            foreach (Control c in panelPrincipal.Controls) c.Dispose();
            panelPrincipal.Controls.Clear();

            var vista = new Manga_Rica_P1.UI.User.UserView(_usuariosService)
            {
                Dock = DockStyle.Fill
            };

            panelPrincipal.Controls.Add(vista);
            panelPrincipal.ResumeLayout();
        }

        private void btnDepartamentos_Click(object sender, EventArgs e)
        {
            vistaDepartamentos();
        }

        private void vistaDepartamentos()
        {
            var existente = panelPrincipal.Controls.OfType<DepartamentoView>().FirstOrDefault();
            if (existente is not null)
            {
                existente.BringToFront();
                return;
            }

            panelPrincipal.SuspendLayout();
            foreach (Control c in panelPrincipal.Controls) c.Dispose();
            panelPrincipal.Controls.Clear();

            var vista = new DepartamentoView(_departamentosService)
            {
                Dock = DockStyle.Fill
            };

            panelPrincipal.Controls.Add(vista);
            panelPrincipal.ResumeLayout();
        }

        private void btnPuestos_Click(object sender, EventArgs e)
        {
            var existente = panelPrincipal.Controls
                .OfType<Manga_Rica_P1.UI.Puesto.PuestoView>()
                .FirstOrDefault();

            if (existente is not null)
            {
                existente.BringToFront();
                return;
            }

            panelPrincipal.SuspendLayout();
            foreach (Control c in panelPrincipal.Controls) c.Dispose();
            panelPrincipal.Controls.Clear();

            var vista = new Manga_Rica_P1.UI.Puesto.PuestoView(_puestosService)
            {
                Dock = DockStyle.Fill
            };

            panelPrincipal.Controls.Add(vista);
            panelPrincipal.ResumeLayout();
        }

        private void btnSemanas_Click(object sender, EventArgs e)
        {
            var existente = panelPrincipal.Controls.OfType<Manga_Rica_P1.UI.Semanas.SemanaView>().FirstOrDefault();
            if (existente is not null) { existente.BringToFront(); return; }

            panelPrincipal.SuspendLayout();
            foreach (Control c in panelPrincipal.Controls) c.Dispose();
            panelPrincipal.Controls.Clear();

            var vista = new Manga_Rica_P1.UI.Semanas.SemanaView(_semanasService)
            {
                Dock = DockStyle.Fill
            };
            panelPrincipal.Controls.Add(vista);
            panelPrincipal.ResumeLayout();
        }

        private void btnArticulos_Click(object sender, EventArgs e)
        {
            var existente = panelPrincipal.Controls.OfType<ArticulosView>().FirstOrDefault();
            if (existente is not null) { existente.BringToFront(); return; }

            panelPrincipal.SuspendLayout();
            foreach (Control c in panelPrincipal.Controls) c.Dispose();
            panelPrincipal.Controls.Clear();

            var vista = new ArticulosView(_articulosService) { Dock = DockStyle.Fill };
            panelPrincipal.Controls.Add(vista);
            panelPrincipal.ResumeLayout();
        }

        private void btnSolicitudesPlanilla_Click(object sender, EventArgs e)
        {
            var existente = panelPrincipal.Controls.OfType<SolicitudView>().FirstOrDefault();
            if (existente is not null) { existente.BringToFront(); return; }

            panelPrincipal.SuspendLayout();
            foreach (Control c in panelPrincipal.Controls) c.Dispose();
            panelPrincipal.Controls.Clear();

            var vista = new SolicitudView(_solicitudesService) { Dock = DockStyle.Fill };
            panelPrincipal.Controls.Add(vista);
            panelPrincipal.ResumeLayout();
        }

        private void btnEmpleado_Click(object sender, EventArgs e)
        {
            var existente = panelPrincipal.Controls
                .OfType<Manga_Rica_P1.UI.Empleados.EmpleadosView>()
                .FirstOrDefault();
            if (existente is not null) { existente.BringToFront(); return; }

            panelPrincipal.SuspendLayout();
            foreach (Control c in panelPrincipal.Controls) c.Dispose();
            panelPrincipal.Controls.Clear();

            var vista = new Manga_Rica_P1.UI.Empleados.EmpleadosView(
                _empleadoService,
                _solicitudesService,
                _departamentosService,
                _puestosService)
            {
                Dock = DockStyle.Fill
            };

            panelPrincipal.Controls.Add(vista);
            panelPrincipal.ResumeLayout();
        }

        // ===================== MÓDULOS DE ASISTENCIA / CIERRE / SODA / UNIFORME =====================

        private void btnEntradaYSalida_Click(object sender, EventArgs e)
        {
            if (_session.CurrentUser is null)
            {
                MessageBox.Show("No hay un usuario autenticado.", "Asistencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var uid = _session.CurrentUser.Id;

            var vista = new Manga_Rica_P1.UI.Asistencia.RegistroAsistenciaView(_horasService, uid)
            {
                Dock = DockStyle.Fill
            };
            panelPrincipal.Controls.Clear();
            panelPrincipal.Controls.Add(vista);
        }

        private void btnCierreDiario_Click(object sender, EventArgs e)
        {
            using var dlg = new Manga_Rica_P1.UI.CierreDiario.CierreDiario(_cierreService);
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.ShowDialog(this);
        }

        private void btnSoda_Click(object sender, EventArgs e)
        {
            using (var dlg = new Manga_Rica_P1.UI.Soda.Soda(_sodaService, _session))
            {
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);
            }
        }

        private void btnUniforme_Click(object sender, EventArgs e)
        {
            using (var dlg = new Manga_Rica_P1.UI.Uniforme.Uniforme(_deduccionesService, _session))
            {
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);
            }
        }

        private void btnActivarPagos_Click(object sender, EventArgs e)
        {
            using (var dlg = new Manga_Rica_P1.UI.Pagos.ActivarPagos(_activarPagosService, _semanasService, _session))
            {
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);
            }
        }

        private void btnPagosSubmenu_Click(object sender, EventArgs e)
        {
            var existente = panelPrincipal.Controls
                .OfType<Manga_Rica_P1.UI.Pagos.RegistroPagos>()
                .FirstOrDefault();
            if (existente is not null)
            {
                existente.BringToFront();
                return;
            }

            var vista = new Manga_Rica_P1.UI.Pagos.RegistroPagos(
                _PagosService, _semanasService, _empleadoService, _session)
            { Dock = DockStyle.Fill };

            panelPrincipal.Controls.Clear();
            panelPrincipal.Controls.Add(vista);
        }

        // ===================== OTROS =====================

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MostrarHome();
        }

        private void buttonCerrarSesion_Click(object sender, EventArgs e)
        {
            _session.CurrentUser = null;

            this.Hide();

            using (var login = new Manga_Rica_P1.UI.Login.LoginForm(_auth, _session))
            {
                var result = login.ShowDialog(this);

                if (result == DialogResult.OK && _session.CurrentUser != null)
                {
                    ActualizarUsuario();
                    MostrarHome();
                    this.Show();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void btnSodaReportes_Click(object sender, EventArgs e)
        {
            try
            {
                var cfg = Program.Configuration
                          ?? throw new InvalidOperationException("Configuración no inicializada.");

                var cs = cfg.GetConnectionString("MangaRicaDb")
                         ?? throw new InvalidOperationException(
                             "Cadena de conexión 'MangaRicaDb' no configurada.");

                var vhost = cfg["WebView2:VirtualHost"] ?? "appassets";

                using (var dlg = new FormReporteSodaPorEmpleado(cs, vhost))
                {
                    dlg.StartPosition = FormStartPosition.CenterParent;
                    dlg.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al abrir el reporte de Soda por empleado:\n{ex.Message}",
                    "Reporte de Soda",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
