using System;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports;
using Manga_Rica_P1.Entity.Reports;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Servicio de dominio para armar el ViewModel del reporte de Horas Diarias
    /// a partir de los datos crudos del repositorio.
    /// </summary>
    public sealed class ReportesHorasDiariasService
    {
        private readonly IHorasDiariasReportRepository _repo;

        public ReportesHorasDiariasService(IHorasDiariasReportRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        /// <summary>
        /// Genera el ViewModel para el reporte de Horas Diarias.
        /// </summary>
        /// <param name="fecha">Fecha del día a consultar.</param>
        /// <param name="ct">Token de cancelación.</param>
        public async Task<ReporteHorasDiariasVm> GenerarReporteAsync(
            DateTime fecha,
            CancellationToken ct = default)
        {
            var rows = await _repo.GetHorasDiariasPorFechaAsync(fecha, ct);

            var vm = new ReporteHorasDiariasVm
            {
                Fecha = fecha.Date,
                Titulo = "Horas Laboradas Día"
            };

            foreach (var r in rows)
            {
                var linea = new HorasDiariasLineaVm
                {
                    Carne = r.Carne,
                    Apellidos = $"{r.PrimerApellido} {r.SegundoApellido}".Trim(),
                    Nombre = r.Nombre,
                    // Convertimos float (BD) a decimal (reporte) para sumar/mostrar con mejor precisión
                    HorasNormales = (decimal)r.Normales,
                    HorasExtras = (decimal)r.Extras,
                    HorasDobles = (decimal)r.Dobles,
                    HorasFeriado = (decimal)r.Feriado
                };

                vm.Lineas.Add(linea);

                // Ir acumulando totales generales
                vm.TotalNormales += linea.HorasNormales;
                vm.TotalExtras += linea.HorasExtras;
                vm.TotalDobles += linea.HorasDobles;
                vm.TotalFeriado += linea.HorasFeriado;
            }

            return vm;
        }

        public async Task<ReporteHorasDiariasVm> GenerarReportePorEmpleadoAsync(
    DateTime fecha,
    long carne,
    CancellationToken ct = default)
        {
            // 1) Reusamos el método general para no duplicar la lógica de mapeo
            var vmGeneral = await GenerarReporteAsync(fecha, ct);

            // 2) Filtramos por carné
            var filtradas = vmGeneral.Lineas
                .FindAll(l => l.Carne == carne);

            // 3) Armamos un nuevo VM solo con esas filas y totales recalculados
            var vm = new ReporteHorasDiariasVm
            {
                Fecha = vmGeneral.Fecha,
                Titulo = $"Horas Laboradas Día - Carné {carne}",
                PieDePagina = vmGeneral.PieDePagina
            };

            foreach (var l in filtradas)
            {
                vm.Lineas.Add(l);

                vm.TotalNormales += l.HorasNormales;
                vm.TotalExtras += l.HorasExtras;
                vm.TotalDobles += l.HorasDobles;
                vm.TotalFeriado += l.HorasFeriado;
            }

            return vm;
        }

    }
}
