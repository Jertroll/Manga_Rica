using Manga_Rica_P1.BLL;
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

        //Reporte
        private readonly ReporteEmpleadosService _reporteEmpleadosService;

        public Principal(IAppSession session,
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
             ReporteEmpleadosService reporteEmpleadosService)
        {
            InitializeComponent();
            _session = session ?? throw new ArgumentNullException(nameof(session));
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

            //Reportes
            _reporteEmpleadosService = reporteEmpleadosService ?? throw new ArgumentNullException(nameof(reporteEmpleadosService));



            // Evita recálculos de layout mientras reacomodamos todo
            this.SuspendLayout();

            // ====== 1) Crear host del sidebar (contenedor izquierdo) ======
            var sideBarHost = new Panel
            {
                Width = flowLayoutPanelSideBar.Width,              // el ancho que ya tenías (198 aprox.)
                Dock = DockStyle.Left,
                BackColor = flowLayoutPanelSideBar.BackColor,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };

            // ====== 2) Preparar los controles internos del sidebar ======
            flowLayoutPanelSideBar.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelSideBar.WrapContents = false;
            flowLayoutPanelSideBar.AutoScroll = false;
            flowLayoutPanelSideBar.Dock = DockStyle.Fill;
            flowLayoutPanelSideBar.Margin = Padding.Empty;

            botonSalirContenedor.Dock = DockStyle.Bottom;
            botonSalirContenedor.BackColor = flowLayoutPanelSideBar.BackColor;
            botonSalirContenedor.Padding = new Padding(6, 8, 6, 8);
            botonSalirContenedor.Margin = Padding.Empty;

            // Estética del botón Salir (como ya tenías)
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.Cursor = Cursors.Hand;
            btnSalir.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnSalir.Margin = new Padding(4);

            // ====== 3) Re-parenting: mover piezas del sidebar al host ======
            this.Controls.Remove(flowLayoutPanelSideBar);
            this.Controls.Remove(botonSalirContenedor);
            sideBarHost.Controls.Add(flowLayoutPanelSideBar);  // Fill
            sideBarHost.Controls.Add(botonSalirContenedor);    // Bottom

            // ====== 4) Asegurar hermandad de contenedores (mismo padre) ======
            // panel1  = topbar, sideBarHost = sidebar, panelPrincipal = área de contenido
            if (panelPrincipal.Parent != this)
                panelPrincipal.Parent = this;

            // Agregar al form si hiciera falta (por seguridad)
            if (!this.Controls.Contains(panelPrincipal))
                this.Controls.Add(panelPrincipal);
            if (!this.Controls.Contains(sideBarHost))
                this.Controls.Add(sideBarHost);
            if (!this.Controls.Contains(panel1))
                this.Controls.Add(panel1);

            // ====== 5) Docking correcto de cada zona ======
            panel1.Dock = DockStyle.Top;           // barra superior
            sideBarHost.Dock = DockStyle.Left;     // barra lateral
            panelPrincipal.Dock = DockStyle.Fill;  // contenido (ocupa el resto)

            // Mantener ancho fijo del sidebar (no “respira”)
            sideBarHost.Width = 198;                       // ajusta si usas otro ancho
            sideBarHost.MinimumSize = new Size(198, 0);

            // Márgenes/padding limpios en el contenedor central
            panelPrincipal.Margin = Padding.Empty;
            panelPrincipal.Padding = Padding.Empty;

            // ====== 6) Z-order: el Fill al fondo, luego Left, luego Top ======
            this.Controls.SetChildIndex(panelPrincipal, 0); // fondo (el Fill se calcula primero)
            this.Controls.SetChildIndex(sideBarHost, 1);    // izquierda por encima del Fill
            this.Controls.SetChildIndex(panel1, 2);         // top por encima de todo

            // Reactivar layout
            this.ResumeLayout();

            // ====== 7) Tu lógica existente ======
            ColocarLabelUsuario();
            ActualizarUsuario();
            _session.UserChanged += (_, __) => ActualizarUsuario();
            this.DoubleBuffered = true; // menos flicker

            InicializarMenus();
            WireUpHeaderClicks();
            AplicarEstilosHover();
        }

        private void InicializarMenus()
        {
            // altoMin = 46 según tu layout actual (alto del “header”)
            _menus = new Dictionary<string, MenuDesplegable>
            {
                // Si no pasas altoMax, lo calcula automáticamente por contenido
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
            // Ya tienes btnConfiguraciones con Click en el designer; podemos mantenerlo o reasignarlo aquí:
            btnConfiguraciones.Click -= btnConfiguraciones_Click;
            btnConfiguraciones.Click += (s, e) => ToggleMenu("conf");

            // Los demás headers no tienen handler en el designer: los asignamos aquí
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

        // Si más adelante quieres abrir/cerrar desde otros puntos:
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
            ConfigurarHoverBoton(btnConfiguraciones, Color.FromArgb(76, 175, 80), Color.FromArgb(56, 142, 60), Color.White); // #4CAF50 / #388E3C
            ConfigurarHoverBoton(btnPlanilla, Color.FromArgb(76, 175, 80), Color.FromArgb(56, 142, 60), Color.White);
            ConfigurarHoverBoton(btnDeducciones, Color.FromArgb(76, 175, 80), Color.FromArgb(56, 142, 60), Color.White);
            ConfigurarHoverBoton(btnPagosPrincipal, Color.FromArgb(76, 175, 80), Color.FromArgb(56, 142, 60), Color.White);
            ConfigurarHoverBoton(btnReporte, Color.FromArgb(76, 175, 80), Color.FromArgb(56, 142, 60), Color.White);

            // VERDES (submenús)
            var hoverGreen = Color.FromArgb(139, 195, 74);  // Verde lima claro (#8BC34A)
            var downGreen = Color.FromArgb(104, 159, 56);   // Verde oliva (#689F38)

            ConfigurarHoverBoton(btnUsuarios, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnDepartamentos, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnSemanas, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnArticulos, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnEmpleado, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnEntradaYSalida, hoverGreen, downGreen);
            ConfigurarHoverBoton(btnSalidas, hoverGreen, downGreen);
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

        private void btnEmpleadosReporte_Click(object? sender, EventArgs e)
        {
            PopupMenus.ShowEmpleadosMenu(
                btnEmpleadosReporte,
                activos: MostrarReporteEmpleadosActivos,
                noActivos: MostrarReporteEmpleadosInactivos
            );
        }

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
                MessageBox.Show("Reporte de Empleados Inactivos - Funcionalidad pendiente de implementar",
                    "Reporte de Empleados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // TODO: Implementar el reporte de empleados inactivos
                // var cs = Program.Configuration?.GetConnectionString("MangaRicaDb")
                //             ?? throw new InvalidOperationException("Cadena de conexión 'MangaRicaDb' no configurada");
                // var vhost = Program.Configuration?["WebView2:VirtualHost"] ?? "appassets";
                //
                // using var dlg = new MangaRica.UI.Forms.FormReporteEmpleadosInactivos(cs, vhost);
                // dlg.StartPosition = FormStartPosition.CenterParent;
                // dlg.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el reporte: {ex.Message}",
                    "Reporte de Empleados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPlanillaReportes_Click(object sender, EventArgs e)
        {
            PopupMenus.ShowPlanillaMenu(btnPlanillaReportes);
        }

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

        // ▼ NUEVO: vista de Departamentos con inyección del servicio y evitando duplicados
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

        private void btnSemanas_Click(object sender, EventArgs e)
        {
            var existente = panelPrincipal.Controls.OfType<Manga_Rica_P1.UI.Semanas.SemanaView>().FirstOrDefault();
            if (existente is not null) { existente.BringToFront(); return; }

            panelPrincipal.SuspendLayout();
            foreach (Control c in panelPrincipal.Controls) c.Dispose();
            panelPrincipal.Controls.Clear();

            var vista = new Manga_Rica_P1.UI.Semanas.SemanaView(_semanasService) { Dock = DockStyle.Fill };
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

            // ⬅️ CORREGIDO: pasar el servicio requerido por el ctor
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

            var vista = new SolicitudView(_solicitudesService) { Dock = DockStyle.Fill }; // ✅ pasar servicio
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

            var vista = new Manga_Rica_P1.UI.Empleados.EmpleadosView(_empleadoService, _solicitudesService, _departamentosService)
            {
                Dock = DockStyle.Fill
            };
            panelPrincipal.Controls.Add(vista);
            panelPrincipal.ResumeLayout();
        }

        private void btnEntradaYSalida_Click(object sender, EventArgs e)
        {
            if (_session.CurrentUser is null)
            {
                MessageBox.Show("No hay un usuario autenticado.", "Asistencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var uid = _session.CurrentUser.Id; // si no es int: var uid = Convert.ToInt32(_session.CurrentUser.Id);

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
            var vista = new Manga_Rica_P1.UI.Pagos.RegistroPagos
            {
                Dock = DockStyle.Fill
            };
            panelPrincipal.Controls.Clear();
            panelPrincipal.Controls.Add(vista);
        }
    }
}
