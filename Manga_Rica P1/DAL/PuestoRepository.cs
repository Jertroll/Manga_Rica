using Manga_Rica_P1.Entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Manga_Rica_P1.DAL
{
    public sealed class PuestoRepository
    {
        private readonly string _cs;
        public PuestoRepository(string connectionString) => _cs = connectionString;

        // ============================
        //  Paginación + filtro
        // ============================
        public (IEnumerable<PuestoEntity> items, int total) GetPage(int pageIndex, int pageSize, string? filtro)
        {
            int offset = Math.Max(0, (pageIndex - 1) * pageSize);
            var items = new List<PuestoEntity>();
            int total = 0;

            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = @"
SELECT p.Id, p.Puesto, p.Descripcion
FROM dbo.Puesto p
WHERE (
    @f IS NULL
    OR CONVERT(nvarchar(20), p.Id)      LIKE '%' + @f + '%'
    OR p.Puesto                         LIKE '%' + @f + '%'
    OR p.Descripcion                    LIKE '%' + @f + '%'
)
ORDER BY p.Puesto ASC, p.Id ASC
OFFSET @off ROWS FETCH NEXT @ps ROWS ONLY;

SELECT COUNT(*)
FROM dbo.Puesto p
WHERE (
    @f IS NULL
    OR CONVERT(nvarchar(20), p.Id)      LIKE '%' + @f + '%'
    OR p.Puesto                         LIKE '%' + @f + '%'
    OR p.Descripcion                    LIKE '%' + @f + '%'
);";

            cmd.Parameters.Add("@f", SqlDbType.NVarChar, 100).Value =
                string.IsNullOrWhiteSpace(filtro) ? DBNull.Value : filtro!.Trim();

            cmd.Parameters.Add("@off", SqlDbType.Int).Value = offset;
            cmd.Parameters.Add("@ps", SqlDbType.Int).Value = pageSize;

            cn.Open();
            using var rd = cmd.ExecuteReader();

            // Página
            while (rd.Read())
            {
                items.Add(new PuestoEntity
                {
                    Id = rd.GetInt32(0),
                    puesto = rd.GetString(1),
                    descripcion = rd.IsDBNull(2) ? "" : rd.GetString(2)
                });
            }

            // Total
            if (rd.NextResult() && rd.Read())
                total = Convert.ToInt32(rd.GetValue(0));

            return (items, total);
        }

        // ============================
        //  GetAll (para combos o catálogos)
        // ============================
        public IEnumerable<PuestoEntity> GetAll()
        {
            var items = new List<PuestoEntity>();
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT Id, Puesto, Descripcion
FROM dbo.Puesto
ORDER BY Puesto ASC, Id ASC;";

            cn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                items.Add(new PuestoEntity
                {
                    Id = rd.GetInt32(0),
                    puesto = rd.GetString(1),
                    descripcion = rd.IsDBNull(2) ? "" : rd.GetString(2)
                });
            }
            return items;
        }

        // ============================
        //  Get por Id
        // ============================
        public PuestoEntity? GetById(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = @"
SELECT Id, Puesto, Descripcion
FROM dbo.Puesto
WHERE Id = @id;";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            cn.Open();
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null;

            return new PuestoEntity
            {
                Id = rd.GetInt32(0),
                puesto = rd.GetString(1),
                descripcion = rd.IsDBNull(2) ? "" : rd.GetString(2)
            };
        }

        // ============================
        //  Unicidad por nombre de Puesto
        // ============================
        public bool ExistsByNombre(string nombrePuesto, int? exceptId = null)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT TOP(1) 1
FROM dbo.Puesto
WHERE Puesto = @nombre
  AND (@id IS NULL OR Id <> @id);";

            cmd.Parameters.Add("@nombre", SqlDbType.NVarChar, 150).Value = nombrePuesto;
            var pId = cmd.Parameters.Add("@id", SqlDbType.Int);
            pId.Value = exceptId.HasValue ? exceptId.Value : DBNull.Value;

            cn.Open();
            var x = cmd.ExecuteScalar();
            return x != null;
        }

        // ============================
        //  Insert
        // ============================
        public int Insert(PuestoEntity p)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = @"
INSERT INTO dbo.Puesto (Puesto, Descripcion)
VALUES (@Puesto, @Descripcion);
SELECT CAST(SCOPE_IDENTITY() AS int);";

            cmd.Parameters.Add("@Puesto", SqlDbType.NVarChar, 150).Value = p.puesto;
            cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar, -1).Value =
                string.IsNullOrEmpty(p.descripcion) ? DBNull.Value : p.descripcion;

            cn.Open();
            var newId = Convert.ToInt32(cmd.ExecuteScalar());
            p.Id = newId;
            return newId;
        }

        // ============================
        //  Update
        // ============================
        public void Update(PuestoEntity p)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = @"
UPDATE dbo.Puesto
SET Puesto      = @Puesto,
    Descripcion = @Descripcion
WHERE Id = @Id;";

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = p.Id;
            cmd.Parameters.Add("@Puesto", SqlDbType.NVarChar, 150).Value = p.puesto;
            cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar, -1).Value =
                string.IsNullOrEmpty(p.descripcion) ? DBNull.Value : p.descripcion;

            cn.Open();
            cmd.ExecuteNonQuery();
        }

        // ============================
        //  Delete
        // ============================
        public void Delete(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = "DELETE FROM dbo.Puesto WHERE Id = @Id;";
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            cn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
