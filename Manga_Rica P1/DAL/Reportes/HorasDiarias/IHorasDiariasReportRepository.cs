using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports.Dtos;

namespace Manga_Rica_P1.DAL.Reports
{
    /// <summary>
    /// Contrato de lectura para el reporte de Horas Diarias.
    /// Devuelve filas "crudas" de la BD (DTO).
    /// </summary>
    public interface IHorasDiariasReportRepository
    {
        /// <summary>
        /// Obtiene las horas diarias (una fila por empleado) para una fecha dada.
        /// </summary>
        /// <param name="fecha">Día a consultar (solo se usa la parte de fecha).</param>
        /// <param name="ct">Token de cancelación.</param>
        Task<IReadOnlyList<HorasDiariasRowDto>> GetHorasDiariasPorFechaAsync(
            DateTime fecha,
            CancellationToken ct = default);
    }
}
