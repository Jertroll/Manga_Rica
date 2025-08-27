using System;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;

using Manga_Rica_P1.DAL;
using Manga_Rica_P1.BLL.AutentificacionService;
using Manga_Rica_P1.BLL.Session;                // ⬅️ NUEVO (IAppSession, AppSession)
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

            string connectionBd = Configuration.GetConnectionString("MangaRicaDb")
                ?? throw new InvalidOperationException("Cadena de conexión 'Manga Rica' no está configurada");

            // Infra
            var usuarioRepository = new UsuarioRepository(connectionBd);
            var autentificacionService = new AutentificacionService(usuarioRepository);

            // 🔸 Sesión compartida para toda la app
            IAppSession session = new AppSession();

            // UI
            ApplicationConfiguration.Initialize();

            // 🔸 Mostrar login modal (inyectando auth + session)
            using (var login = new LoginForm(autentificacionService, session))
            {
                var result = login.ShowDialog();

                // Si canceló o no hay usuario en sesión, salimos
                if (result != DialogResult.OK || session.CurrentUser is null)
                {
                    Application.Exit();
                    return;
                }
            }

            // 🔸 Abrir Principal con la misma sesión (mostrar "Usuario: ..." allí)
            Application.Run(new Principal(session));
        }
    }
}
