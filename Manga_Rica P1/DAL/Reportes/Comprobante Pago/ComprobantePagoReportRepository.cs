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
    /// Implementación ADO.NET del repositorio de Comprobantes de Pago.
    /// </summary>
    public sealed class ComprobantePagoReportRepository : IComprobantePagoReportRepository
    {
        private readonly string _connectionString;

        public ComprobantePagoReportRepository(string connectionString)
        {
            _connectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IReadOnlyList<ComprobantePagoRowDto>> GetComprobantesPorSemanaAsync(
            int semana,
            CancellationToken ct = default)
        {
            const string sql = @"
SELECT
    s.Semana                       AS Semana,
    p.Fecha                        AS Fecha,
    s.Fecha_Inicio                 AS FechaInicio,
    s.Fecha_Final                  AS FechaFin,
    e.Carne                        AS Carne,
    e.Cedula                       AS Cedula,
    e.Nombre                       AS Nombre,
    e.Primer_Apellido              AS PrimerApellido,
    e.Segundo_Apellido             AS SegundoApellido,
    d.Departamento                 AS Departamento,
    p.Horas_Normales               AS HorasNormales,
    p.Horas_Extras                 AS HorasExtras,
    p.Horas_Dobles                 AS HorasDobles,
    p.Feriadas                     AS Feriados,
    p.Deduccion_Soda               AS DeduccionSoda,
    p.Deduccion_Uniforme           AS DeduccionUniforme,
    p.Deduccion_Otras              AS DeduccionOtras,
    p.Salario_Bruto                AS SalarioBruto,
    p.Salario_Neto                 AS SalarioNeto
FROM dbo.Pagos       AS p
INNER JOIN dbo.Empleados     AS e ON p.Id_Empleado   = e.Id
INNER JOIN dbo.Semanas       AS s ON p.Id_Semana     = s.Id
LEFT  JOIN dbo.Departamentos AS d ON e.Id_Departamento = d.Id
WHERE s.Semana = @Semana
ORDER BY e.Carne;
";

            var resultado = new List<ComprobantePagoRowDto>();

            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(ct).ConfigureAwait(false);

            await using var command = new SqlCommand(sql, connection)
            {
                CommandType = CommandType.Text
            };
            command.Parameters.Add("@Semana", SqlDbType.Int).Value = semana;

            // Usamos CommandBehavior.CloseConnection para asegurar que la conexión se cierre con el reader.
            await using var reader = await command
                .ExecuteReaderAsync(CommandBehavior.CloseConnection, ct)
                .ConfigureAwait(false);

            while (await reader.ReadAsync(ct).ConfigureAwait(false))
            {
                var row = new ComprobantePagoRowDto
                {
                    Semana = (int)reader["Semana"],
                    Fecha = (DateTime)reader["Fecha"],
                    FechaInicio = (DateTime)reader["FechaInicio"],
                    FechaFin = (DateTime)reader["FechaFin"],
                    Carne = (long)reader["Carne"],
                    Cedula = Convert.ToString(reader["Cedula"]) ?? string.Empty,
                    Nombre = Convert.ToString(reader["Nombre"]) ?? string.Empty,
                    PrimerApellido = Convert.ToString(reader["PrimerApellido"]) ?? string.Empty,
                    SegundoApellido = Convert.ToString(reader["SegundoApellido"]) ?? string.Empty,
                    Departamento = Convert.ToString(reader["Departamento"]) ?? string.Empty,

                    HorasNormales = Convert.ToDecimal(reader["HorasNormales"]),
                    HorasExtras = Convert.ToDecimal(reader["HorasExtras"]),
                    HorasDobles = Convert.ToDecimal(reader["HorasDobles"]),
                    Feriados = Convert.ToDecimal(reader["Feriados"]),
                    DeduccionSoda = Convert.ToDecimal(reader["DeduccionSoda"]),
                    DeduccionUniforme = Convert.ToDecimal(reader["DeduccionUniforme"]),
                    DeduccionOtras = Convert.ToDecimal(reader["DeduccionOtras"]),
                    SalarioBruto = Convert.ToDecimal(reader["SalarioBruto"]),
                    SalarioNeto = Convert.ToDecimal(reader["SalarioNeto"])
                };

                resultado.Add(row);
            }

            return resultado;
        }
    }
}
