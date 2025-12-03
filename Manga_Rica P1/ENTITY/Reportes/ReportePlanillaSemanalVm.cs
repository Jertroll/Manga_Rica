using System;
using System.Collections.Generic;

namespace Manga_Rica_P1.Entity.Reports
{
    /// <summary>
    /// ViewModel principal para el reporte de Planilla Semanal.
    /// </summary>
    public sealed class ReportePlanillaSemanalVm
    {
        public string Titulo { get; set; } = "Planilla Semanal";
        public string PieDePagina { get; set; } = string.Empty;

        public int Semana { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public List<PlanillaSemanalLineaVm> Lineas { get; } = new();

        /// <summary>Total general de salario bruto.</summary>
        public decimal TotalSalarioBruto { get; set; }

        /// <summary>Total general de deducciones (Soda + Uniforme).</summary>
        public decimal TotalDeducciones { get; set; }

        /// <summary>Total general de salario neto.</summary>
        public decimal TotalSalarioNeto { get; set; }
    }

    /// <summary>
    /// Línea detallada de la planilla semanal (equivalente a una fila en el reporte).
    /// </summary>
    public sealed class PlanillaSemanalLineaVm
    {
        /// <summary>Número de semana.</summary>
        public int Semana { get; set; }

        /// <summary>Fecha de inicio del período.</summary>
        public DateTime FechaInicio { get; set; }

        /// <summary>Fecha de fin del período.</summary>
        public DateTime FechaFin { get; set; }

        /// <summary>Departamento al que pertenece el empleado.</summary>
        public string Departamento { get; set; } = string.Empty;

        /// <summary>Carné / código interno del empleado.</summary>
        public long Carne { get; set; }

        /// <summary>Cédula del empleado.</summary>
        public string Cedula { get; set; } = string.Empty;

        /// <summary>Nombre completo del empleado.</summary>
        public string Empleado { get; set; } = string.Empty;

        /// <summary>Horas normales trabajadas en la semana.</summary>
        public decimal HorasNormales { get; set; }

        /// <summary>Horas extras trabajadas en la semana.</summary>
        public decimal HorasExtras { get; set; }

        /// <summary>Horas dobles trabajadas en la semana.</summary>
        public decimal HorasDobles { get; set; }

        /// <summary>Horas en días feriados trabajadas en la semana.</summary>
        public decimal Feriados { get; set; }

        /// <summary>Salario por hora del empleado (S/H).</summary>
        public decimal SalarioHora { get; set; }

        /// <summary>Total consumido en soda para esa semana.</summary>
        public decimal Soda { get; set; }

        /// <summary>Total de deducciones por uniforme para esa semana.</summary>
        public decimal Uniforme { get; set; }

        /// <summary>Salario bruto semanal.</summary>
        public decimal SalarioBruto { get; set; }

        /// <summary>Salario neto semanal.</summary>
        public decimal SalarioNeto { get; set; }
    }
}
