using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports.Dtos;

namespace Manga_Rica_P1.DAL.Reports
{
    public interface IPlanillaSemanalReportRepository
    {
        /// <summary>
        /// Devuelve las filas de planilla para una semana específica (todos los empleados).
        /// </summary>
        Task<IReadOnlyList<PlanillaSemanalRowDto>> GetPlanillaSemanalAsync(
            int semana,
            CancellationToken ct = default);

        /// <summary>
        /// Devuelve las filas de planilla para una semana y un carnet específico.
        /// </summary>
        Task<IReadOnlyList<PlanillaSemanalRowDto>> GetPlanillaSemanalPorEmpleadoAsync(
            int semana,
            long carne,
            CancellationToken ct = default);

        Task<IReadOnlyList<PlanillaSemanalRowDto>> GetPlanillaSemanalPorDepartamentoAsync(
            int semana,
            int idDepartamento,
            CancellationToken ct = default);
    }
}
