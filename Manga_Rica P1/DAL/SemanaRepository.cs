using Manga_Rica_P1.Entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Manga_Rica_P1.DAL
{
    /// <summary>
    /// Acceso a datos para Semanas (SQL Server).
    /// Entity: Semana (Id INT, Semana INT, Fecha_Inicio DATETIME/DATE, Fecha_Final DATETIME/DATE)
    /// </summary>
    public sealed class SemanaRepository
    {
        private readonly string _cs;
        public SemanaRepository(string connectionString) => _cs = connectionString;

        // ============================
        //  Paginación + filtro
        // ============================
        public (IEnumerable<Semana> items, int total) GetPage(int pageIndex, int pageSize, string? filtro)
        {
            int offset = Math.Max(0, (pageIndex - 1) * pageSize);
            var items = new List<Semana>();
            int total = 0;

            // Parse “inteligente” del filtro
            int? fInt = null;
            DateTime? fDate = null;

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var f = filtro.Trim();

                if (int.TryParse(f, out var n)) fInt = n;

                if (DateTime.TryParseExact(f, new[] { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" },
                                           CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt)
                    || DateTime.TryParse(f, out dt))
                {
                    fDate = dt.Date;
                }
            }

            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = @"
SELECT s.Id, s.Semana, s.Fecha_Inicio, s.Fecha_Final
FROM dbo.Semanas s
WHERE (
    @f IS NULL
    OR CONVERT(nvarchar(20), s.Id)                 LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(20), s.Semana)             LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(10), s.Fecha_Inicio, 23)   LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(10), s.Fecha_Final, 23)    LIKE '%' + @f + '%'
    OR (@fInt  IS NOT NULL AND (s.Id = @fInt OR s.Semana = @fInt))
    OR (@fDate IS NOT NULL AND (CAST(s.Fecha_Inicio AS date) = @fDate OR CAST(s.Fecha_Final AS date) = @fDate))
)
ORDER BY s.Fecha_Inicio DESC, s.Id DESC
OFFSET @off ROWS FETCH NEXT @ps ROWS ONLY;

SELECT COUNT(*)
FROM dbo.Semanas s
WHERE (
    @f IS NULL
    OR CONVERT(nvarchar(20), s.Id)                 LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(20), s.Semana)             LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(10), s.Fecha_Inicio, 23)   LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(10), s.Fecha_Final, 23)    LIKE '%' + @f + '%'
    OR (@fInt  IS NOT NULL AND (s.Id = @fInt OR s.Semana = @fInt))
    OR (@fDate IS NOT NULL AND (CAST(s.Fecha_Inicio AS date) = @fDate OR CAST(s.Fecha_Final AS date) = @fDate))
);";

            cmd.Parameters.Add("@f", SqlDbType.NVarChar, 100).Value =
                string.IsNullOrWhiteSpace(filtro) ? DBNull.Value : filtro!.Trim();

            var pInt = cmd.Parameters.Add("@fInt", SqlDbType.Int);
            pInt.Value = fInt.HasValue ? fInt.Value : DBNull.Value;

            var pDate = cmd.Parameters.Add("@fDate", SqlDbType.Date);
            pDate.Value = fDate.HasValue ? fDate.Value : DBNull.Value;

            cmd.Parameters.Add("@off", SqlDbType.Int).Value = offset;
            cmd.Parameters.Add("@ps", SqlDbType.Int).Value = pageSize;

            cn.Open();
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                items.Add(new Semana
                {
                    Id = rd.GetInt32(0),
                    semana = rd.GetInt32(1),
                    fecha_Inicio = rd.GetDateTime(2),
                    fecha_Final = rd.GetDateTime(3)
                });
            }

            if (rd.NextResult() && rd.Read())
                total = Convert.ToInt32(rd.GetValue(0));

            return (items, total);
        }

        // ============================
        //  Nueva implementacion: GetAll (para combos o catálogos)
        // ============================
        public IEnumerable<Semana> GetAll()
        {
            var items = new List<Semana>();
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT Id, Semana, Fecha_Inicio, Fecha_Final FROM dbo.Semanas ORDER BY Id DESC;";
            cn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                items.Add(new Semana
                {
                    Id = rd.GetInt32(0),
                    semana = rd.GetInt32(1),
                    fecha_Inicio = rd.GetDateTime(2),
                    fecha_Final = rd.GetDateTime(3)
                });
            }
            return items;
        }

        // ============================
        //  Get por Id
        // ============================
        public Semana? GetById(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = "SELECT Id, Semana, Fecha_Inicio, Fecha_Final FROM dbo.Semanas WHERE Id=@id;";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            cn.Open();
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null;

            return new Semana
            {
                Id = rd.GetInt32(0),
                semana = rd.GetInt32(1),
                fecha_Inicio = rd.GetDateTime(2),
                fecha_Final = rd.GetDateTime(3)
            };
        }

        // ============================
        //  Nueva implementacion: Unicidad por número de semana
        // ============================
        public bool ExistsByNumeroSemana(int numeroSemana, int? exceptId = null)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT TOP(1) 1
FROM dbo.Semanas
WHERE Semana = @sem
  AND (@id IS NULL OR Id <> @id);";
            cmd.Parameters.Add("@sem", SqlDbType.Int).Value = numeroSemana;
            var pId = cmd.Parameters.Add("@id", SqlDbType.Int);
            pId.Value = exceptId.HasValue ? exceptId.Value : DBNull.Value;

            cn.Open();
            var x = cmd.ExecuteScalar();
            return x != null;
        }

        // ============================
        //  Nueva implementacion: Validar traslapes de rango
        // ============================
        public bool ExistsOverlap(DateTime fechaInicio, DateTime fechaFinal, int? exceptId = null)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            // Regla: hay traslape si [A1, A2] se cruza con [B1, B2] -> (A1 <= B2) AND (B1 <= A2)
            cmd.CommandText = @"
SELECT TOP(1) 1
FROM dbo.Semanas
WHERE (@id IS NULL OR Id <> @id)
  AND (CAST(Fecha_Inicio AS date) <= @fin)
  AND (CAST(@ini AS date) <= CAST(Fecha_Final AS date));";
            cmd.Parameters.Add("@ini", SqlDbType.Date).Value = fechaInicio.Date;
            cmd.Parameters.Add("@fin", SqlDbType.Date).Value = fechaFinal.Date;
            var pId = cmd.Parameters.Add("@id", SqlDbType.Int);
            pId.Value = exceptId.HasValue ? exceptId.Value : DBNull.Value;

            cn.Open();
            var x = cmd.ExecuteScalar();
            return x != null;
        }

        // ============================
        //  Insert
        // ============================
        public int Insert(Semana s)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = @"
INSERT INTO dbo.Semanas (Semana, Fecha_Inicio, Fecha_Final)
VALUES (@Semana, @Inicio, @Final);
SELECT CAST(SCOPE_IDENTITY() AS int);";

            cmd.Parameters.Add("@Semana", SqlDbType.Int).Value = s.semana;
            cmd.Parameters.Add("@Inicio", SqlDbType.DateTime).Value = s.fecha_Inicio;
            cmd.Parameters.Add("@Final", SqlDbType.DateTime).Value = s.fecha_Final;

            cn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // ============================
        //  Update
        // ============================
        public void Update(Semana s)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = @"
UPDATE dbo.Semanas
SET Semana=@Semana, Fecha_Inicio=@Inicio, Fecha_Final=@Final
WHERE Id=@Id;";

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = s.Id;
            cmd.Parameters.Add("@Semana", SqlDbType.Int).Value = s.semana;
            cmd.Parameters.Add("@Inicio", SqlDbType.DateTime).Value = s.fecha_Inicio;
            cmd.Parameters.Add("@Final", SqlDbType.DateTime).Value = s.fecha_Final;

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

            cmd.CommandText = "DELETE FROM dbo.Semanas WHERE Id=@Id;";
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            cn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
