using System.Data;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.DAL
{
    public class UsuarioRepository
    {
        private readonly string _cs;

        public UsuarioRepository(string connectionString)
            => _cs = connectionString;
        public List<Usuario> GetAllUsuario()
        {
            var listaUsuario = new List<Usuario>();

            using var connectionBd = new SqlConnection(_cs);
            using var commandBd = new SqlCommand("SELECT Id, Nombre, Clave, Perfil FROM dbo.Usuarios", connectionBd);
            connectionBd.Open();

            using var readerCommand = commandBd.ExecuteReader();

            while (readerCommand.Read())
            {
                listaUsuario.Add(new Usuario
                {
                    Id = readerCommand.GetInt32(0),
                    username = readerCommand.GetString(1),
                    password = readerCommand.GetString(2),
                    perfil = readerCommand.IsDBNull(3) ? null : readerCommand.GetString(3),
                });
            }
                return listaUsuario;
            }

        public Usuario? GetByUsername(string username)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(
                "SELECT Id, Nombre, Clave, Perfil, Fecha " +
                "FROM dbo.Usuarios WHERE Nombre = @u", cn);

            cmd.Parameters.Add("@u", SqlDbType.NVarChar, 100).Value = username;

            cn.Open();
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);

            if (!rd.Read()) return null;

            return new Usuario
            {
                Id = rd.GetInt32(0),
                username = rd.GetString(1),
                password = rd.GetString(2),
                perfil = rd.IsDBNull(3) ? null : rd.GetString(3),
                fecha = rd.GetDateTime(4)
            };
        }
    }
}
