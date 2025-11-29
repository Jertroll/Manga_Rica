using System;
using System.Collections.Generic;

namespace MangaRica.ENTITY.ViewModels.Reports
{
    // Base reutilizable para ambos reportes (Activos e Inactivos)
    public class ReporteEmpleadosVmBase
    {
        public string Titulo { get; set; } = "";
        public string PieDePagina { get; set; } = "";
        public List<DepartamentoGrupoVm> Departamentos { get; set; } = new();
    }

    // Reporte de Empleados Activos
    public sealed class ReporteEmpleadosListaVm : ReporteEmpleadosVmBase
    {
        // Si quieres, puedes poner Titulo por defecto específico:
        // public ReporteEmpleadosActivosVm() => Titulo = "Empleados Activos por Departamento";
    }

    // Reporte de Empleados Inactivos 
    public sealed class ReporteEmpleadosInactivosVm : ReporteEmpleadosVmBase
    {
        // public ReporteEmpleadosInactivosVm() => Titulo = "Empleados No Activos por Departamento";
    }

   
    public sealed class DepartamentoGrupoVm
    {
        public string Nombre { get; set; } = "";
        public List<EmpleadoItemVm> Empleados { get; set; } = new();
        public int TotalEmpleados => Empleados?.Count ?? 0;
    }

    
    public sealed class EmpleadoItemVm
    {
        public string Carne { get; set; } = "";
        public string Apellido1 { get; set; } = "";
        public string Apellido2 { get; set; } = "";
        public string Nombre { get; set; } = "";
        public decimal Salario { get; set; }
        public string Puesto { get; set; } = "";
        public DateTime? FechaIngreso { get; set; }   
    }
}
