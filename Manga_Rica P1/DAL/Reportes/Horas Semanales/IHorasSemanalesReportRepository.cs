using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports.Dtos;

namespace Manga_Rica_P1.DAL.Reports
{
    /// <summary>
    /// Contrato de lectura de datos crudos para el reporte de Horas Semanales
    /// desde la BD de planilla (Acumulado_Diario + Empleados).
    /// </summary>
    public interface IHorasSemanalesReportRepository
    {
        /// <summary>
        /// Obtiene las horas acumuladas por empleado en el rango [desde, hasta]
        /// (ambos inclusive).
        /// </summary>
        Task<IReadOnlyList<HorasSemanalesRowDto>> GetHorasSemanalesPorRangoAsync(
            DateTime desde,
            DateTime hasta,
            CancellationToken ct = default);
    }
}
