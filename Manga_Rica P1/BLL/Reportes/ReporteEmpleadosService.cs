
using System.Data;
using Manga_Rica_P1.DAL;

namespace Manga_Rica_P1.BLL
{
    public sealed class ReporteEmpleadosService
    {
        private readonly ReporteEmpleadosDAL _dal;
        public ReporteEmpleadosService(ReporteEmpleadosDAL dal) => _dal = dal;

        public DataTable EmpleadosActivos(string puesto = "*")
            => _dal.ConsultarEmpleadosActivos(puesto);
    }
}
