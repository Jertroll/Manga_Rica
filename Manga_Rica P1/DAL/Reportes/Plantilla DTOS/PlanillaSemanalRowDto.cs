using System;

namespace Manga_Rica_P1.DAL.Reports.Dtos
{
    /// <summary>
    /// DTO plano con los datos que devuelve el SQL para la planilla semanal.
    /// Debe reflejar 1:1 los alias de la consulta SQL.
    /// </summary>
    public sealed class PlanillaSemanalRowDto
    {
        public int Semana { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public string Departamento { get; set; } = string.Empty;

        public long Carne { get; set; }
        public string Cedula { get; set; } = string.Empty;

        /// <summary>Nombre completo del empleado (Empleado).</summary>
        public string Empleado { get; set; } = string.Empty;

        public decimal HorasNormales { get; set; }
        public decimal HorasExtras { get; set; }
        public decimal HorasDobles { get; set; }
        public decimal Feriados { get; set; }

        /// <summary>Salario por hora (S/H).</summary>
        public decimal SalarioHora { get; set; }

        /// <summary>Total consumido en soda en la semana.</summary>
        public decimal Soda { get; set; }

        /// <summary>Total de deducciones por uniforme en la semana.</summary>
        public decimal Uniforme { get; set; }

        public decimal SalarioBruto { get; set; }
        public decimal SalarioNeto { get; set; }
    }
}
