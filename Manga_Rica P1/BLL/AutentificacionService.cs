// BLL/AuthService.cs
using Manga_Rica_P1.DAL;
using Manga_Rica_P1.Entity;


namespace Manga_Rica_P1.BLL.AutentificacionService
{
    public class AutentificacionService
    {
        private readonly UsuarioRepository _repo;
        public AutentificacionService(UsuarioRepository repo) => _repo = repo;


        public List<Usuario> ObtenerUsuarios()
        {
            var usuarios = _repo.GetAllUsuario();

          if (usuarios is null || usuarios.Count == 0)
            {
                return new List<Usuario>();
            }
                return usuarios;

        }


        public List<string> ObtenerUsernames()
        {
            return ObtenerUsuarios()
                .Select(user => user.username)
                .Distinct()
                .OrderBy(n => n)
                .ToList();
        }

        public (bool Ok, string Message, Usuario? User) Login(string username, string password)
        {
            var user = _repo.GetByUsername(username);
            if (user is null) return (false, "Usuario no existe. Solicita un nuevo perfil al administrador", null);

            // 1) Regla de expiración
            if (user.fecha < DateTime.Now)
                return (false, "Usuario expirado. Contacte al administrador.", null);

            // 2) Verificación de contraseña
            bool ok = user.password == password;

            if (!ok) return (false, "Contraseña inválida.", null);

            return (true, "OK", user);
        }
    }
}
