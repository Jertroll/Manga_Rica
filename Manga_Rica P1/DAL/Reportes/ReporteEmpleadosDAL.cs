using Microsoft.Data.SqlClient;
using System.Data;

namespace Manga_Rica_P1.DAL
{
    public sealed class ReporteEmpleadosDAL
    {
        private readonly string _cs;
        public ReporteEmpleadosDAL(string cs) => _cs = cs;

        public DataTable ConsultarEmpleadosActivos(string puesto = "*")
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
                SELECT e.Id, e.Carne, e.Cedula, e.Primer_Apellido, e.Segundo_Apellido, e.Nombre,
                       e.Fecha_Ingreso, e.Puesto, e.Salario, e.Activo,
                       ISNULL(d.Departamento, 'Sin Departamento') AS Departamento
                FROM dbo.Empleados e
                LEFT JOIN dbo.Departamentos d ON d.Id = e.Id_Departamento
                WHERE e.Activo = 1
                  AND (@Puesto = '*' OR e.Puesto = @Puesto)
                ORDER BY ISNULL(d.Departamento, 'Sin Departamento'), e.Nombre, e.Primer_Apellido, e.Segundo_Apellido;";
            cmd.Parameters.Add(new SqlParameter("@Puesto", puesto));
            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable("Empleados");
            da.Fill(dt);
            return dt;
        }
    }
}
