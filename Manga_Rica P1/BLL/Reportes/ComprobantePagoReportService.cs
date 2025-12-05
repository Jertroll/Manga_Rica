using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports;
using Manga_Rica_P1.Entity.Reports;

namespace Manga_Rica_P1.BLL.Reportes
{
    /// <summary>
    /// Servicio de negocio para construir el ViewModel del reporte
    /// de Comprobantes de Pago.
    /// </summary>
    public sealed class ComprobantePagoReportService
    {
        private readonly IComprobantePagoReportRepository _repository;

        public ComprobantePagoReportService(IComprobantePagoReportRepository repository)
        {
            _repository = repository
                ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Construye el ViewModel completo del reporte para la semana indicada.
        /// </summary>
        /// <param name="semana">Número de semana (Semanas.Semana).</param>
        public async Task<ReporteComprobantesPagoVm> BuildReporteAsync(
            int semana,
            CancellationToken ct = default)
        {
            // 1. Obtener todas las filas desde la BD (DAL)
            var rows = await _repository
                .GetComprobantesPorSemanaAsync(semana, ct)
                .ConfigureAwait(false);

            // 2. Armar el ViewModel de reporte (capa Entity/Reports)
            var vm = new ReporteComprobantesPagoVm
            {
                Semana = semana,
                Titulo = $"Comprobantes de Pago - Semana: {semana}"
            };

            // Si hay filas, usamos la primera para rellenar las fechas del encabezado.
            var firstRow = rows.FirstOrDefault();
            if (firstRow != null)
            {
                vm.FechaInicio = firstRow.FechaInicio;
                vm.FechaFin = firstRow.FechaFin;
            }

            // 3. Mapear cada fila del DTO DAL a la línea del ViewModel
            foreach (var row in rows)
            {
                var linea = new ComprobantePagoLineaVm
                {
                    Semana = row.Semana,
                    Fecha = row.Fecha,
                    Carne = row.Carne,
                    Cedula = row.Cedula,
                    Nombre = row.Nombre,
                    PrimerApellido = row.PrimerApellido,
                    SegundoApellido = row.SegundoApellido,
                    Departamento = row.Departamento,

                    HorasNormales = row.HorasNormales,
                    HorasExtras = row.HorasExtras,
                    HorasDobles = row.HorasDobles,
                    Feriados = row.Feriados,
                    DeduccionSoda = row.DeduccionSoda,
                    DeduccionUniforme = row.DeduccionUniforme,
                    DeduccionOtras = row.DeduccionOtras,
                    SalarioBruto = row.SalarioBruto,
                    SalarioNeto = row.SalarioNeto
                };

                vm.Comprobantes.Add(linea);
            }

            return vm;
        }
    }
}
