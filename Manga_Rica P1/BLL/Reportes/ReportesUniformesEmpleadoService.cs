using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Manga_Rica_P1.DAL.Reports;
using Manga_Rica_P1.Entity.Reports;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Servicio de negocio para el reporte "Uniformes por Empleado".
    /// Se encarga de transformar los datos planos del repositorio
    /// al ViewModel que usará la vista Razor.
    /// </summary>
    public sealed class ReportesUniformesEmpleadoService
    {
        private readonly IUniformesEmpleadoReportRepository _repo;

        public ReportesUniformesEmpleadoService(IUniformesEmpleadoReportRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Construye el VM del reporte para un carné específico.
        /// </summary>
        public async Task<ReporteUniformesEmpleadoVm> GetUniformesPorEmpleadoVmAsync(
            long carne,
            CancellationToken ct = default)
        {
            const string categoriaUniformes = "UNIFORMES"; // valor real en la BD

            var rows = await _repo.GetUniformesPorEmpleadoAsync(carne, categoriaUniformes, ct);

            var vm = new ReporteUniformesEmpleadoVm
            {
                Titulo = "Detalle de Uniformes",
                PieDePagina = " "
            };

            if (rows.Any())
            {
                var first = rows.First();

                vm.Carne = first.Carne; // ya viene en string por el CAST
                vm.NombreCompleto = $"{first.Apellidos} {first.Nombre}";
                vm.Departamento = first.Departamento;

                vm.Lineas = rows.Select(r => new UniformeEmpleadoLineaVm
                {
                    Fecha = r.Fecha,
                    IdDeduccion = r.IdDeduccion,
                    CodigoArticulo = r.CodigoArticulo,
                    DescripcionArticulo = r.DescripcionArticulo,
                    Cantidad = r.Cantidad,
                    PrecioUnitario = r.PrecioUnitario,
                    TotalLinea = r.TotalLinea
                }).ToList();

                vm.TotalGeneral = vm.Lineas.Sum(l => l.TotalLinea);
            }
            else
            {
               
                vm.Carne = carne.ToString();
                vm.NombreCompleto = "";
                vm.Departamento = "";
                vm.Lineas = new();
                vm.TotalGeneral = 0m;
            }

            return vm;
        }
    }
}
