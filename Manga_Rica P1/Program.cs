using System;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;

using Manga_Rica_P1.DAL;
using Manga_Rica_P1.BLL;
using Manga_Rica_P1.BLL.AutentificacionService;
using Manga_Rica_P1.BLL.Session;
using Manga_Rica_P1.UI.Login;
using Manga_Rica_P1.UI.Ventana_Principal;

namespace Manga_Rica_P1
{
    internal static class Program
    {
        public static IConfigurationRoot? Configuration { get; private set; }

        [STAThread]
        static void Main()
        {
            // Config
            Configuration = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .Build();

            string cs = Configuration.GetConnectionString("MangaRicaDb")
                ?? throw new InvalidOperationException("Cadena de conexión 'MangaRicaDb' no está configurada");

            // Infra (Repos)
            var usuarioRepo = new UsuarioRepository(cs);
            var departamentoRepo = new DepartamentoRepository(cs);
            var semanaRepo = new SemanaRepository(cs);
            var articulosRepo = new ArticulosRepository(cs);
            var solicitudRepo = new SolicitudRepository(cs);

            // Servicios (BLL)
            var usuariosService = new UsuariosService(usuarioRepo);
            var departamentosService = new DepartamentosService(departamentoRepo);
            var semanasService = new SemanasService(semanaRepo);
            var articulosService = new ArticulosService(articulosRepo);
            var solicitudesService = new SolicitudesService(solicitudRepo);

            // Autenticación + sesión
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
            }

            // Inyecta todos los servicios al form principal (5 args)
            Application.Run(new Principal(
                session,
                usuariosService,
                departamentosService,
                semanasService,
                articulosService, 
                solicitudesService
            ));
        }
    }
}
