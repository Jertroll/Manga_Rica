// BLL/AuthService.cs
using Manga_Rica_P1.DAL;
using Manga_Rica_P1.Entity; 
using Manga_Rica_P1.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manga_Rica_P1.BLL.AutentificacionService
{
    public class AutentificacionService
    {
        private readonly UsuarioRepository _repo;
        public AutentificacionService(UsuarioRepository repo) => _repo = repo;

        // =======================
        //        QUERIES
        // =======================
        public List<Usuario> ObtenerUsuarios()
        {
            var usuarios = _repo.GetAllUsuario();
            return (usuarios is null || usuarios.Count == 0) ? new List<Usuario>() : usuarios;
        }

        public List<string> ObtenerUsernames()
        {
            return ObtenerUsuarios()
                .Select(u => u.username)
                .Distinct()
                .OrderBy(n => n)
                .ToList();
        }

        // ==============================================
        //  LOGIN (mismo nombre) -> ahora devuelve AuthUser
        // ==============================================
        public (bool Ok, string Message, AuthUser? User) Login(string username, string password)
        {
            var usuario = _repo.GetByUsername(username);
            if (usuario is null)
                return (false, "Usuario no existe. Solicite un nuevo perfil al administrador.", null);

            // Regla de expiración (ajusta si 'fecha' significa otra cosa)
            if (usuario.fecha < DateTime.Now)
                return (false, "Usuario expirado. Contacte al administrador.", null);

            // TODO: migrar a verificación de hash (bcrypt/Argon2)
            if (usuario.password != password)
                return (false, "Contraseña inválida.", null);

            // Mapea a DTO de sesión antes de devolver
            var auth = MapToAuthUser(usuario);
            return (true, "OK", auth);
        }

        // =======================
        //       MAPEO PRIVADO
        // =======================
        private static AuthUser MapToAuthUser(Usuario u) => new AuthUser
        {
            Id = u.Id,
            Username = u.username,        
            Rol = u.perfil
        };
    }
}
