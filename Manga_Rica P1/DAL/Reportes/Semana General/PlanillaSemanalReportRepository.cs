using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.DAL.Reports.Dtos;

namespace Manga_Rica_P1.DAL.Reports
{
    /// <summary>
    /// Repositorio ADO.NET para leer la planilla semanal
    /// desde las tablas reales: Pagos, Empleados, Semanas, Departamentos.
    /// </summary>
    public sealed class PlanillaSemanalReportRepository : IPlanillaSemanalReportRepository
    {
        private readonly string _connectionString;

        public PlanillaSemanalReportRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Devuelve todas las filas de planilla para una semana dada.
        /// </summary>
        public async Task<IReadOnlyList<PlanillaSemanalRowDto>> GetPlanillaSemanalAsync(
            int semana,
            CancellationToken ct = default)
        {
            // IMPORTANTE: nombres de tablas y columnas según el script:
            //  - Semanas: Id, Semana, Fecha_Inicio, Fecha_Final
            //  - Empleados: Id, Carne, Cedula, Primer_Apellido, Segundo_Apellido, Nombre, Id_Departamento, Salario
            //  - Departamentos: Id, Departamento, Codigo
            //  - Pagos: Id, Id_Empleado, Fecha, Id_Semana, Horas_Normales, Horas_Extras,
            //           Horas_Dobles, Feriadas, Deduccion_Soda, Deduccion_Uniforme,
            //           Deduccion_Otras, Salario_Bruto, Salario_Neto, Id_Usuario, Registrado
            const string sql = @"
SELECT
    s.Semana                          AS Semana,
    s.Fecha_Inicio                    AS FechaInicio,
    s.Fecha_Final                     AS FechaFin,
    d.Departamento                    AS Departamento,

    e.Carne                           AS Carne,
    e.Cedula                          AS Cedula,
    (e.Primer_Apellido + ' ' +
     e.Segundo_Apellido + ' ' +
     e.Nombre)                        AS Nombre,

    p.Horas_Normales                  AS HorasNormales,
    p.Horas_Extras                    AS HorasExtras,
    p.Horas_Dobles                    AS HorasDobles,
    p.Feriadas                        AS Feriados,

    e.Salario                         AS SalarioHora,

    p.Deduccion_Soda                  AS Soda,
    p.Deduccion_Uniforme              AS Uniforme,

    p.Salario_Bruto                   AS SalarioBruto,
    p.Salario_Neto                    AS SalarioNeto
FROM dbo.Pagos          AS p
INNER JOIN dbo.Empleados    AS e ON p.Id_Empleado = e.Id
INNER JOIN dbo.Departamentos AS d ON e.Id_Departamento = d.Id
INNER JOIN dbo.Semanas       AS s ON p.Id_Semana     = s.Id
WHERE
    s.Semana = @Semana
ORDER BY
    d.Departamento,
    e.Primer_Apellido,
    e.Segundo_Apellido,
    e.Nombre,
    e.Carne;";

            var result = new List<PlanillaSemanalRowDto>();

            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add("@Semana", SqlDbType.Int).Value = semana;

                await cn.OpenAsync(ct).ConfigureAwait(false);

                using var rd = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false);
                while (await rd.ReadAsync(ct).ConfigureAwait(false))
                {
                    var row = new PlanillaSemanalRowDto
                    {
                        Semana = rd.GetInt32(rd.GetOrdinal("Semana")),
                        FechaInicio = rd.GetDateTime(rd.GetOrdinal("FechaInicio")),
                        FechaFin = rd.GetDateTime(rd.GetOrdinal("FechaFin")),
                        Departamento = rd["Departamento"] as string ?? string.Empty,

                        Carne = Convert.ToInt64(rd["Carne"]),
                        Cedula = rd["Cedula"] as string ?? string.Empty,
                        Empleado = rd["Nombre"] as string ?? string.Empty,

                        HorasNormales = Convert.ToDecimal(rd["HorasNormales"]),
                        HorasExtras = Convert.ToDecimal(rd["HorasExtras"]),
                        HorasDobles = Convert.ToDecimal(rd["HorasDobles"]),
                        Feriados = Convert.ToDecimal(rd["Feriados"]),

                        SalarioHora = Convert.ToDecimal(rd["SalarioHora"]),

                        Soda = Convert.ToDecimal(rd["Soda"]),
                        Uniforme = Convert.ToDecimal(rd["Uniforme"]),

                        SalarioBruto = Convert.ToDecimal(rd["SalarioBruto"]),
                        SalarioNeto = Convert.ToDecimal(rd["SalarioNeto"])
                    };

                    result.Add(row);
                }
            }

            return result;
        }
    }
}
