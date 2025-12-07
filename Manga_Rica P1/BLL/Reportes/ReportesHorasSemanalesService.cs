using System;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports;
using Manga_Rica_P1.Entity.Reports;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Lógica de negocio para construir el ViewModel del reporte de Horas Semanales.
    /// </summary>
    public sealed class ReportesHorasSemanalesService
    {
        private readonly IHorasSemanalesReportRepository _repo;

        public ReportesHorasSemanalesService(IHorasSemanalesReportRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        /// <summary>
        /// Genera el ViewModel del reporte de Horas Semanales para el rango [desde, hasta].
        /// </summary>
        public async Task<ReporteHorasSemanalesVm> GenerarReporteAsync(
            DateTime desde,
            DateTime hasta,
            CancellationToken ct = default)
        {
            var d1 = desde.Date;
            var d2 = hasta.Date;

            // Normalizamos por si el usuario invierte el rango
            if (d2 < d1)
            {
                var tmp = d1;
                d1 = d2;
                d2 = tmp;
            }

            var rows = await _repo.GetHorasSemanalesPorRangoAsync(d1, d2, ct);

            var vm = new ReporteHorasSemanalesVm
            {
                Desde = d1,
                Hasta = d2,
                Titulo = "Horas Laboradas",
                PieDePagina = $"Rango del {d1:dd/MM/yyyy} al {d2:dd/MM/yyyy}"
            };

            foreach (var r in rows)
            {
                
                var normales = r.HorasNormales;
                var extras = r.HorasExtras;
                var dobles = r.HorasDobles;
                var feriado = r.HorasFeriado;

                vm.Lineas.Add(new HorasSemanalesLineaVm
                {
                    Carne = r.Carne,
                    Apellidos = r.Apellidos,
                    Nombre = r.Nombre,
                    HorasNormales = normales,
                    HorasExtras = extras,
                    HorasDobles = dobles,
                    HorasFeriado = feriado
                });

                vm.TotalNormales += normales;
                vm.TotalExtras += extras;
                vm.TotalDobles += dobles;
                vm.TotalFeriado += feriado;
            }

            return vm;
        }
    }
}
