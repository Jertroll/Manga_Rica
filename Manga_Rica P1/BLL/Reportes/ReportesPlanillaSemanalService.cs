using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports;
using Manga_Rica_P1.Entity.Reports;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Servicio de negocio para el reporte "Planilla Semanal".
    /// Orquesta la lectura desde el repositorio y arma el ViewModel
    /// que consumirá la plantilla Razor.
    /// </summary>
    public sealed class ReportesPlanillaSemanalService
    {
        private readonly IPlanillaSemanalReportRepository _repo;

        public ReportesPlanillaSemanalService(IPlanillaSemanalReportRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        /// <summary>
        /// Construye el ViewModel del reporte para una semana específica.
        /// </summary>
        public async Task<ReportePlanillaSemanalVm> GenerarReporteAsync(
            int semana,
            CancellationToken ct = default)
        {
            var rows = await _repo
                .GetPlanillaSemanalAsync(semana, ct)
                .ConfigureAwait(false);

            var vm = new ReportePlanillaSemanalVm
            {
                Semana = semana,
                Titulo = $"Planilla Semanal - Semana {semana}",
                PieDePagina = ""
            };

            if (rows.Any())
            {
                var first = rows.First();
                vm.FechaInicio = first.FechaInicio.Date;
                vm.FechaFin = first.FechaFin.Date;
            }

            foreach (var r in rows)
            {
                var linea = new PlanillaSemanalLineaVm
                {
                    Semana = r.Semana,
                    FechaInicio = r.FechaInicio,
                    FechaFin = r.FechaFin,
                    Departamento = r.Departamento,

                    Carne = r.Carne,
                    Cedula = r.Cedula,
                    Empleado = r.Empleado,

                    HorasNormales = r.HorasNormales,
                    HorasExtras = r.HorasExtras,
                    HorasDobles = r.HorasDobles,
                    Feriados = r.Feriados,

                    SalarioHora = r.SalarioHora,
                    Soda = r.Soda,
                    Uniforme = r.Uniforme,

                    SalarioBruto = r.SalarioBruto,
                    SalarioNeto = r.SalarioNeto
                };

                vm.Lineas.Add(linea);
            }

            // Totales globales
            vm.TotalSalarioBruto = vm.Lineas.Sum(l => l.SalarioBruto);
            vm.TotalDeducciones = vm.Lineas.Sum(l => l.Soda + l.Uniforme);
            vm.TotalSalarioNeto = vm.Lineas.Sum(l => l.SalarioNeto);

            return vm;
        }
    }
}
