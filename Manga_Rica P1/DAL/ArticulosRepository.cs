using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.DAL
{
    /// <summary>
    /// Acceso a datos para Articulos (SQL Server).
    /// Tabla dbo.Articulos: Id, Descripcion, Precio, Existencia, Categoria.
    /// </summary>
    public sealed class ArticulosRepository
    {
        private readonly string _cs;
        public ArticulosRepository(string connectionString) => _cs = connectionString;

        // =========================================================
        //  Paginación + filtro (para usar desde la UI con GetPage)
        // =========================================================
        public (IEnumerable<Articulos> items, int total) GetPage(int pageIndex, int pageSize, string? filtro)
        {
            int offset = Math.Max(0, (pageIndex - 1) * pageSize);
            var items = new List<Articulos>();
            int total = 0;

            int? fInt = null;
            decimal? fDec = null;

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var f = filtro.Trim();

                if (int.TryParse(f, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n))
                    fInt = n;

                if (decimal.TryParse(f, NumberStyles.Float, CultureInfo.InvariantCulture, out var d))
                    fDec = d;
            }

            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = @"
SELECT a.Id, a.Descripcion, a.Precio, a.Existencia, a.Categoria
FROM dbo.Articulos a
WHERE (
    @f IS NULL
    OR CONVERT(nvarchar(20), a.Id)           LIKE '%' + @f + '%'
    OR a.Descripcion                         LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(50), a.Precio)       LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(20), a.Existencia)   LIKE '%' + @f + '%'
    OR a.Categoria                           LIKE '%' + @f + '%'
    OR (@fInt IS NOT NULL AND (a.Id = @fInt OR a.Existencia = @fInt))
    OR (@fDec IS NOT NULL AND a.Precio = @fDec)
)
ORDER BY a.Id DESC
OFFSET @off ROWS FETCH NEXT @ps ROWS ONLY;

SELECT COUNT(*)
FROM dbo.Articulos a
WHERE (
    @f IS NULL
    OR CONVERT(nvarchar(20), a.Id)           LIKE '%' + @f + '%'
    OR a.Descripcion                         LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(50), a.Precio)       LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(20), a.Existencia)   LIKE '%' + @f + '%'
    OR a.Categoria                           LIKE '%' + @f + '%'
    OR (@fInt IS NOT NULL AND (a.Id = @fInt OR a.Existencia = @fInt))
    OR (@fDec IS NOT NULL AND a.Precio = @fDec)
);";

            cmd.Parameters.Add("@f", SqlDbType.NVarChar, 100).Value =
                string.IsNullOrWhiteSpace(filtro) ? DBNull.Value : filtro!.Trim();

            var pInt = cmd.Parameters.Add("@fInt", SqlDbType.Int);
            pInt.Value = fInt.HasValue ? fInt.Value : DBNull.Value;

            var pDec = cmd.Parameters.Add("@fDec", SqlDbType.Decimal);
            pDec.Precision = 18; pDec.Scale = 2;
            pDec.Value = fDec.HasValue ? fDec.Value : DBNull.Value;

            cmd.Parameters.Add("@off", SqlDbType.Int).Value = offset;
            cmd.Parameters.Add("@ps", SqlDbType.Int).Value = pageSize;

            cn.Open();
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                // Precio puede ser decimal/money/float en BD; convertimos de forma segura a float
                var precioObj = rd.GetValue(2);
                float precio = Convert.ToSingle(precioObj, CultureInfo.InvariantCulture);

                items.Add(new Articulos
                {
                    Id = rd.GetInt32(0),
                    descripcion = rd.GetString(1),
                    precio = precio,
                    existencia = rd.GetInt32(3),
                    categoria = rd.GetString(4)
                });
            }

            if (rd.NextResult() && rd.Read())
                total = rd.GetInt32(0);

            return (items, total);
        }

        // =========================================================
        //  Get por Id
        // =========================================================
        public Articulos? GetById(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = "SELECT Id, Descripcion, Precio, Existencia, Categoria FROM dbo.Articulos WHERE Id=@Id;";
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            cn.Open();
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null;

            var precioObj = rd.GetValue(2);
            float precio = Convert.ToSingle(precioObj, CultureInfo.InvariantCulture);

            return new Articulos
            {
                Id = rd.GetInt32(0),
                descripcion = rd.GetString(1),
                precio = precio,
                existencia = rd.GetInt32(3),
                categoria = rd.GetString(4)
            };
        }

        // =========================================================
        //  Insert
        // =========================================================
        public int Insert(Articulos a)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = @"
INSERT INTO dbo.Articulos (Descripcion, Precio, Existencia, Categoria)
VALUES (@Descripcion, @Precio, @Existencia, @Categoria);
SELECT CAST(SCOPE_IDENTITY() AS int);";

            cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 200).Value = a.descripcion;
            var pPrecio = cmd.Parameters.Add("@Precio", SqlDbType.Decimal); pPrecio.Precision = 18; pPrecio.Scale = 2;
            pPrecio.Value = Math.Round((decimal)a.precio, 2);
            cmd.Parameters.Add("@Existencia", SqlDbType.Int).Value = a.existencia;
            cmd.Parameters.Add("@Categoria", SqlDbType.NVarChar, 50).Value = a.categoria;

            cn.Open();
            return (int)cmd.ExecuteScalar();
        }

        // =========================================================
        //  Update
        // =========================================================
        public void Update(Articulos a)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = @"
UPDATE dbo.Articulos
SET Descripcion = @Descripcion,
    Precio      = @Precio,
    Existencia  = @Existencia,
    Categoria   = @Categoria
WHERE Id=@Id;";

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = a.Id;
            cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 200).Value = a.descripcion;
            var pPrecio = cmd.Parameters.Add("@Precio", SqlDbType.Decimal); pPrecio.Precision = 18; pPrecio.Scale = 2;
            pPrecio.Value = Math.Round((decimal)a.precio, 2);
            cmd.Parameters.Add("@Existencia", SqlDbType.Int).Value = a.existencia;
            cmd.Parameters.Add("@Categoria", SqlDbType.NVarChar, 50).Value = a.categoria;

            cn.Open();
            cmd.ExecuteNonQuery();
        }

        // =========================================================
        //  Delete
        // =========================================================
        public void Delete(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = "DELETE FROM dbo.Articulos WHERE Id=@Id;";
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            cn.Open();
            cmd.ExecuteNonQuery();
        }

        // (Opcional) Verificar duplicados por descripción
        public bool ExistsByDescripcion(string descripcion, int? excludeId = null)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = @"
SELECT 1
FROM dbo.Articulos
WHERE UPPER(Descripcion) = UPPER(@d)
  AND (@ex IS NULL OR Id <> @ex);";

            cmd.Parameters.Add("@d", SqlDbType.NVarChar, 200).Value = descripcion;
            var pEx = cmd.Parameters.Add("@ex", SqlDbType.Int);
            pEx.Value = excludeId.HasValue ? excludeId.Value : DBNull.Value;

            cn.Open();
            var o = cmd.ExecuteScalar();
            return o != null;
        }
    }
}
