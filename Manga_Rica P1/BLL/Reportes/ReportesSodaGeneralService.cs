using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports;
using Manga_Rica_P1.Entity.Reports;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Servicio de negocio para el reporte "Detalle de Soda" (general).
    /// </summary>
    public sealed class ReportesSodaGeneralService
    {
        private readonly ISodaGeneralReportRepository _repo;

        public ReportesSodaGeneralService(ISodaGeneralReportRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Construye el ViewModel del reporte de soda para un rango de fechas.
        /// </summary>
        public async Task<ReporteSodaGeneralVm> GetSodaGeneralVmAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            CancellationToken ct = default)
        {
            // Normalizamos a fecha sin hora por consistencia
            fechaDesde = fechaDesde.Date;
            fechaHasta = fechaHasta.Date;

            // Si quieres filtrar por categoría "SODA", úsala aquí.
            const string categoriaSoda = "SODA";

            // 1) Traemos los datos reales desde el repo (DAL)
            var rows = (await _repo.GetSodaGeneralAsync(
                fechaDesde,
                fechaHasta,
                categoriaSoda,
                ct)).ToList();

#if DEBUG
            // 2) DEBUG: duplicar los registros x20 para probar el PDF en muchas páginas
            const int factorDuplicacion = 20;
            var original = rows.ToList(); // copia de la lista original

            for (int i = 0; i < factorDuplicacion - 1; i++)
            {
                rows.AddRange(original);
            }
#endif

            var vm = new ReporteSodaGeneralVm
            {
#if DEBUG
                Titulo = "Detalle de Soda General (TEST x20)",
#else
                Titulo = "Detalle de Soda General",
#endif
                PieDePagina = " ",
                FechaInicio = fechaDesde,
                FechaFin = fechaHasta
            };

            if (rows.Any())
            {
                vm.Lineas = rows.Select(r => new SodaLineaVm
                {
                    Factura = r.Factura,
                    Fecha = r.Fecha,
                    Carne = r.Carne,
                    Nombre = r.NombreCompleto,
                    Descripcion = r.DescripcionArticulo,
                    Cantidad = r.Cantidad,
                    Precio = r.PrecioUnitario,
                    Total = r.TotalLinea
                }).ToList();

                vm.TotalGeneral = vm.Lineas.Sum(l => l.Total);
            }
            else
            {
                vm.Lineas = new();
                vm.TotalGeneral = 0m;
            }

            return vm;
        }
    }
}
