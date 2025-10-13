// Nueva implementacion
using System;
using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.DAL
{
    public interface IHorasRepository
    {
        (DataTable page, int total) GetHorasPage(int pageIndex, int pageSize, string filtro);
        Hora? GetById(long id);
        long InsertEntrada(long idEmpleado, long carne, DateTime fecha, DateTime horaEntrada, int idUsuario);
        bool TryCerrar(long idHora, DateTime horaSalida, decimal totalHoras);
        Hora? GetAbiertaHoy(long carne, DateTime hoy);
        Hora? GetAbiertaAyer(long carne, DateTime hoyMenosUno);
        void UpdateEntrada(Hora h);
    }

    public sealed class HorasRepository : IHorasRepository
    {
        private readonly Func<SqlConnection> _cnFactory;

        public HorasRepository(Func<SqlConnection> cnFactory) => _cnFactory = cnFactory;

        // Conveniencia: ctor con connection string
        public HorasRepository(string connectionString)
            : this(() => new SqlConnection(connectionString)) { }

        // ===================== Paginación =====================
        public (DataTable page, int total) GetHorasPage(int pageIndex, int pageSize, string? filtro)
        {
            using var cn = _cnFactory();
            cn.Open();

            // Intentamos parsear el filtro a tipos útiles
            int? fInt = null;
            long? fLong = null;
            decimal? fDec = null;
            DateTime? fDate = null;

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var f = filtro.Trim();
                if (int.TryParse(f, out var i)) fInt = i;
                if (long.TryParse(f, out var l)) fLong = l;
                if (decimal.TryParse(
                        f, NumberStyles.Any, CultureInfo.InvariantCulture, out var dec)) fDec = dec;
                if (DateTime.TryParse(f, out var parsedDate)) fDate = parsedDate.Date; // ← renombrado
            }

            // WHERE: cubre todas las columnas mostradas en la grilla
            var where = @"
WHERE (
    @f IS NULL
    OR CONVERT(nvarchar(20), h.Id)               LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(20), h.Id_Empleado)      LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(20), h.Carne)            LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(10), h.Fecha, 23)        LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(10), h.Fecha, 103)       LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(8),  h.Hora_Entrada,108) LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(8),  h.Hora_Salida, 108) LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(20), h.Total_Horas)      LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(20), h.Id_Usuario)       LIKE '%' + @f + '%'
    OR (@fInt  IS NOT NULL  AND (h.Id = @fInt OR h.Id_Usuario = @fInt))
    OR (@fLong IS NOT NULL  AND (h.Id_Empleado = @fLong OR h.Carne = @fLong))
    OR (@fDec  IS NOT NULL  AND h.Total_Horas = @fDec)
    OR (@fDate IS NOT NULL  AND CAST(h.Fecha AS date) = @fDate)
)";

            var sqlPage = $@"
SELECT h.Id, h.Id_Empleado, h.Carne, h.Fecha, h.Hora_Entrada, h.Hora_Salida, h.Total_Horas, h.Id_Usuario
FROM dbo.Horas h
{where}
ORDER BY h.Id DESC
OFFSET (@off) ROWS FETCH NEXT (@ps) ROWS ONLY;";

            var sqlCount = $@"SELECT COUNT(1) FROM dbo.Horas h {where};";

            using var cmdPage = new SqlCommand(sqlPage, cn);
            using var cmdCount = new SqlCommand(sqlCount, cn);

            // Parámetros compartidos
            foreach (var cmd in new[] { cmdPage, cmdCount })
            {
                cmd.Parameters.Add("@f", SqlDbType.NVarChar, 120).Value =
                    string.IsNullOrWhiteSpace(filtro) ? DBNull.Value : filtro!.Trim();

                var pInt = cmd.Parameters.Add("@fInt", SqlDbType.Int);
                pInt.Value = fInt.HasValue ? fInt.Value : DBNull.Value;

                var pLong = cmd.Parameters.Add("@fLong", SqlDbType.BigInt);
                pLong.Value = fLong.HasValue ? fLong.Value : DBNull.Value;

                var pDec = cmd.Parameters.Add("@fDec", SqlDbType.Decimal);
                pDec.Precision = 6; pDec.Scale = 2;
                pDec.Value = fDec.HasValue ? fDec.Value : DBNull.Value;

                var pDate = cmd.Parameters.Add("@fDate", SqlDbType.Date);
                pDate.Value = fDate.HasValue ? fDate.Value : DBNull.Value;
            }

            // Paginación (1-based para ser consistente con EmpleadoRepository)
            cmdPage.Parameters.Add("@off", SqlDbType.Int).Value =
                Math.Max(0, (pageIndex - 1) * Math.Max(pageSize, 1));
            cmdPage.Parameters.Add("@ps", SqlDbType.Int).Value = pageSize;

            var total = Convert.ToInt32(cmdCount.ExecuteScalar());
            var table = new DataTable(); // ← renombrado (antes 'dt')
            using (var rd = cmdPage.ExecuteReader()) table.Load(rd);
            return (table, total);
        }

        // ===================== GetById =====================
        public Hora? GetById(long id)
        {
            using var cn = _cnFactory();
            cn.Open();
            const string sql = @"
                SELECT Id, Id_Empleado, Carne, Fecha, Hora_Entrada, Hora_Salida, Total_Horas, Id_Usuario
                FROM dbo.Horas WHERE Id=@id;";
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = id;
            using var rd = cmd.ExecuteReader();
            return rd.Read() ? Map(rd) : null;
        }

        // ===================== Insert Entrada =====================
        public long InsertEntrada(long idEmpleado, long carne, DateTime fecha, DateTime horaEntrada, int idUsuario)
        {
            using var cn = _cnFactory();
            cn.Open();

            const string sql = @"
                INSERT INTO dbo.Horas
                    (Id_Empleado, Carne, Fecha, Hora_Entrada, Hora_Salida, Total_Horas, Id_Usuario)
                VALUES
                    (@idEmp, @carne, @fecha, @entrada, NULL, NULL, @idUsu);
                SELECT SCOPE_IDENTITY();"; // puede venir como decimal o bigint

            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@idEmp", SqlDbType.BigInt).Value = idEmpleado;
            cmd.Parameters.Add("@carne", SqlDbType.BigInt).Value = carne;
            cmd.Parameters.Add("@fecha", SqlDbType.DateTime2).Value = fecha.Date;
            cmd.Parameters.Add("@entrada", SqlDbType.DateTime2).Value = horaEntrada;
            cmd.Parameters.Add("@idUsu", SqlDbType.Int).Value = idUsuario;

            var obj = cmd.ExecuteScalar();
            return Convert.ToInt64(obj); // robusto: decimal/int64 → long
        }

        // ===================== Cerrar (Salida) =====================
        public bool TryCerrar(long idHora, DateTime horaSalida, decimal totalHoras)
        {
            using var cn = _cnFactory();
            cn.Open();

            const string sql = @"
                UPDATE dbo.Horas
                   SET Hora_Salida = @salida,
                       Total_Horas = @total
                 WHERE Id = @id AND Hora_Salida IS NULL;";

            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@salida", SqlDbType.DateTime2).Value = horaSalida;

            // Tipar decimal(6,2) explícito para evitar problemas de escala
            var pTotal = cmd.Parameters.Add("@total", SqlDbType.Decimal);
            pTotal.Precision = 6; pTotal.Scale = 2;
            pTotal.Value = totalHoras;

            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = idHora;

            return cmd.ExecuteNonQuery() == 1;
        }

        // ===================== Abiertas (hoy/ayer) =====================
        public Hora? GetAbiertaHoy(long carne, DateTime hoy)
        {
            using var cn = _cnFactory();
            cn.Open();
            const string sql = @"
                SELECT TOP 1 Id, Id_Empleado, Carne, Fecha, Hora_Entrada, Hora_Salida, Total_Horas, Id_Usuario
                FROM dbo.Horas
                WHERE Carne=@carne AND CAST(Fecha AS date) = @fecha AND Hora_Salida IS NULL
                ORDER BY Id DESC;";
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@carne", SqlDbType.BigInt).Value = carne;
            cmd.Parameters.Add("@fecha", SqlDbType.Date).Value = hoy.Date;
            using var rd = cmd.ExecuteReader();
            return rd.Read() ? Map(rd) : null;
        }

        public Hora? GetAbiertaAyer(long carne, DateTime hoyMenosUno)
        {
            using var cn = _cnFactory();
            cn.Open();
            const string sql = @"
                SELECT TOP 1 Id, Id_Empleado, Carne, Fecha, Hora_Entrada, Hora_Salida, Total_Horas, Id_Usuario
                FROM dbo.Horas
                WHERE Carne=@carne AND CAST(Fecha AS date) = @fecha AND Hora_Salida IS NULL
                ORDER BY Id DESC;";
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@carne", SqlDbType.BigInt).Value = carne;
            cmd.Parameters.Add("@fecha", SqlDbType.Date).Value = hoyMenosUno.Date;
            using var rd = cmd.ExecuteReader();
            return rd.Read() ? Map(rd) : null;
        }

        // ===================== Update Entrada (si está abierta) =====================
        public void UpdateEntrada(Hora h)
        {
            using var cn = _cnFactory();
            cn.Open();
            const string sql = @"
                UPDATE dbo.Horas
                   SET Fecha=@fecha, Hora_Entrada=@entrada
                 WHERE Id=@id AND Hora_Salida IS NULL;";
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.Add("@fecha", SqlDbType.DateTime2).Value = h.Fecha.Date;
            cmd.Parameters.Add("@entrada", SqlDbType.DateTime2).Value = h.Hora_Entrada;
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = h.Id;
            cmd.ExecuteNonQuery();
        }

        // ===================== Map =====================
        private static Hora Map(SqlDataReader rd)
        {
            return new Hora
            {
                Id = rd.GetInt64(0),
                Id_Empleado = rd.GetInt64(1),
                Carne = rd.GetInt64(2),
                Fecha = rd.GetDateTime(3),
                Hora_Entrada = rd.GetDateTime(4),
                Hora_Salida = rd.IsDBNull(5) ? (DateTime?)null : rd.GetDateTime(5),
                Total_Horas = rd.IsDBNull(6) ? (decimal?)null : rd.GetDecimal(6),
                Id_Usuario = rd.GetInt32(7)
            };
        }
    }
}
