using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports.Dtos;

namespace Manga_Rica_P1.DAL.Reports
{
    /// <summary>
    /// Contrato de lectura de datos crudos para el reporte de Entradas y Salidas
    /// desde ClockDb (Bit2).
    /// </summary>
    public interface IEntradasSalidasReportRepository
    {
        /// <summary>
        /// Obtiene todas las filas de entradas y salidas en el rango [desde, hasta]
        /// (ambos inclusive), directamente desde Bit2.dbo.calculatedAttendance
        /// y Bit2.dbo.employees.
        /// </summary>
        Task<IReadOnlyList<EntradasSalidasRowDto>> GetEntradasSalidasPorRangoAsync(
            DateTime desde,
            DateTime hasta,
            CancellationToken ct = default);

        /// <summary>
        /// Obtiene las filas de entradas y salidas SOLO para un empleado (por carné)
        /// en el rango [desde, hasta] (ambos inclusive).
        /// </summary>
        Task<IReadOnlyList<EntradasSalidasRowDto>> GetEntradasSalidasPorEmpleadoAsync(
            DateTime desde,
            DateTime hasta,
            string carne,
            CancellationToken ct = default);
    }
}
