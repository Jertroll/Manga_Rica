using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.DAL.Reports.Dtos;

namespace Manga_Rica_P1.DAL.Reports
{
    public interface ISodaGeneralReportRepository
    {
        Task<IReadOnlyList<SodaGeneralRow>> GetSodaGeneralAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            string? categoriaArticulo = null,
            CancellationToken ct = default);
    }

    public sealed class SodaGeneralReportRepository : ISodaGeneralReportRepository
    {
        private readonly string _cs;

        public SodaGeneralReportRepository(string connectionString)
        {
            _cs = connectionString;
        }

        public async Task<IReadOnlyList<SodaGeneralRow>> GetSodaGeneralAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            string? categoriaArticulo = null,
            CancellationToken ct = default)
        {
            // Normalizamos a solo fecha (por si el caller manda DateTime con hora)
            fechaDesde = fechaDesde.Date;
            fechaHasta = fechaHasta.Date;

            const string sql = @"
SELECT
    s.Id                                   AS Factura,
    s.Fecha                                AS Fecha,
    CAST(e.Carne AS varchar(50))           AS Carne,
    (e.Primer_Apellido + ' ' + e.Segundo_Apellido + ' ' + e.Nombre) AS NombreCompleto,
    a.Descripcion                          AS DescripcionArticulo,
    sd.Cantidad                            AS Cantidad,
    CAST(sd.Precio AS decimal(18,2))       AS PrecioUnitario,
    CAST(sd.Total  AS decimal(18,2))       AS TotalLinea
FROM dbo.Soda s
JOIN dbo.Empleados e
    ON e.Id = s.Id_Empleado
JOIN dbo.Soda_Detalles sd
    ON sd.Id_Soda = s.Id                  -- ✅ FK correcta
JOIN dbo.Articulos a
    ON a.Id = sd.Codigo_Articulo
WHERE
    s.Anulada = 0
    AND s.Fecha >= @FechaDesde
    AND s.Fecha < DATEADD(DAY, 1, @FechaHasta)
    AND (@Categoria IS NULL OR a.Categoria = @Categoria)
ORDER BY
    s.Fecha,
    e.Carne,
    a.Descripcion,
    s.Id;";

            var list = new List<SodaGeneralRow>();

            using var cn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(sql, cn) { CommandType = CommandType.Text };

            cmd.Parameters.Add(new SqlParameter("@FechaDesde", SqlDbType.Date) { Value = fechaDesde });
            cmd.Parameters.Add(new SqlParameter("@FechaHasta", SqlDbType.Date) { Value = fechaHasta });

            if (string.IsNullOrWhiteSpace(categoriaArticulo))
            {
                cmd.Parameters.Add(new SqlParameter("@Categoria", SqlDbType.VarChar, 50) { Value = DBNull.Value });
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@Categoria", SqlDbType.VarChar, 50) { Value = categoriaArticulo });
            }

            await cn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);

            // 👇 estos nombres deben coincidir EXACTAMENTE con los alias del SELECT
            int iFactura = rd.GetOrdinal("Factura");
            int iFecha = rd.GetOrdinal("Fecha");
            int iCarne = rd.GetOrdinal("Carne");
            int iNombre = rd.GetOrdinal("NombreCompleto");
            int iDesc = rd.GetOrdinal("DescripcionArticulo");
            int iCant = rd.GetOrdinal("Cantidad");
            int iPrecio = rd.GetOrdinal("PrecioUnitario");
            int iTotal = rd.GetOrdinal("TotalLinea");

            while (await rd.ReadAsync(ct))
            {
                list.Add(new SodaGeneralRow
                {
                    Factura = rd.GetInt64(iFactura),
                    Fecha = rd.GetDateTime(iFecha),
                    Carne = rd.GetString(iCarne),
                    NombreCompleto = rd.GetString(iNombre),
                    DescripcionArticulo = rd.GetString(iDesc),
                    Cantidad = rd.GetInt32(iCant),
                    PrecioUnitario = rd.GetDecimal(iPrecio),
                    TotalLinea = rd.GetDecimal(iTotal)
                });
            }

            return list;
        }
    }
}
