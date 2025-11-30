using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.DAL.Reports.Dtos;

namespace Manga_Rica_P1.DAL.Reports
{
    public interface IUniformesReportRepository
    {
        Task<IReadOnlyList<UniformeReportRow>> GetUniformesAsync(
            string categoriaArticulo,
            CancellationToken ct = default);
    }

    public sealed class UniformesReportRepository : IUniformesReportRepository
    {
        private readonly string _cs;

        public UniformesReportRepository(string connectionString)
        {
            _cs = connectionString;
        }

        public async Task<IReadOnlyList<UniformeReportRow>> GetUniformesAsync(
            string categoriaArticulo,
            CancellationToken ct = default)
        {
            const string sql = @"
SELECT
    ISNULL(dep.Departamento, 'Sin Departamento') AS Departamento,
    CAST(e.Carne AS varchar(50))                AS Carne,
    (e.Primer_Apellido + ' ' + e.Segundo_Apellido) AS Apellidos,
    e.Nombre                                    AS Nombre,
    d.Id                                        AS IdDeduccion,
    dd.Codigo_Articulo                          AS CodigoArticulo,
    a.Descripcion                               AS DescripcionArticulo,
    dd.Cantidad                                 AS Cantidad,
    CAST(dd.Precio AS decimal(18,2))            AS PrecioUnitario,
    CAST(dd.Total  AS decimal(18,2))            AS TotalLinea,
    CAST(d.Saldo   AS decimal(18,2))            AS SaldoDeduccion
FROM dbo.Deducciones d
JOIN dbo.Empleados e
    ON e.Id = d.Id_Empleado
JOIN dbo.Deducciones_Detalles dd
    ON dd.Id_Deduccion = d.Id
JOIN dbo.Articulos a
    ON a.Id = dd.Codigo_Articulo
LEFT JOIN dbo.Departamentos dep
    ON dep.Id = e.Id_Departamento
WHERE
    d.Anulada = 0
    AND a.Categoria = @Categoria
ORDER BY
    Apellidos,
    Nombre,
    d.Id,
    DescripcionArticulo;";

            var list = new List<UniformeReportRow>();

            using var cn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(sql, cn) { CommandType = CommandType.Text };
            cmd.Parameters.Add(new SqlParameter("@Categoria", categoriaArticulo));

            await cn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);

            int iDepto = rd.GetOrdinal("Departamento");
            int iCarne = rd.GetOrdinal("Carne");
            int iApell = rd.GetOrdinal("Apellidos");
            int iNombre = rd.GetOrdinal("Nombre");
            int iIdDed = rd.GetOrdinal("IdDeduccion");
            int iCodArt = rd.GetOrdinal("CodigoArticulo");
            int iDesc = rd.GetOrdinal("DescripcionArticulo");
            int iCant = rd.GetOrdinal("Cantidad");
            int iPrecio = rd.GetOrdinal("PrecioUnitario");
            int iTotal = rd.GetOrdinal("TotalLinea");
            int iSaldo = rd.GetOrdinal("SaldoDeduccion");

            while (await rd.ReadAsync(ct))
            {
                list.Add(new UniformeReportRow
                {
                    Departamento = rd.GetString(iDepto),
                    Carne = rd.GetString(iCarne),
                    Apellidos = rd.GetString(iApell),
                    Nombre = rd.GetString(iNombre),
                    IdDeduccion = rd.GetInt64(iIdDed),
                    CodigoArticulo = rd.GetInt32(iCodArt),
                    DescripcionArticulo = rd.GetString(iDesc),
                    Cantidad = rd.GetInt32(iCant),
                    PrecioUnitario = rd.GetDecimal(iPrecio),
                    TotalLinea = rd.GetDecimal(iTotal),
                    SaldoDeduccion = rd.GetDecimal(iSaldo)
                });
            }

            return list;
        }
    }
}
