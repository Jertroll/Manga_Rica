using System;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports;
using Manga_Rica_P1.Entity.Reports;

namespace Manga_Rica_P1.BLL
{
    public sealed class ReportesEntradasSalidasService
    {
        private readonly IEntradasSalidasReportRepository _repo;

        public ReportesEntradasSalidasService(IEntradasSalidasReportRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        // ===== Reporte GENERAL (todas las personas) =====
        public async Task<ReporteEntradasSalidasVm> GenerarReporteAsync(
            DateTime desde,
            DateTime hasta,
            CancellationToken ct = default)
        {
            var d1 = desde.Date;
            var d2 = hasta.Date;

            if (d2 < d1)
            {
                var tmp = d1;
                d1 = d2;
                d2 = tmp;
            }

            var rows = await _repo.GetEntradasSalidasPorRangoAsync(d1, d2, ct);

            var vm = new ReporteEntradasSalidasVm
            {
                Desde = d1,
                Hasta = d2,
                Titulo = "Entradas y Salidas",
                PieDePagina = $"Rango del {d1:dd/MM/yyyy} al {d2:dd/MM/yyyy}"
            };

            foreach (var r in rows)
            {
                // TotalMinutos viene como INT desde el repo; se convierte a horas (decimal)
                var totalHoras = Math.Round((decimal)r.TotalMinutos / 60m, 2);

                vm.Lineas.Add(new EntradasSalidasLineaVm
                {
                    Fecha = r.Fecha.Date,
                    Carne = r.Carne,      // string (code del reloj)
                    Apellidos = r.Apellidos,
                    Nombre = r.Nombre,
                    HoraEntrada = r.HoraEntrada,
                    HoraSalida = r.HoraSalida,
                    TotalHoras = totalHoras
                });
            }

            return vm;
        }

        // ===== Reporte POR EMPLEADO (filtrado por carné string) =====
        public async Task<ReporteEntradasSalidasVm> GenerarReportePorEmpleadoAsync(
            DateTime desde,
            DateTime hasta,
            string carne,
            CancellationToken ct = default)
        {
            var d1 = desde.Date;
            var d2 = hasta.Date;

            if (d2 < d1)
            {
                var tmp = d1;
                d1 = d2;
                d2 = tmp;
            }

            var rows = await _repo.GetEntradasSalidasPorEmpleadoAsync(d1, d2, carne, ct);

            var vm = new ReporteEntradasSalidasVm
            {
                Desde = d1,
                Hasta = d2,
                Titulo = $"Entradas y Salidas - Carnet {carne}",
                PieDePagina = $"Carnet {carne} · Rango del {d1:dd/MM/yyyy} al {d2:dd/MM/yyyy}"
            };

            foreach (var r in rows)
            {
                var totalHoras = Math.Round((decimal)r.TotalMinutos / 60m, 2);

                vm.Lineas.Add(new EntradasSalidasLineaVm
                {
                    Fecha = r.Fecha.Date,
                    Carne = r.Carne,
                    Apellidos = r.Apellidos,
                    Nombre = r.Nombre,
                    HoraEntrada = r.HoraEntrada,
                    HoraSalida = r.HoraSalida,
                    TotalHoras = totalHoras
                });
            }

            return vm;
        }
    }
}
