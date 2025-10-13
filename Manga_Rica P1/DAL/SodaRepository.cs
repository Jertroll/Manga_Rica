using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.DAL
{
    /// <summary>
    /// Repositorio para el manejo de datos de Soda y sus detalles.
    /// </summary>
    public sealed class SodaRepository
    {
        private readonly string _cs;
        public SodaRepository(string connectionString) => _cs = connectionString;

        // =========================================================
        //  Paginación con filtro para Soda
        // =========================================================
        public (IEnumerable<Soda> items, int total) GetPage(int pageIndex, int pageSize, string? filtro)
        {
            int offset = Math.Max(0, (pageIndex - 1) * pageSize);
            var items = new List<Soda>();
            int total = 0;

            int? fInt = null;
            float? fFloat = null;
            DateTime? fDate = null;

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var f = filtro.Trim();
                if (int.TryParse(f, out var n)) fInt = n;
                if (float.TryParse(f, NumberStyles.Any, CultureInfo.InvariantCulture, out var fl)) fFloat = fl;
                if (DateTime.TryParseExact(f, new[] { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" },
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) || DateTime.TryParse(f, out dt))
                    fDate = dt.Date;
            }

            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = @"
SELECT s.Id, s.Id_Empleado, s.Total, s.Id_Usuario, s.Anulada, s.Fecha
FROM dbo.Soda s
WHERE (
    @f IS NULL
    OR CONVERT(nvarchar(20), s.Id) LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(20), s.Id_Empleado) LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(20), s.Total) LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(20), s.Id_Usuario) LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(10), s.Fecha, 23) LIKE '%' + @f + '%'
    OR (@fInt IS NOT NULL AND (s.Id = @fInt OR s.Id_Empleado = @fInt OR s.Id_Usuario = @fInt OR s.Anulada = @fInt))
    OR (@fFloat IS NOT NULL AND s.Total = @fFloat)
    OR (@fDate IS NOT NULL AND CAST(s.Fecha AS date) = @fDate)
)
ORDER BY s.Id DESC
OFFSET @off ROWS FETCH NEXT @ps ROWS ONLY;

SELECT COUNT(*)
FROM dbo.Soda s
WHERE (
    @f IS NULL
    OR CONVERT(nvarchar(20), s.Id) LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(20), s.Id_Empleado) LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(20), s.Total) LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(20), s.Id_Usuario) LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(10), s.Fecha, 23) LIKE '%' + @f + '%'
    OR (@fInt IS NOT NULL AND (s.Id = @fInt OR s.Id_Empleado = @fInt OR s.Id_Usuario = @fInt OR s.Anulada = @fInt))
    OR (@fFloat IS NOT NULL AND s.Total = @fFloat)
    OR (@fDate IS NOT NULL AND CAST(s.Fecha AS date) = @fDate)
);";

            cmd.Parameters.Add("@f", SqlDbType.NVarChar, 120).Value =
                string.IsNullOrWhiteSpace(filtro) ? DBNull.Value : filtro!.Trim();

            var pInt = cmd.Parameters.Add("@fInt", SqlDbType.Int);
            pInt.Value = fInt.HasValue ? fInt.Value : DBNull.Value;

            var pFloat = cmd.Parameters.Add("@fFloat", SqlDbType.Float);
            pFloat.Value = fFloat.HasValue ? fFloat.Value : DBNull.Value;

            var pDate = cmd.Parameters.Add("@fDate", SqlDbType.Date);
            pDate.Value = fDate.HasValue ? fDate.Value : DBNull.Value;

            cmd.Parameters.Add("@off", SqlDbType.Int).Value = offset;
            cmd.Parameters.Add("@ps", SqlDbType.Int).Value = pageSize;

            cn.Open();
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                items.Add(MapSoda(rd));
            }

            if (rd.NextResult() && rd.Read())
                total = Convert.ToInt32(rd.GetValue(0));

            return (items, total);
        }

        // =========================================================
        //  CRUD Soda
        // =========================================================
        public Soda? GetById(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT Id, Id_Empleado, Total, Id_Usuario, Anulada, Fecha
FROM dbo.Soda WHERE Id=@id;";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            cn.Open();
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null;

            return MapSoda(rd);
        }

        public int Insert(Soda s)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
INSERT INTO dbo.Soda (Id_Empleado, Total, Id_Usuario, Anulada, Fecha)
VALUES (@IdEmp, @Total, @IdUsu, @Anulada, @Fecha);
SELECT CAST(SCOPE_IDENTITY() AS int);";

            cmd.Parameters.Add("@IdEmp", SqlDbType.Int).Value = s.Id_Empleado;
            cmd.Parameters.Add("@Total", SqlDbType.Float).Value = s.Total;
            cmd.Parameters.Add("@IdUsu", SqlDbType.Int).Value = s.Id_Usuario;
            cmd.Parameters.Add("@Anulada", SqlDbType.Int).Value = s.Anulada;
            cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = s.Fecha;

            cn.Open();
            var newIdObj = cmd.ExecuteScalar();
            return Convert.ToInt32(newIdObj);
        }

        public void Update(Soda s)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
UPDATE dbo.Soda SET
    Id_Empleado=@IdEmp, Total=@Total, Id_Usuario=@IdUsu, 
    Anulada=@Anulada, Fecha=@Fecha
WHERE Id=@Id;";

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = s.Id;
            cmd.Parameters.Add("@IdEmp", SqlDbType.Int).Value = s.Id_Empleado;
            cmd.Parameters.Add("@Total", SqlDbType.Float).Value = s.Total;
            cmd.Parameters.Add("@IdUsu", SqlDbType.Int).Value = s.Id_Usuario;
            cmd.Parameters.Add("@Anulada", SqlDbType.Int).Value = s.Anulada;
            cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = s.Fecha;

            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "DELETE FROM dbo.Soda WHERE Id=@Id;";
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        // =========================================================
        //  Soda_Detalles
        // =========================================================
        public List<Soda_Detalles> GetDetallesBySodaId(int idSoda)
        {
            var detalles = new List<Soda_Detalles>();
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT Id, Id_Soda, Codigo_Articulo, Cantidad, Precio, Total
FROM dbo.Soda_Detalles WHERE Id_Soda=@idSoda ORDER BY Id;";
            cmd.Parameters.Add("@idSoda", SqlDbType.Int).Value = idSoda;

            cn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                detalles.Add(MapSodaDetalle(rd));
            }
            return detalles;
        }

        public void InsertDetalle(Soda_Detalles detalle)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
INSERT INTO dbo.Soda_Detalles (Id_Soda, Codigo_Articulo, Cantidad, Precio, Total)
VALUES (@IdSoda, @CodArt, @Cant, @Precio, @Total);";

            cmd.Parameters.Add("@IdSoda", SqlDbType.Int).Value = detalle.Id_Soda;
            cmd.Parameters.Add("@CodArt", SqlDbType.Int).Value = detalle.Codigo_Articulo;
            cmd.Parameters.Add("@Cant", SqlDbType.Int).Value = detalle.Cantidad;
            cmd.Parameters.Add("@Precio", SqlDbType.Float).Value = detalle.Precio;
            cmd.Parameters.Add("@Total", SqlDbType.Float).Value = detalle.Total;

            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteDetallesBySodaId(int idSoda)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "DELETE FROM dbo.Soda_Detalles WHERE Id_Soda=@idSoda;";
            cmd.Parameters.Add("@idSoda", SqlDbType.Int).Value = idSoda;
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        // =========================================================
        //  Mappers
        // =========================================================
        private static Soda MapSoda(SqlDataReader rd)
        {
            return new Soda
            {
                Id = rd.GetInt32(0),
                Id_Empleado = rd.GetInt32(1),
                Total = Convert.ToSingle(rd.GetValue(2)),
                Id_Usuario = rd.GetInt32(3),
                Anulada = rd.GetInt32(4),
                Fecha = rd.GetDateTime(5)
            };
        }

        private static Soda_Detalles MapSodaDetalle(SqlDataReader rd)
        {
            return new Soda_Detalles
            {
                Id = rd.GetInt32(0),
                Id_Soda = rd.GetInt32(1),
                Codigo_Articulo = rd.GetInt32(2),
                Cantidad = rd.GetInt32(3),
                Precio = Convert.ToSingle(rd.GetValue(4)),
                Total = Convert.ToSingle(rd.GetValue(5))
            };
        }
    }
}