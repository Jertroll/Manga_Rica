using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports.Dtos;

namespace Manga_Rica_P1.DAL.Reports
{
    /// <summary>
    /// Contrato de acceso a datos para el reporte de Comprobantes de Pago.
    /// </summary>
    public interface IComprobantePagoReportRepository
    {
        /// <summary>
        /// Obtiene todas las filas de comprobantes de pago para una semana dada.
        /// El parámetro <paramref name="semana"/> corresponde a Semanas.Semana.
        /// </summary>
        Task<IReadOnlyList<ComprobantePagoRowDto>> GetComprobantesPorSemanaAsync(
            int semana,
            CancellationToken ct = default);
    }
}
