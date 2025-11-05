
using System;
using System.Collections.Generic;

namespace MangaRica.ENTITY.ViewModels.Reports
{
    // Nueva implementacion: ViewModel raíz para la plantilla
    public sealed class ReporteEmpleadosActivosVm
    {
        public string Titulo { get; set; } = "Empleados Activos por Departamento";
        public string PieDePagina { get; set; } = "Manga Rica S.A. • Reporte demo";
        public List<DepartamentoGrupoVm> Departamentos { get; set; } = new();
    }

    // Nueva implementacion: Grupo por departamento
    public sealed class DepartamentoGrupoVm
    {
        public string Nombre { get; set; } = "";
        public List<EmpleadoItemVm> Empleados { get; set; } = new();
        public int TotalEmpleados => Empleados?.Count ?? 0;
    }

    // Nueva implementacion: Fila de empleado para el reporte
    public sealed class EmpleadoItemVm
    {
        public string Carne { get; set; } = "";
        public string Apellido1 { get; set; } = "";
        public string Apellido2 { get; set; } = "";
        public string Nombre { get; set; } = "";
        public decimal Salario { get; set; }
        public string Puesto { get; set; } = "";
        public DateTime FechaIngreso { get; set; }
    }
}
