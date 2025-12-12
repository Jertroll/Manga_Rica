using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Manga_Rica_P1.DAL.Reports;
using Manga_Rica_P1.DAL.Reports.Dtos;
using Manga_Rica_P1.Entity.Reports;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Servicio de negocio para el reporte "Planilla Semanal".
    /// Orquesta la lectura desde el repositorio y arma el ViewModel
    /// que consumirá las plantillas Razor.
    /// </summary>
    public sealed class ReportesPlanillaSemanalService
    {
        private readonly IPlanillaSemanalReportRepository _repo;

        public ReportesPlanillaSemanalService(IPlanillaSemanalReportRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        /// <summary>
        /// Reporte general de planilla por semana (todas las personas).
        /// </summary>
        public async Task<ReportePlanillaSemanalVm> GenerarReporteAsync(
            int semana,
            CancellationToken ct = default)
        {
            var rows = await _repo
                .GetPlanillaSemanalAsync(semana, ct)
                .ConfigureAwait(false);

            return ConstruirVmComun(
                rows,
                titulo: $"Planilla Semanal - Semana {semana}",
                pieDePagina: string.Empty);
        }

        /// <summary>
        /// Reporte de planilla por semana filtrado por carnet (un solo empleado).
        /// </summary>
        public async Task<ReportePlanillaSemanalVm> GenerarReportePorSemanaYEmpleadoAsync(
            int semana,
            string carnetTexto,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(carnetTexto))
                throw new ArgumentException("El carnet no puede estar vacío.", nameof(carnetTexto));

            if (!long.TryParse(carnetTexto.Trim(), out var carne))
                throw new ArgumentException("El carnet debe ser numérico.", nameof(carnetTexto));

            var rows = await _repo
                .GetPlanillaSemanalPorEmpleadoAsync(semana, carne, ct)
                .ConfigureAwait(false);

            return ConstruirVmComun(
                rows,
                titulo: $"Planilla Semanal - Semana {semana} - Carnet {carnetTexto.Trim()}",
                pieDePagina: string.Empty);
        }

        public async Task<ReportePlanillaSemanalVm> GenerarReportePorSemanaYDepartamentoAsync(
            int semana,
            int idDepartamento,
            string nombreDepartamento,
            CancellationToken ct = default)
        {
            var rows = await _repo
                .GetPlanillaSemanalPorDepartamentoAsync(semana, idDepartamento, ct)
                .ConfigureAwait(false);

            var titulo = $"Planilla Semanal - Semana {semana} - Departamento {nombreDepartamento}";

            return ConstruirVmComun(
                rows,
                titulo: titulo,
                pieDePagina: string.Empty);
        }

        /// <summary>
        /// Arma el ViewModel a partir de las filas devueltas por el repositorio.
        /// Lo usamos tanto para el reporte general como para el filtrado por empleado.
        /// </summary>
        private static ReportePlanillaSemanalVm ConstruirVmComun(
            IReadOnlyList<PlanillaSemanalRowDto> rows,
            string titulo,
            string pieDePagina)
        {
            var vm = new ReportePlanillaSemanalVm
            {
                Titulo = titulo,
                PieDePagina = pieDePagina
            };

            if (rows != null && rows.Count > 0)
            {
                var first = rows[0];
                vm.Semana = first.Semana;
                vm.FechaInicio = first.FechaInicio.Date;
                vm.FechaFin = first.FechaFin.Date;
            }

            if (rows != null)
            {
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
            }

            vm.TotalSalarioBruto = vm.Lineas.Sum(l => l.SalarioBruto);
            vm.TotalDeducciones = vm.Lineas.Sum(l => l.Soda + l.Uniforme);
            vm.TotalSalarioNeto = vm.Lineas.Sum(l => l.SalarioNeto);

            return vm;
        }
    }
}
