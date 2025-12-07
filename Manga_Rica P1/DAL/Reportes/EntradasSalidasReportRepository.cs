using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports.Dtos;
using Microsoft.Data.SqlClient;

namespace Manga_Rica_P1.DAL.Reports
{
    public sealed class EntradasSalidasReportRepository : IEntradasSalidasReportRepository
    {
        private readonly string _connectionString;

        public EntradasSalidasReportRepository(string clockConnectionString)
        {
            _connectionString = clockConnectionString
                ?? throw new ArgumentNullException(nameof(clockConnectionString));
        }

        public async Task<IReadOnlyList<EntradasSalidasRowDto>> GetEntradasSalidasPorRangoAsync(
            DateTime desde,
            DateTime hasta,
            CancellationToken ct = default)
        {
            var fechaDesde = desde.Date;
            var fechaHasta = hasta.Date.AddDays(1); // exclusivo

            const string sql = @"
SELECT
    ca._date                    AS Fecha,
    emp.code                    AS Carne,
    ISNULL(emp.lastName, '')    AS Apellidos,
    ISNULL(emp.name, '')        AS Nombre,
    ca.startEnroll              AS HoraEntrada,
    ca.endEnroll                AS HoraSalida,
    CAST(ISNULL(ca.total, 0) AS INT) AS TotalMinutos  -- smallint -> int
FROM Bit2.dbo.calculatedAttendance AS ca
INNER JOIN Bit2.dbo.employees AS emp
    ON ca.idEmployee = emp.id
WHERE ca._date >= @Desde
  AND ca._date <  @Hasta
ORDER BY
    ca._date,
    emp.lastName,
    emp.name,
    ca.startEnroll;";

            var result = new List<EntradasSalidasRowDto>();

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync(ct);

            await using var cmd = new SqlCommand(sql, conn)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.Add(new SqlParameter("@Desde", SqlDbType.DateTime) { Value = fechaDesde });
            cmd.Parameters.Add(new SqlParameter("@Hasta", SqlDbType.DateTime) { Value = fechaHasta });

            await using var reader = await cmd.ExecuteReaderAsync(ct);

            while (await reader.ReadAsync(ct))
            {
                var row = new EntradasSalidasRowDto
                {
                    Fecha = reader.GetDateTime(0),
                    Carne = reader.IsDBNull(1) ? "" : reader.GetString(1),   // string (code)
                    Apellidos = reader.GetString(2),
                    Nombre = reader.GetString(3),
                    HoraEntrada = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                    HoraSalida = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                    TotalMinutos = reader.IsDBNull(6) ? 0 : reader.GetInt32(6)     // ya es INT
                };

                result.Add(row);
            }

            return result;
        }

        public async Task<IReadOnlyList<EntradasSalidasRowDto>> GetEntradasSalidasPorEmpleadoAsync(
            DateTime desde,
            DateTime hasta,
            string carne,
            CancellationToken ct = default)
        {
            var fechaDesde = desde.Date;
            var fechaHasta = hasta.Date.AddDays(1); // exclusivo

            const string sql = @"
SELECT
    ca._date                    AS Fecha,
    emp.code                    AS Carne,
    ISNULL(emp.lastName, '')    AS Apellidos,
    ISNULL(emp.name, '')        AS Nombre,
    ca.startEnroll              AS HoraEntrada,
    ca.endEnroll                AS HoraSalida,
    CAST(ISNULL(ca.total, 0) AS INT) AS TotalMinutos
FROM Bit2.dbo.calculatedAttendance AS ca
INNER JOIN Bit2.dbo.employees AS emp
    ON ca.idEmployee = emp.id
WHERE ca._date >= @Desde
  AND ca._date <  @Hasta
  AND emp.code = @Carne
ORDER BY
    ca._date,
    emp.lastName,
    emp.name,
    ca.startEnroll;";

            var result = new List<EntradasSalidasRowDto>();

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync(ct);

            await using var cmd = new SqlCommand(sql, conn)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.Add(new SqlParameter("@Desde", SqlDbType.DateTime) { Value = fechaDesde });
            cmd.Parameters.Add(new SqlParameter("@Hasta", SqlDbType.DateTime) { Value = fechaHasta });
            cmd.Parameters.Add(new SqlParameter("@Carne", SqlDbType.VarChar, 50) { Value = carne });

            await using var reader = await cmd.ExecuteReaderAsync(ct);

            while (await reader.ReadAsync(ct))
            {
                var row = new EntradasSalidasRowDto
                {
                    Fecha = reader.GetDateTime(0),
                    Carne = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    Apellidos = reader.GetString(2),
                    Nombre = reader.GetString(3),
                    HoraEntrada = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                    HoraSalida = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                    TotalMinutos = reader.IsDBNull(6) ? 0 : reader.GetInt32(6)
                };

                result.Add(row);
            }

            return result;
        }
    }
}
