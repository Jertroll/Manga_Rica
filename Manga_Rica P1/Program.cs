using Microsoft.Extensions.Configuration;
using Manga_Rica_P1.DAL;
using Manga_Rica_P1.BLL.AutentificacionService;
using Manga_Rica_P1.UI.Login;
using Manga_Rica_P1.UI.Ventana_Principal;

namespace Manga_Rica_P1
{
    internal static class Program
    {
        public static IConfigurationRoot? Configuration { get; private set; } 

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Configuration = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .Build();


            string connectionBd = Configuration.GetConnectionString("MangaRicaDb")
                      ?? throw new InvalidOperationException("Cadena de conexión 'Manga Rica' no está configurada");


            var usuarioRepository = new UsuarioRepository(connectionBd); //
            var autentificacionService = new AutentificacionService(usuarioRepository);

        // 3) UI (inyectar la BLL)
        ApplicationConfiguration.Initialize();
        using var login = new LoginForm(autentificacionService);
            {
                if (login.ShowDialog() == DialogResult.OK)
                    Application.Run(new Principal());
                else
                    Application.Exit();
            }

        }
    }
}