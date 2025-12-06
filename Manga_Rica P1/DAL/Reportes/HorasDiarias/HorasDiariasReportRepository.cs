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
    /// Repositorio ADO.NET para leer datos del reporte de Horas Diarias
    /// desde las tablas Acumulado_Diario y Empleados.
    /// </summary>
    public sealed class HorasDiariasReportRepository : IHorasDiariasReportRepository
    {
        private readonly string _connectionString;

        public HorasDiariasReportRepository(string connectionString)
        {
            _connectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IReadOnlyList<HorasDiariasRowDto>> GetHorasDiariasPorFechaAsync(
            DateTime fecha,
            CancellationToken ct = default)
        {
            // Usamos un rango [desde, hasta) para evitar problemas con la parte de hora.
            var desde = fecha.Date;
            var hasta = desde.AddDays(1);

            const string sql = @"
SELECT
    ad.Id_Empleado        AS IdEmpleado,
    ad.Fecha              AS Fecha,
    ad.Normales           AS Normales,
    ad.Extras             AS Extras,
    ad.Dobles             AS Dobles,
    ad.Feriado            AS Feriado,
    e.Carne               AS Carne,
    e.Primer_Apellido     AS PrimerApellido,
    e.Segundo_Apellido    AS SegundoApellido,
    e.Nombre              AS Nombre
FROM dbo.Acumulado_Diario ad
INNER JOIN dbo.Empleados e
    ON ad.Id_Empleado = e.Id
WHERE ad.Fecha >= @Desde
  AND ad.Fecha <  @Hasta
ORDER BY
    e.Primer_Apellido,
    e.Segundo_Apellido,
    e.Nombre;";

            var result = new List<HorasDiariasRowDto>();

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync(ct).ConfigureAwait(false);

            await using var cmd = new SqlCommand(sql, conn)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.Add(new SqlParameter("@Desde", SqlDbType.DateTime)
            {
                Value = desde
            });
            cmd.Parameters.Add(new SqlParameter("@Hasta", SqlDbType.DateTime)
            {
                Value = hasta
            });

            await using var reader = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false);

            // Índices de columnas (por claridad, mantenemos los ordinales como ya los usas)
            const int IDX_ID_EMPLEADO = 0;
            const int IDX_FECHA = 1;
            const int IDX_NORMALES = 2;
            const int IDX_EXTRAS = 3;
            const int IDX_DOBLES = 4;
            const int IDX_FERIADO = 5;
            const int IDX_CARNE = 6;
            const int IDX_PRIMER_APELLIDO = 7;
            const int IDX_SEGUNDO_APELLIDO = 8;
            const int IDX_NOMBRE = 9;

            while (await reader.ReadAsync(ct).ConfigureAwait(false))
            {
                var row = new HorasDiariasRowDto
                {
                    IdEmpleado = reader.GetInt64(IDX_ID_EMPLEADO),
                    Fecha = reader.GetDateTime(IDX_FECHA),

                    // IMPORTANTE:
                    // Las columnas float de SQL Server se leen como Double en .NET.
                    // Usamos GetDouble + Convert.ToSingle para mapear al float del DTO.
                    Normales = reader.IsDBNull(IDX_NORMALES)
                        ? 0f
                        : Convert.ToSingle(reader.GetDouble(IDX_NORMALES)),

                    Extras = reader.IsDBNull(IDX_EXTRAS)
                        ? 0f
                        : Convert.ToSingle(reader.GetDouble(IDX_EXTRAS)),

                    Dobles = reader.IsDBNull(IDX_DOBLES)
                        ? 0f
                        : Convert.ToSingle(reader.GetDouble(IDX_DOBLES)),

                    Feriado = reader.IsDBNull(IDX_FERIADO)
                        ? 0f
                        : Convert.ToSingle(reader.GetDouble(IDX_FERIADO)),

                    Carne = reader.GetInt64(IDX_CARNE),
                    PrimerApellido = reader.GetString(IDX_PRIMER_APELLIDO),
                    SegundoApellido = reader.GetString(IDX_SEGUNDO_APELLIDO),
                    Nombre = reader.GetString(IDX_NOMBRE)
                };

                result.Add(row);
            }

            return result;
        }
    }
}
