using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports.Dtos;
using Microsoft.Data.SqlClient;

namespace Manga_Rica_P1.DAL.Reports
{
    /// <summary>
    /// Implementación SQL para el reporte de Horas Semanales.
    /// Usa tablas: Acumulado_Diario y Empleados.
    /// </summary>
    public sealed class HorasSemanalesReportRepository : IHorasSemanalesReportRepository
    {
        private readonly string _connectionString;

        public HorasSemanalesReportRepository(string connectionString)
        {
            _connectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IReadOnlyList<HorasSemanalesRowDto>> GetHorasSemanalesPorRangoAsync(
            DateTime desde,
            DateTime hasta,
            CancellationToken ct = default)
        {
            // Normalizamos rango a fechas (sin hora)
            var fechaDesde = desde.Date;
            var fechaHastaExclusiva = hasta.Date.AddDays(1); // < Hasta, inclusivo para el usuario

            const string sql = @"
SELECT
    emp.Carne                                        AS Carne,
    LTRIM(RTRIM(
        ISNULL(emp.Primer_Apellido, '') + ' ' +
        ISNULL(emp.Segundo_Apellido, '')
    ))                                               AS Apellidos,
    ISNULL(emp.Nombre, '')                           AS Nombre,
    SUM(ISNULL(ad.Normales, 0))                      AS TotalNormales,
    SUM(ISNULL(ad.Extras,   0))                      AS TotalExtras,
    SUM(ISNULL(ad.Dobles,   0))                      AS TotalDobles,
    SUM(ISNULL(ad.Feriado,  0))                      AS TotalFeriado
FROM dbo.Acumulado_Diario AS ad
INNER JOIN dbo.Empleados AS emp
    ON ad.Id_Empleado = emp.Id
WHERE ad.Fecha >= @Desde
  AND ad.Fecha <  @Hasta
GROUP BY
    emp.Carne,
    emp.Primer_Apellido,
    emp.Segundo_Apellido,
    emp.Nombre
ORDER BY
    Apellidos,
    Nombre,
    emp.Carne;";

            var result = new List<HorasSemanalesRowDto>();

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync(ct);

            await using var cmd = new SqlCommand(sql, conn)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.Add(new SqlParameter("@Desde", SqlDbType.DateTime) { Value = fechaDesde });
            cmd.Parameters.Add(new SqlParameter("@Hasta", SqlDbType.DateTime) { Value = fechaHastaExclusiva });

            await using var reader = await cmd.ExecuteReaderAsync(ct);

            while (await reader.ReadAsync(ct))
            {
                // Usamos Convert.ToX para evitar problemas de Int16/Int32/Int64
                var carneObj = reader.GetValue(0);
                var normalesObj = reader.GetValue(3);
                var extrasObj = reader.GetValue(4);
                var doblesObj = reader.GetValue(5);
                var feriadoObj = reader.GetValue(6);

                var row = new HorasSemanalesRowDto
                {
                    Carne = carneObj == DBNull.Value ? 0L : Convert.ToInt64(carneObj),
                    Apellidos = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    Nombre = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    HorasNormales = normalesObj == DBNull.Value ? 0m : Convert.ToDecimal(normalesObj),
                    HorasExtras = extrasObj == DBNull.Value ? 0m : Convert.ToDecimal(extrasObj),
                    HorasDobles = doblesObj == DBNull.Value ? 0m : Convert.ToDecimal(doblesObj),
                    HorasFeriado = feriadoObj == DBNull.Value ? 0m : Convert.ToDecimal(feriadoObj)
                };

                result.Add(row);
            }

            return result;
        }
    }
}
