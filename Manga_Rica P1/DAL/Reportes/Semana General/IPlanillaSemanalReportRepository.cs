using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports.Dtos;

namespace Manga_Rica_P1.DAL.Reports
{
    public interface IPlanillaSemanalReportRepository
    {
        /// <summary>
        /// Devuelve las filas de planilla para una semana específica.
        /// </summary>
        Task<IReadOnlyList<PlanillaSemanalRowDto>> GetPlanillaSemanalAsync(
            int semana,
            CancellationToken ct = default);
    }
}
