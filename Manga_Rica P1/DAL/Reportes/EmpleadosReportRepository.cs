// Nueva implementacion
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.DAL.Reports.Dtos;

namespace Manga_Rica_P1.DAL.Reports
{
    public interface IEmpleadosReportRepository
    {
        // Nueva implementacion: interfaz clara para tests/DI
        Task<IReadOnlyList<EmpleadoReportRow>> GetEmpleadosActivosAsync(CancellationToken ct = default);
        Task<IReadOnlyList<EmpleadoReportRow>> GetEmpleadosInactivosAsync(CancellationToken ct = default);
    }

    public sealed class EmpleadosReportRepository : IEmpleadosReportRepository
    {
        private readonly string _cs;
        public EmpleadosReportRepository(string connectionString) => _cs = connectionString;

        // Nueva implementacion: consulta orientada a reporte (solo lectura) - ACTIVOS
        public async Task<IReadOnlyList<EmpleadoReportRow>> GetEmpleadosActivosAsync(CancellationToken ct = default)
        {
            var list = new List<EmpleadoReportRow>();
            const string sql = @"
SELECT 
    d.Departamento                              AS Departamento,
    CAST(e.Carne AS varchar(50))                AS Carne,           -- BIGINT -> texto para el reporte
    e.Primer_Apellido                           AS Apellido1,
    e.Segundo_Apellido                          AS Apellido2,
    e.Nombre                                    AS Nombre,
    CAST(e.Salario AS decimal(18,2))            AS Salario,         -- evitar artefactos de float
    e.Puesto                                    AS Puesto,
    e.Fecha_Ingreso                             AS FechaIngreso
FROM dbo.Empleados e
JOIN dbo.Departamentos d ON d.Id = e.Id_Departamento
WHERE e.Activo = 1
ORDER BY d.Departamento, e.Primer_Apellido, e.Segundo_Apellido, e.Nombre;";

            using var cn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(sql, cn) { CommandType = CommandType.Text };
            await cn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);

            // Nueva implementacion: mapeo por nombre (coinciden con los alias del SELECT)
            int iDepto = rd.GetOrdinal("Departamento");
            int iCarne = rd.GetOrdinal("Carne");
            int iAp1 = rd.GetOrdinal("Apellido1");
            int iAp2 = rd.GetOrdinal("Apellido2");
            int iNom = rd.GetOrdinal("Nombre");
            int iSal = rd.GetOrdinal("Salario");
            int iPue = rd.GetOrdinal("Puesto");
            int iFec = rd.GetOrdinal("FechaIngreso");

            while (await rd.ReadAsync(ct))
            {
                list.Add(new EmpleadoReportRow
                {
                    Departamento = rd.GetString(iDepto),
                    Carne = rd.GetString(iCarne),
                    Apellido1 = rd.GetString(iAp1),
                    Apellido2 = rd.GetString(iAp2),
                    Nombre = rd.GetString(iNom),
                    Salario = rd.GetFieldValue<decimal>(iSal),
                    Puesto = rd.IsDBNull(iPue) ? "" : rd.GetString(iPue),
                    FechaIngreso = rd.GetDateTime(iFec)
                });
            }
            return list;
        }

        // Nueva implementacion: consulta orientada a reporte (solo lectura) - INACTIVOS
        public async Task<IReadOnlyList<EmpleadoReportRow>> GetEmpleadosInactivosAsync(CancellationToken ct = default)
        {
            var list = new List<EmpleadoReportRow>();
            const string sql = @"
SELECT 
    d.Departamento                              AS Departamento,
    CAST(e.Carne AS varchar(50))                AS Carne,
    e.Primer_Apellido                           AS Apellido1,
    e.Segundo_Apellido                          AS Apellido2,
    e.Nombre                                    AS Nombre,
    CAST(e.Salario AS decimal(18,2))            AS Salario,
    e.Puesto                                    AS Puesto,
    e.Fecha_Ingreso                             AS FechaIngreso
FROM dbo.Empleados e
JOIN dbo.Departamentos d ON d.Id = e.Id_Departamento
WHERE e.Activo = 0
ORDER BY d.Departamento, e.Primer_Apellido, e.Segundo_Apellido, e.Nombre;";

            using var cn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(sql, cn) { CommandType = CommandType.Text };
            await cn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);

            int iDepto = rd.GetOrdinal("Departamento");
            int iCarne = rd.GetOrdinal("Carne");
            int iAp1 = rd.GetOrdinal("Apellido1");
            int iAp2 = rd.GetOrdinal("Apellido2");
            int iNom = rd.GetOrdinal("Nombre");
            int iSal = rd.GetOrdinal("Salario");
            int iPue = rd.GetOrdinal("Puesto");
            int iFec = rd.GetOrdinal("FechaIngreso");

            while (await rd.ReadAsync(ct))
            {
                list.Add(new EmpleadoReportRow
                {
                    Departamento = rd.GetString(iDepto),
                    Carne = rd.GetString(iCarne),
                    Apellido1 = rd.GetString(iAp1),
                    Apellido2 = rd.GetString(iAp2),
                    Nombre = rd.GetString(iNom),
                    Salario = rd.GetFieldValue<decimal>(iSal),
                    Puesto = rd.IsDBNull(iPue) ? "" : rd.GetString(iPue),
                    FechaIngreso = rd.GetDateTime(iFec)
                });
            }
            return list;
        }
    }
}
