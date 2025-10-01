using Manga_Rica_P1.BLL;
using Manga_Rica_P1.BLL.Session;
using Manga_Rica_P1.UI.Articulos;
using Manga_Rica_P1.UI.Helpers;
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
        // Cambia la declaración de lblUsuario para permitir nulos y suprime la advertencia donde se usa
        private Label? lblUsuario;
        private readonly IAppSession _session;
        private readonly UsuariosService _usuariosService;   // ⬅️ NUEVO
        public Principal(IAppSession session, UsuariosService usuariosService)
        {
            InitializeComponent();
            _session = session;
            _usuariosService = usuariosService;

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
            this.Controls.SetChildIndex(sideBarHost, 1);  // izquierda por encima del Fill
            this.Controls.SetChildIndex(panel1, 2);  // top por encima de todo

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

            // VERDES (submenús, combinan con el fondo de tu screenshot)
            var hoverGreen = Color.FromArgb(139, 195, 74);  // Verde lima claro (#8BC34A)
            var downGreen = Color.FromArgb(104, 159, 56);  // Verde oliva (#689F38)

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
            // Fondo personalizado
            btn.UseVisualStyleBackColor = false;

            // Necesario para que funcionen los colores de hover integrados
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

            // Colores nativos de hover/click
            btn.FlatAppearance.MouseOverBackColor = hoverBack;
            btn.FlatAppearance.MouseDownBackColor = downBack;

            // Cursor de mano
            btn.Cursor = Cursors.Hand;

            // Si quieres cambiar también el color del texto en hover
            if (hoverFore.HasValue)
            {
                var normalFore = btn.ForeColor;
                btn.MouseEnter += (s, e) => ((Button)s).ForeColor = hoverFore.Value;
                btn.MouseLeave += (s, e) => ((Button)s).ForeColor = normalFore;
            }
        }

        private void ColocarLabelUsuario()
        {
            // Panel contenedor para juntar icono + label
            var panelUsuario = new Panel
            {
                AutoSize = true,
                Dock = DockStyle.Right,
                Padding = new Padding(0, 5, 12, 0),
                BackColor = Color.Transparent
            };

            // Imagen (puedes controlar el tamaño aquí mismo)
            var picUser = new PictureBox
            {
                Size = new Size(16, 16),                      // 👈 tamaño de la imagen reducido
                SizeMode = PictureBoxSizeMode.StretchImage,  // ajusta la imagen al tamaño
                Image = Properties.Resources.usuarioNegro,      // 👈 ícono agregado a Resources
                Location = new Point(0, 9)                   // leve ajuste vertical
            };

            // Texto
            lblUsuario = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(picUser.Right + 5, 9),  // 👈 lo coloca a la derecha del icono
                Text = "Usuario: —"
            };

            // Agregar controles al panel
            panelUsuario.Controls.Add(picUser);
            panelUsuario.Controls.Add(lblUsuario);

            // Agregar el panel al topbar
            panel1.Controls.Add(panelUsuario);
        }


        // ...resto del código...

        private void ActualizarUsuario()
        {
            var u = _session.CurrentUser;
            if (lblUsuario is not null)
            {
                lblUsuario.Text = u is null ? "Usuario: —"
                    : $"Usuario: {u.Username}";
            }
        }

        private void lblUsuario_Click(object sender, EventArgs e)
        {

        }

        private void Principal_Load(object sender, EventArgs e)
        {

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnEmpleadosReporte_Click(object sender, EventArgs e)
        {
            PopupMenus.ShowEmpleadosMenu(
                btnEmpleado
   // listado: () => ShowView<UI.Vistas.EmpleadosView>(),
   //  nuevo: () => ShowView<UI.Vistas.EmpleadoNuevoView>(),
   //  importar: () => ShowView<UI.Vistas.ImportarEmpleadosView>()
   );
        }

        private void btnPlanillaReportes_Click(object sender, EventArgs e)
        {
            PopupMenus.ShowPlanillaMenu(
                btnPlanillaReportes
                );
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            vistaUsuario();
        }


        private void vistaUsuario()
        {
            // Si ya está cargada, solo la traemos al frente
            var existente = panelPrincipal.Controls.OfType<Manga_Rica_P1.UI.User.UserView>().FirstOrDefault();
            if (existente is not null)
            {
                existente.BringToFront();
                return;
            }

            panelPrincipal.SuspendLayout();

            // Limpia y desecha lo que haya antes (evita fugas)
            foreach (Control c in panelPrincipal.Controls) c.Dispose();
            panelPrincipal.Controls.Clear();

            // ⚠️ ahora sí: inyecta el servicio que tienes en el form
            var vista = new Manga_Rica_P1.UI.User.UserView(_usuariosService)
            {
                Dock = DockStyle.Fill
            };

            panelPrincipal.Controls.Add(vista);
            panelPrincipal.ResumeLayout();
        }


        private void btnDepartamentos_Click(object sender, EventArgs e)
        {
            var vista = new Manga_Rica_P1.UI.Departamentos.DepartamentoView();
            vista.Dock = DockStyle.Fill;

            panelPrincipal.Controls.Clear();

            panelPrincipal.Controls.Add(vista);
        }

        private void btnSemanas_Click(object sender, EventArgs e)
        {
            var vista = new Manga_Rica_P1.UI.Semanas.SemanaView();
            vista.Dock = DockStyle.Fill;
            panelPrincipal.Controls.Clear();
            panelPrincipal.Controls.Add(vista);
        }

        private void btnArticulos_Click(object sender, EventArgs e)
        {
            var vista = new Manga_Rica_P1.UI.Articulos.ArticulosView();
            vista.Dock = DockStyle.Fill;
            panelPrincipal.Controls.Clear();
            panelPrincipal.Controls.Add(vista);
        }

        private void btnSolicitudesPlanilla_Click(object sender, EventArgs e)
        {
            var vista = new Manga_Rica_P1.UI.Solicitudes.SolicitudView();
            vista.Dock = DockStyle.Fill;
            panelPrincipal.Controls.Clear();
            panelPrincipal.Controls.Add(vista);
        }

        private void btnEmpleado_Click(object sender, EventArgs e)
        {
            var vista = new Manga_Rica_P1.UI.Empleados.EmpleadosView();
            vista.Dock = DockStyle.Fill;
            panelPrincipal.Controls.Clear();
            panelPrincipal.Controls.Add(vista);
        }

        private void btnEntradaYSalida_Click(object sender, EventArgs e)
        {
            var vista = new Manga_Rica_P1.UI.Asistencia.RegistroAsistenciaView();
            vista.Dock = DockStyle.Fill;
            panelPrincipal.Controls.Clear();
            panelPrincipal.Controls.Add(vista);
        }

        private void btnCierreDiario_Click(object sender, EventArgs e)
        {
            using (var dlg = new Manga_Rica_P1.UI.CierreDiario.CierreDiario())
            {
                // para centrar respecto a la ventana principal:
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);  // <- modal con dueño
            }
        }

        private void btnSoda_Click(object sender, EventArgs e)
        {
            using (var dlg = new Manga_Rica_P1.UI.Soda.Soda())
            {
                // para centrar respecto a la ventana principal:
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);  // <- modal con dueño
            }

        }

        private void btnUniforme_Click(object sender, EventArgs e)
        {
            using (var dlg = new Manga_Rica_P1.UI.Uniforme.Uniforme())
            {
                // para centrar respecto a la ventana principal:
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);  // <- modal con dueño
            }
        }

        private void btnActivarPagos_Click(object sender, EventArgs e)
        {

            using (var dlg = new Manga_Rica_P1.UI.Pagos.ActivarPagos())
            {
                // para centrar respecto a la ventana principal:
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);  // <- modal con dueño
            }
        }

        private void btnPagosSubmenu_Click(object sender, EventArgs e)
        {
            var vista = new Manga_Rica_P1.UI.Pagos.RegistroPagos();
            vista.Dock = DockStyle.Fill;
            panelPrincipal.Controls.Clear();
            panelPrincipal.Controls.Add(vista);
        }
    }
}
