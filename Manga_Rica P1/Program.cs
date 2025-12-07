using Manga_Rica_P1.BLL;
using Manga_Rica_P1.BLL.AutentificacionService;
using Manga_Rica_P1.BLL.Session;
using Manga_Rica_P1.DAL;
using Manga_Rica_P1.DAL.Reports;
using Manga_Rica_P1.UI.Login;
using Manga_Rica_P1.UI.Ventana_Principal;
using Microsoft.Extensions.Configuration;
using System;
using System.Windows.Forms;

namespace Manga_Rica_P1
{
    internal static class Program
    {
        public static IConfigurationRoot? Configuration { get; private set; }

        [STAThread]
        static void Main()
        {
            // Configuración
            Configuration = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .Build();

            string cs = Configuration.GetConnectionString("MangaRicaDb")
                ?? throw new InvalidOperationException("Cadena de conexión 'MangaRicaDb' no está configurada");

            string csClock = Configuration.GetConnectionString("ClockDb")
                ?? throw new InvalidOperationException("Falta 'ClockDb', no hay conexion");

            // Repositorios (DB principal)
            var usuarioRepo = new UsuarioRepository(cs);
            var departamentoRepo = new DepartamentoRepository(cs);
            var semanaRepo = new SemanaRepository(cs);
            var articulosRepo = new ArticulosRepository(cs);
            var solicitudRepo = new SolicitudRepository(cs);
            var empleadoRepo = new EmpleadoRepository(cs);
            var horaRepo = new HorasRepository(cs);
            var sodaRepo = new SodaRepository(cs);
            var sodaDetallesRepo = new SodaDetallesRepository(cs);
            var deduccionesRepo = new DeduccionesRepository(cs);
            var deduccionesDetallesRepo = new DeduccionesDetallesRepository(cs);
            var puestoRepo = new PuestoRepository(cs);

            // Repositorios (Reloj marcador)
            var clockEmployeesRepo = new Manga_Rica_P1.DAL.Clock.EmployeesClockRepository(csClock);
            var clockGlogsRepo = new Manga_Rica_P1.DAL.Clock.GLogsRepository(csClock);
            var clockCalcAttRepo = new Manga_Rica_P1.DAL.Clock.CalculatedAttendanceRepository(csClock);

            var acumuladoRepo = new AcumuladoDiarioRepository(cs, empleadoRepo, clockCalcAttRepo);
            var pagosRowRepo = new PagosRepository(cs);
            var pagosRepo = new ActivarPagosRepository(cs);

            // Repositorios de reportes
            var entradasSalidasRepo = new EntradasSalidasReportRepository(csClock); // este sí va contra ClockDb
            var horasSemanalesRepo = new HorasSemanalesReportRepository(cs);       // <-- usar MangaRicaDb


            // Servicios (BLL principales)
            var usuariosService = new UsuariosService(usuarioRepo);
            var departamentosService = new DepartamentosService(departamentoRepo);
            var semanasService = new SemanasService(semanaRepo);
            var articulosService = new ArticulosService(articulosRepo);
            var solicitudesService = new SolicitudesService(solicitudRepo);
            var empleadosService = new EmpleadosService(empleadoRepo);
            var horasService = new HorasService(horaRepo, empleadoRepo);
            var sodaService = new SodaService(sodaRepo, sodaDetallesRepo, articulosRepo, empleadoRepo);
            var deduccionesService = new DeduccionesService(deduccionesRepo, deduccionesDetallesRepo, articulosRepo, empleadoRepo);
            var cierreService = new CierreDiarioService(acumuladoRepo);
            var pagosService = new Manga_Rica_P1.BLL.Pagos.PagosService(
                acumuladoRepo,
                pagosRowRepo,
                deduccionesRepo,
                empleadoRepo,
                sodaRepo,
                semanaRepo);
            var puestosService = new PuestosService(puestoRepo);
            var activarPagosService = new ActivarPagosService(pagosRepo, empleadoRepo, semanaRepo);

            // Servicios de reportes (BLL)
            var entradasSalidasService = new ReportesEntradasSalidasService(entradasSalidasRepo);
            var horasSemanalesService = new ReportesHorasSemanalesService(horasSemanalesRepo);

            // AUTH + sesión
            var autentificacionService = new AutentificacionService(usuarioRepo);
            IAppSession session = new AppSession();

            // UI
            ApplicationConfiguration.Initialize();

            using (var login = new LoginForm(autentificacionService, session))
            {
                var result = login.ShowDialog();
                if (result != DialogResult.OK || session.CurrentUser is null)
                {
                    Application.Exit();
                    return;
                }

                // Revisar flag que nos dejó el login
                bool esUsuarioSoda = login.EsUsuarioSoda;

                if (esUsuarioSoda)
                {
                    // Ir directo al módulo de Soda
                    Application.Run(new Manga_Rica_P1.UI.Soda.Soda(sodaService, session));
                    return;
                }
            }

            // Inyecta todos los servicios al form principal
            Application.Run(new Principal(
                session,
                autentificacionService,
                usuariosService,
                departamentosService,
                semanasService,
                articulosService,
                solicitudesService,
                empleadosService,
                horasService,
                sodaService,
                deduccionesService,
                cierreService,
                activarPagosService,
                pagosService,
                puestosService,
                entradasSalidasService,
                horasSemanalesService
            ));
        }
    }
}
