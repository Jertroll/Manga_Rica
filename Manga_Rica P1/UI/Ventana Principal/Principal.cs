using Manga_Rica_P1.BLL.Session;
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
        private Label lblUsuario;
        private readonly IAppSession _session;
        public Principal(IAppSession session)
        {
            InitializeComponent();
            _session = session;

            // aquí puedes usar _session.CurrentUser
            ColocarLabelUsuario();
            ActualizarUsuario();

            // Si quieres que se actualice automáticamente si cambia la sesión
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
            // VERDES (headers)
            ConfigurarHoverBoton(btnConfiguraciones, Color.FromArgb(0, 200, 0), Color.FromArgb(0, 180, 0), Color.White);
            ConfigurarHoverBoton(btnPlanilla, Color.FromArgb(0, 200, 0), Color.FromArgb(0, 180, 0), Color.White);
            ConfigurarHoverBoton(btnDeducciones, Color.FromArgb(0, 200, 0), Color.FromArgb(0, 180, 0), Color.White);
            ConfigurarHoverBoton(btnPagosPrincipal, Color.FromArgb(0, 200, 0), Color.FromArgb(0, 180, 0), Color.White);
            ConfigurarHoverBoton(btnReporte, Color.FromArgb(0, 200, 0), Color.FromArgb(0, 180, 0), Color.White);

            // GRIS (submenús)
            var hoverGray = Color.FromArgb(240, 240, 240);
            var downGray = Color.FromArgb(225, 225, 225);

            ConfigurarHoverBoton(btnUsuarios, hoverGray, downGray);
            ConfigurarHoverBoton(btnDepartamentos, hoverGray, downGray);
            ConfigurarHoverBoton(btnSemanas, hoverGray, downGray);
            ConfigurarHoverBoton(btnArticulos, hoverGray, downGray);
            ConfigurarHoverBoton(btnEmpleado, hoverGray, downGray);
            ConfigurarHoverBoton(btnEntrada, hoverGray, downGray);
            ConfigurarHoverBoton(btnSalidas, hoverGray, downGray);
            ConfigurarHoverBoton(btnCierreDiario, hoverGray, downGray);
            ConfigurarHoverBoton(btnSolicitudesPlanilla, hoverGray, downGray);
            ConfigurarHoverBoton(btnSoda, hoverGray, downGray);
            ConfigurarHoverBoton(btnUniforme, hoverGray, downGray);
            ConfigurarHoverBoton(btnReporteEmpleadoDeducciones, hoverGray, downGray);
            ConfigurarHoverBoton(btnReporteGneralDeducciones, hoverGray, downGray);
            ConfigurarHoverBoton(btnActivarPagos, hoverGray, downGray);
            ConfigurarHoverBoton(btnPagosSubmenu, hoverGray, downGray);
            ConfigurarHoverBoton(btnEmpleadosReporte, hoverGray, downGray);
            ConfigurarHoverBoton(btnPlanillaReportes, hoverGray, downGray);
            ConfigurarHoverBoton(btnSodaReportes, hoverGray, downGray);
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


        private void ActualizarUsuario()
        {
            var u = _session.CurrentUser;
            lblUsuario.Text = u is null ? "Usuario: —"
                : $"Usuario: {u.Username}";
        }

        private void lblUsuario_Click(object sender, EventArgs e)
        {

        }
    }
}
