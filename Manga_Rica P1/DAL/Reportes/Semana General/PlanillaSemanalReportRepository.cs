using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports.Dtos;
using Microsoft.Data.SqlClient;

namespace Manga_Rica_P1.DAL.Reports
{
    public sealed class PlanillaSemanalReportRepository : IPlanillaSemanalReportRepository
    {
        private readonly string _connectionString;

        public PlanillaSemanalReportRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // ================== MÉTODO GENERAL (TODOS LOS EMPLEADOS) ==================

        public async Task<IReadOnlyList<PlanillaSemanalRowDto>> GetPlanillaSemanalAsync(
            int semana,
            CancellationToken ct = default)
        {
            const string sql = @"
SELECT
    s.Semana                 AS Semana,
    s.Fecha_Inicio           AS FechaInicio,
    s.Fecha_Final            AS FechaFin,
    d.Departamento           AS Departamento,
    e.Carne                  AS Carne,
    e.Cedula                 AS Cedula,
    (e.Nombre + ' ' + e.Primer_Apellido + ' ' + e.Segundo_Apellido) AS Empleado,
    p.Horas_Normales         AS HorasNormales,
    p.Horas_Extras           AS HorasExtras,
    p.Horas_Dobles           AS HorasDobles,
    p.Feriadas               AS Feriados,
    e.Salario                AS SalarioHora,
    p.Deduccion_Soda         AS Soda,
    p.Deduccion_Uniforme     AS Uniforme,
    p.Salario_Bruto          AS SalarioBruto,
    p.Salario_Neto           AS SalarioNeto
FROM dbo.Pagos p
INNER JOIN dbo.Empleados e
    ON p.Id_Empleado = e.Id
INNER JOIN dbo.Departamentos d
    ON e.Id_Departamento = d.Id
INNER JOIN dbo.Semanas s
    ON p.Id_Semana = s.Id
WHERE
    p.Registrado = 1
    AND s.Semana = @Semana
ORDER BY
    d.Departamento,
    e.Carne;";

            return await ExecuteQueryAsync(
                sql,
                cmd =>
                {
                    cmd.Parameters.Add("@Semana", SqlDbType.Int).Value = semana;
                },
                ct
            ).ConfigureAwait(false);
        }

        // ================== MÉTODO POR CARNET ==================

        public async Task<IReadOnlyList<PlanillaSemanalRowDto>> GetPlanillaSemanalPorEmpleadoAsync(
            int semana,
            long carne,
            CancellationToken ct = default)
        {
            const string sql = @"
SELECT
    s.Semana                 AS Semana,
    s.Fecha_Inicio           AS FechaInicio,
    s.Fecha_Final            AS FechaFin,
    d.Departamento           AS Departamento,
    e.Carne                  AS Carne,
    e.Cedula                 AS Cedula,
    (e.Nombre + ' ' + e.Primer_Apellido + ' ' + e.Segundo_Apellido) AS Empleado,
    p.Horas_Normales         AS HorasNormales,
    p.Horas_Extras           AS HorasExtras,
    p.Horas_Dobles           AS HorasDobles,
    p.Feriadas               AS Feriados,
    e.Salario                AS SalarioHora,
    p.Deduccion_Soda         AS Soda,
    p.Deduccion_Uniforme     AS Uniforme,
    p.Salario_Bruto          AS SalarioBruto,
    p.Salario_Neto           AS SalarioNeto
FROM dbo.Pagos p
INNER JOIN dbo.Empleados e
    ON p.Id_Empleado = e.Id
INNER JOIN dbo.Departamentos d
    ON e.Id_Departamento = d.Id
INNER JOIN dbo.Semanas s
    ON p.Id_Semana = s.Id
WHERE
    p.Registrado = 1
    AND s.Semana = @Semana
    AND e.Carne = @Carne
ORDER BY
    d.Departamento,
    e.Carne;";

            return await ExecuteQueryAsync(
                sql,
                cmd =>
                {
                    cmd.Parameters.Add("@Semana", SqlDbType.Int).Value = semana;

                    // Ajusta el tipo (BigInt/Int) según el tipo real de la columna Carne en la BD.
                    cmd.Parameters.Add("@Carne", SqlDbType.BigInt).Value = carne;
                },
                ct
            ).ConfigureAwait(false);
        }

        // ================== MÉTODO POR DEPARTAMENTO ==================

        public async Task<IReadOnlyList<PlanillaSemanalRowDto>> GetPlanillaSemanalPorDepartamentoAsync(
            int semana,
            int idDepartamento,
            CancellationToken ct = default)
        {
            const string sql = @"
SELECT
    s.Semana                 AS Semana,
    s.Fecha_Inicio           AS FechaInicio,
    s.Fecha_Final            AS FechaFin,
    d.Departamento           AS Departamento,
    e.Carne                  AS Carne,
    e.Cedula                 AS Cedula,
    (e.Nombre + ' ' + e.Primer_Apellido + ' ' + e.Segundo_Apellido) AS Empleado,
    p.Horas_Normales         AS HorasNormales,
    p.Horas_Extras           AS HorasExtras,
    p.Horas_Dobles           AS HorasDobles,
    p.Feriadas               AS Feriados,
    e.Salario                AS SalarioHora,
    p.Deduccion_Soda         AS Soda,
    p.Deduccion_Uniforme     AS Uniforme,
    p.Salario_Bruto          AS SalarioBruto,
    p.Salario_Neto           AS SalarioNeto
FROM dbo.Pagos p
INNER JOIN dbo.Empleados e
    ON p.Id_Empleado = e.Id
INNER JOIN dbo.Departamentos d
    ON e.Id_Departamento = d.Id
INNER JOIN dbo.Semanas s
    ON p.Id_Semana = s.Id
WHERE
    p.Registrado   = 1
    AND s.Semana   = @Semana
    AND d.Id       = @IdDepartamento   -- o e.Id_Departamento = @IdDepartamento
ORDER BY
    d.Departamento,
    e.Carne;";

            return await ExecuteQueryAsync(
                sql,
                cmd =>
                {
                    cmd.Parameters.Add("@Semana", SqlDbType.Int).Value = semana;
                    cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int).Value = idDepartamento;
                },
                ct
            ).ConfigureAwait(false);
        }

        // ================== MÉTODO AUXILIAR COMÚN ==================

        private async Task<IReadOnlyList<PlanillaSemanalRowDto>> ExecuteQueryAsync(
            string sql,
            Action<SqlCommand> configure,
            CancellationToken ct)
        {
            var result = new List<PlanillaSemanalRowDto>();

            await using var cn = new SqlConnection(_connectionString);
            await using var cmd = new SqlCommand(sql, cn);

            configure(cmd);

            await cn.OpenAsync(ct).ConfigureAwait(false);
            await using var rd = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false);

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
                    Empleado = rd["Empleado"] as string ?? string.Empty,

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

            return result;
        }
    }
}
