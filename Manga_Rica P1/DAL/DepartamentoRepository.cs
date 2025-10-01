using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.DAL
{
    /// <summary>
    /// Acceso a datos para Departamentos (SQL Server, Microsoft.Data.SqlClient).
    /// </summary>
    public class DepartamentoRepository
    {
        private readonly string _cs;
        public DepartamentoRepository(string connectionString) => _cs = connectionString;

        // ===== Paginación con filtro =====
        public (IEnumerable<Departamento> items, int total) GetPage(int pageIndex, int pageSize, string? filtro)
        {
            int offset = Math.Max(0, (pageIndex - 1) * pageSize);
            var items = new List<Departamento>();
            int total = 0;

            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT d.Id, d.Codigo, d.Departamento
FROM dbo.Departamentos d
WHERE (@filtro IS NULL OR d.Departamento LIKE '%' + @filtro + '%' OR d.Codigo LIKE '%' + @filtro + '%')
ORDER BY d.Id
OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;

SELECT COUNT(*)
FROM dbo.Departamentos d
WHERE (@filtro IS NULL OR d.Departamento LIKE '%' + @filtro + '%' OR d.Codigo LIKE '%' + @filtro + '%');";

            cmd.Parameters.Add("@filtro", SqlDbType.NVarChar, 100).Value =
                string.IsNullOrWhiteSpace(filtro) ? DBNull.Value : filtro!.Trim();
            cmd.Parameters.Add("@offset", SqlDbType.Int).Value = offset;
            cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pageSize;

            cn.Open();
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                items.Add(new Departamento
                {
                    Id = rd.GetInt32(0),
                    codigo = rd.IsDBNull(1) ? "" : rd.GetString(1),
                    nombre = rd.GetString(2)
                });
            }

            if (rd.NextResult() && rd.Read())
                total = rd.GetInt32(0);

            return (items, total);
        }

        // ===== Consultas =====
        public Departamento? GetById(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT Id, Codigo, Departamento FROM dbo.Departamentos WHERE Id=@id;";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            cn.Open();
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null;

            return new Departamento
            {
                Id = rd.GetInt32(0),
                codigo = rd.IsDBNull(1) ? "" : rd.GetString(1),
                nombre = rd.GetString(2)
            };
        }

        // ===== Comandos =====
        public int Insert(Departamento d)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
INSERT INTO dbo.Departamentos (Departamento, Codigo)
VALUES (@Nombre, @Codigo);
SELECT CAST(SCOPE_IDENTITY() AS int);";

            // Ajusta tamaños si tu esquema usa otras longitudes o VARCHAR en vez de NVARCHAR
            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = d.nombre;
            cmd.Parameters.Add("@Codigo", SqlDbType.NVarChar, 50).Value = d.codigo;

            cn.Open();
            return (int)cmd.ExecuteScalar();
        }

        public void Update(Departamento d)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
UPDATE dbo.Departamentos
SET Departamento=@Nombre, Codigo=@Codigo
WHERE Id=@Id;";

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = d.Id;
            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = d.nombre;
            cmd.Parameters.Add("@Codigo", SqlDbType.NVarChar, 50).Value = d.codigo;

            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "DELETE FROM dbo.Departamentos WHERE Id=@Id;";
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            cn.Open();
            cmd.ExecuteNonQuery();
        }

        // ===== Unicidades =====
        public bool ExistsByNombre(string nombre, int? excludingId = null)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT 1 FROM dbo.Departamentos
WHERE Departamento=@Nombre AND (@Excl IS NULL OR Id<>@Excl);";
            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = nombre;
            cmd.Parameters.Add("@Excl", SqlDbType.Int).Value =
                excludingId.HasValue ? excludingId.Value : DBNull.Value;

            cn.Open();
            var res = cmd.ExecuteScalar();
            return res is not null;
        }

        public bool ExistsByCodigo(string codigo, int? excludingId = null)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT 1 FROM dbo.Departamentos
WHERE Codigo=@Codigo AND (@Excl IS NULL OR Id<>@Excl);";
            cmd.Parameters.Add("@Codigo", SqlDbType.NVarChar, 50).Value = codigo;
            cmd.Parameters.Add("@Excl", SqlDbType.Int).Value =
                excludingId.HasValue ? excludingId.Value : DBNull.Value;

            cn.Open();
            var res = cmd.ExecuteScalar();
            return res is not null;
        }
    }
}
