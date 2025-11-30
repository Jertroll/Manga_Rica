using Manga_Rica_P1.DAL.Reports;
using Manga_Rica_P1.Entity.Reports;
using MangaRica.ENTITY.ViewModels.Reports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MangaRica.BLL
{
    public sealed class ReportesUniformesService
    {
        private readonly IUniformesReportRepository _repo;

        public ReportesUniformesService(IUniformesReportRepository repo)
        {
            _repo = repo;
        }

        public async Task<ReporteUniformesVm> GetUniformesVmAsync(
            CancellationToken ct = default)
        {
            // Para este reporte, la categoría va fija:
            const string categoriaUniforme = "UNIFORMES";

            var rows = await _repo.GetUniformesAsync(categoriaUniforme, ct);

            var vm = new ReporteUniformesVm
            {
                Titulo = "Uniformes General",
                PieDePagina = " "
            };

            // Agrupamos por empleado
            foreach (var empGrp in rows.GroupBy(r => new
            {
                r.Carne,
                r.Apellidos,
                r.Nombre,
                r.Departamento
            }))
            {
                var detalles = empGrp.Select(r => new UniformeDetalleVm
                {
                    IdDeduccion = r.IdDeduccion,
                    Codigo = r.CodigoArticulo,
                    Descripcion = r.DescripcionArticulo,
                    Cantidad = r.Cantidad,
                    Precio = r.PrecioUnitario,
                    Total = r.TotalLinea,
                    SaldoDeduccion = r.SaldoDeduccion
                }).ToList();
                var totalDeduc = detalles.Sum(d => d.Total);

                // OJO: el saldo viene repetido por línea -> agrupar por IdDeduccion
                var saldoPendiente = detalles
                    .GroupBy(d => d.IdDeduccion)
                    .Sum(g => g.First().SaldoDeduccion);

                var empVm = new UniformeEmpleadoVm
                {
                    Carne = empGrp.Key.Carne,
                    Apellidos = empGrp.Key.Apellidos,
                    Nombre = empGrp.Key.Nombre,
                    Departamento = empGrp.Key.Departamento,
                    TotalDeducciones = totalDeduc,     
                    SaldoPendiente = saldoPendiente,
                    Detalles = detalles
                };

                vm.Empleados.Add(empVm);
            }

            return vm;
        }
    }
}
