using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports;
using Manga_Rica_P1.DAL.Reports.Dtos;
using Manga_Rica_P1.Entity.Reports;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Servicio de negocio para el reporte ""Soda por empleado"".
    /// Transforma las filas del repositorio en el ViewModel para Razor.
    /// </summary>
    public sealed class ReportesSodaEmpleadoService
    {
        private readonly ISodaEmpleadoReportRepository _repo;

        public ReportesSodaEmpleadoService(ISodaEmpleadoReportRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        // Nombre "oficial" que usaremos desde la UI
        public Task<ReporteSodaEmpleadoVm> GetSodaEmpleadoVmAsync(
            long carne,
            DateTime fechaDesde,
            DateTime fechaHasta,
            CancellationToken ct = default)
        {
            return GenerarReporteInternoAsync(carne, fechaDesde, fechaHasta, ct);
        }

        // Alias para que también compile si en algún lugar llamas GenerarReporteAsync
        public Task<ReporteSodaEmpleadoVm> GenerarReporteAsync(
            long carne,
            DateTime fechaDesde,
            DateTime fechaHasta,
            CancellationToken ct = default)
        {
            return GenerarReporteInternoAsync(carne, fechaDesde, fechaHasta, ct);
        }

        private async Task<ReporteSodaEmpleadoVm> GenerarReporteInternoAsync(
            long carne,
            DateTime fechaDesde,
            DateTime fechaHasta,
            CancellationToken ct)
        {
            IReadOnlyList<SodaEmpleadoRow> rows =
                await _repo.GetSodaPorEmpleadoAsync(carne, fechaDesde, fechaHasta, ct)
                           .ConfigureAwait(false);

            var vm = new ReporteSodaEmpleadoVm
            {
                Titulo = "Detalle de Soda por Empleado",
                PieDePagina = "Reporte generado por el sistema Manga Rica",
                FechaInicio = fechaDesde.Date,
                FechaFin = fechaHasta.Date,
                Lineas = new List<SodaEmpleadoLineaVm>(),
                TotalGeneral = 0m
            };

            if (rows.Count == 0)
            {
                // No hay datos: rellenamos encabezado mínimo
                vm.Carne = carne.ToString();
                vm.Nombre = string.Empty;
                return vm;
            }

            // Encabezado desde la primera fila
            var first = rows[0];
            vm.Carne = first.Carne.ToString();
            vm.Nombre = first.NombreCompleto;

            // Detalle
            vm.Lineas = rows.Select(r => new SodaEmpleadoLineaVm
            {
                Factura = r.Factura,
                Fecha = r.Fecha,
                Descripcion = r.DescripcionArticulo,
                Cantidad = r.Cantidad,
                Precio = r.PrecioUnitario,
                Total = r.TotalLinea
            }).ToList();

            vm.TotalGeneral = vm.Lineas.Sum(l => l.Total);

            return vm;
        }
    }
}
