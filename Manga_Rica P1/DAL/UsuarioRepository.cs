using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.DAL
{
    public class UsuarioRepository
    {
        private readonly string _cs;

        public UsuarioRepository(string connectionString) => _cs = connectionString;

        // =========================================================
        // LISTADO COMPLETO (compatibilidad con tu implementación)
        // =========================================================
        public List<Usuario> GetAllUsuario()
        {
            var listaUsuario = new List<Usuario>();

            using var connectionBd = new SqlConnection(_cs);
            using var commandBd = new SqlCommand(
                "SELECT Id, Nombre, Clave, Perfil, Fecha FROM dbo.Usuarios",
                connectionBd);

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
                    fecha = readerCommand.IsDBNull(4) ? DateTime.MinValue : readerCommand.GetDateTime(4)
                });
            }

            return listaUsuario;
        }

        // =========================================================
        // BÚSQUEDA POR USERNAME (compatibilidad con tu implementación)
        // =========================================================
        public Usuario? GetByUsername(string username)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(
                "SELECT Id, Nombre, Clave, Perfil, Fecha " +
                "FROM dbo.Usuarios WHERE Nombre = @u", cn);

            // Tamaño deliberado (ajústalo al de tu columna si es distinto)
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

        // =========================================================
        // NUEVAS FUNCIONES (para BLL/servicio)
        // =========================================================

        /// <summary>
        /// Página de usuarios con filtro por Nombre/Perfil.
        /// Devuelve (items, total) para paginación servidor.
        /// </summary>
        public (IEnumerable<Usuario> items, int total) GetPage(int pageIndex, int pageSize, string? filtro)
        {
            // Cálculo de desplazamiento (1-based pageIndex)
            int offset = Math.Max(0, (pageIndex - 1) * pageSize);

            var items = new List<Usuario>();
            int total = 0;

            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            // Usamos dos SELECTs: uno para la página, otro para el total.
            // Evitamos CTE para no depender de “una sola sentencia”.
            cmd.CommandText = @"
SELECT u.Id, u.Nombre, u.Clave, u.Perfil, u.Fecha
FROM dbo.Usuarios u
WHERE (@filtro IS NULL OR u.Nombre LIKE '%' + @filtro + '%' OR u.Perfil LIKE '%' + @filtro + '%')
ORDER BY u.Id
OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;

SELECT COUNT(*)
FROM dbo.Usuarios u
WHERE (@filtro IS NULL OR u.Nombre LIKE '%' + @filtro + '%' OR u.Perfil LIKE '%' + @filtro + '%');";

            cmd.Parameters.Add("@filtro", SqlDbType.NVarChar, 100).Value =
                string.IsNullOrWhiteSpace(filtro) ? DBNull.Value : filtro!.Trim();
            cmd.Parameters.Add("@offset", SqlDbType.Int).Value = offset;
            cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pageSize;

            cn.Open();
            using var rd = cmd.ExecuteReader();

            // ResultSet 1: página
            while (rd.Read())
            {
                items.Add(new Usuario
                {
                    Id = rd.GetInt32(0),
                    username = rd.GetString(1),
                    password = rd.GetString(2),
                    perfil = rd.IsDBNull(3) ? null : rd.GetString(3),
                    fecha = rd.GetDateTime(4)
                });
            }

            // ResultSet 2: total
            if (rd.NextResult() && rd.Read())
                total = rd.GetInt32(0);

            return (items, total);
        }

        /// <summary>
        /// Obtener un usuario por Id.
        /// </summary>
        public Usuario? GetById(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Clave, Perfil, Fecha FROM dbo.Usuarios WHERE Id=@id;";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

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

        /// <summary>
        /// Inserta y retorna el nuevo Id (SCOPE_IDENTITY).
        /// </summary>
        public int Insert(Usuario u)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
INSERT INTO dbo.Usuarios (Nombre, Clave, Perfil, Fecha)
VALUES (@Nombre, @Clave, @Perfil, @Fecha);
SELECT CAST(SCOPE_IDENTITY() AS int);";

            // Sugeridos: ajusta tamaños a tu esquema real si difieren.
            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = u.username;
            // Ojo: en tu BD Clave es VARCHAR(15). Mantén <= 15 desde BLL.
            cmd.Parameters.Add("@Clave", SqlDbType.NVarChar, 50).Value = u.password;
            cmd.Parameters.Add("@Perfil", SqlDbType.NVarChar, 50).Value = (object?)u.perfil ?? DBNull.Value;
            cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = u.fecha == default ? DateTime.Now : u.fecha;

            cn.Open();
            var newId = (int)cmd.ExecuteScalar();
            return newId;
        }

        /// <summary>
        /// Actualiza Nombre, Clave, Perfil del usuario.
        /// </summary>
        public void Update(Usuario u)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
UPDATE dbo.Usuarios
SET Nombre=@Nombre, Clave=@Clave, Perfil=@Perfil
WHERE Id=@Id;";

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = u.Id;
            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = u.username;
            cmd.Parameters.Add("@Clave", SqlDbType.NVarChar, 50).Value = u.password;
            cmd.Parameters.Add("@Perfil", SqlDbType.NVarChar, 50).Value = (object?)u.perfil ?? DBNull.Value;

            cn.Open();
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Elimina un usuario por Id (considera FKs antes de usar en prod).
        /// </summary>
        public void Delete(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "DELETE FROM dbo.Usuarios WHERE Id=@Id;";
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            cn.Open();
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Verifica si existe un usuario con ese Nombre. 
        /// excludingId permite ignorar el propio Id en edición.
        /// </summary>
        public bool ExistsByNombre(string nombre, int? excludingId = null)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT 1
FROM dbo.Usuarios
WHERE Nombre=@Nombre AND (@Excl IS NULL OR Id<>@Excl);";

            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = nombre;
            cmd.Parameters.Add("@Excl", SqlDbType.Int).Value =
                excludingId.HasValue ? excludingId.Value : DBNull.Value;

            cn.Open();
            var res = cmd.ExecuteScalar();
            return res is not null;
        }
    }
}
