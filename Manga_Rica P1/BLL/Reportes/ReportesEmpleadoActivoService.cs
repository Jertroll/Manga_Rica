// Nueva implementacion
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports;             // Repo interface
using Manga_Rica_P1.DAL.Reports.Dtos;       // EmpleadoReportRow
using MangaRica.ENTITY.ViewModels.Reports;  // ViewModels del reporte

namespace MangaRica.BLL
{
    /// <summary>
    /// Nueva implementacion: Servicio BLL para construir el ViewModel del reporte de empleados activos.
    /// </summary>
    public sealed class ReportesEmpleadoActivoService
    {
        private readonly IEmpleadosReportRepository _repo;

        // Nueva implementacion: inyecta la interfaz (mejor para pruebas/DI)
        public ReportesEmpleadoActivoService(IEmpleadosReportRepository repo)
        {
            _repo = repo;
        }

        // Nueva implementacion: consulta DAL y proyecta a ViewModel de Reporte
        public async Task<ReporteEmpleadosActivosVm> GetEmpleadosActivosVmAsync(CancellationToken ct = default)
        {
            var rows = await _repo.GetEmpleadosActivosAsync(ct);

            var vm = new ReporteEmpleadosActivosVm
            {
                Titulo = "Empleados Activos por Departamento",
                PieDePagina = "--Manga Rica S.A.--"
            };

            foreach (var grp in rows.GroupBy(r => r.Departamento))
            {
                var depVm = new DepartamentoGrupoVm { Nombre = grp.Key };
                depVm.Empleados = grp.Select(r => new EmpleadoItemVm
                {
                    Carne = r.Carne,
                    Apellido1 = r.Apellido1,
                    Apellido2 = r.Apellido2,
                    Nombre = r.Nombre,
                    Salario = r.Salario,
                    Puesto = r.Puesto,
                    FechaIngreso = r.FechaIngreso
                }).ToList();

                vm.Departamentos.Add(depVm);
            }

            return vm;
        }
    }
}
