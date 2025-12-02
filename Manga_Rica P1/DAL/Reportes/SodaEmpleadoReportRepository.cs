using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.DAL.Reports.Dtos;

namespace Manga_Rica_P1.DAL.Reports
{
    public interface ISodaEmpleadoReportRepository
    {
        Task<IReadOnlyList<SodaEmpleadoRow>> GetSodaPorEmpleadoAsync(
            long carne,
            DateTime fechaDesde,
            DateTime fechaHasta,
            CancellationToken ct = default);
    }

    public sealed class SodaEmpleadoReportRepository : ISodaEmpleadoReportRepository
    {
        private readonly string _cs;

        public SodaEmpleadoReportRepository(string connectionString)
        {
            _cs = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IReadOnlyList<SodaEmpleadoRow>> GetSodaPorEmpleadoAsync(
            long carne,
            DateTime fechaDesde,
            DateTime fechaHasta,
            CancellationToken ct = default)
        {
            // Normalizamos a solo fecha
            fechaDesde = fechaDesde.Date;
            fechaHasta = fechaHasta.Date;

            const string sql = @"
SELECT
    s.Id                                        AS Factura,
    s.Fecha                                     AS Fecha,
    CAST(e.Carne AS bigint)                     AS Carne,
    (e.Primer_Apellido + ' ' + e.Segundo_Apellido + ' ' + e.Nombre)
                                                AS NombreCompleto,
    a.Descripcion                               AS DescripcionArticulo,
    sd.Cantidad                                 AS Cantidad,
    CAST(sd.Precio AS decimal(18,2))            AS PrecioUnitario,
    CAST(sd.Total  AS decimal(18,2))            AS TotalLinea
FROM dbo.Soda s
JOIN dbo.Empleados e
    ON e.Id = s.Id_Empleado
JOIN dbo.Soda_Detalles sd
    ON sd.Id_Soda = s.Id          -- si en tu tabla es Id_Deduccion, cámbialo aquí
JOIN dbo.Articulos a
    ON a.Id = sd.Codigo_Articulo
WHERE
    s.Anulada = 0
    AND e.Carne = @Carne
    AND s.Fecha >= @FechaDesde
    AND s.Fecha <  DATEADD(DAY, 1, @FechaHasta)
ORDER BY
    s.Fecha,
    s.Id,
    a.Descripcion;";

            var list = new List<SodaEmpleadoRow>();

            using var cn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(sql, cn) { CommandType = CommandType.Text };

            cmd.Parameters.Add(new SqlParameter("@Carne", SqlDbType.BigInt) { Value = carne });
            cmd.Parameters.Add(new SqlParameter("@FechaDesde", SqlDbType.Date) { Value = fechaDesde });
            cmd.Parameters.Add(new SqlParameter("@FechaHasta", SqlDbType.Date) { Value = fechaHasta });

            await cn.OpenAsync(ct).ConfigureAwait(false);
            using var rd = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false);

            int iFactura = rd.GetOrdinal("Factura");
            int iFecha = rd.GetOrdinal("Fecha");
            int iCarne = rd.GetOrdinal("Carne");
            int iNombre = rd.GetOrdinal("NombreCompleto");
            int iDesc = rd.GetOrdinal("DescripcionArticulo");
            int iCant = rd.GetOrdinal("Cantidad");
            int iPrecio = rd.GetOrdinal("PrecioUnitario");
            int iTotal = rd.GetOrdinal("TotalLinea");

            while (await rd.ReadAsync(ct).ConfigureAwait(false))
            {
                var row = new SodaEmpleadoRow
                {
                    Factura = rd.GetInt64(iFactura),
                    Fecha = rd.GetDateTime(iFecha),
                    Carne = rd.GetInt64(iCarne),
                    NombreCompleto = rd.GetString(iNombre),
                    DescripcionArticulo = rd.GetString(iDesc),
                    Cantidad = rd.GetInt32(iCant),
                    PrecioUnitario = rd.GetDecimal(iPrecio),
                    TotalLinea = rd.GetDecimal(iTotal)
                };

                list.Add(row);
            }

            return list;
        }
    }
}
