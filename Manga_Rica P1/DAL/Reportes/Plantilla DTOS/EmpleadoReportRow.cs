// Nueva implementacion
using System;

namespace Manga_Rica_P1.DAL.Reports.Dtos
{
    public sealed class EmpleadoReportRow
    {
        public string Departamento { get; set; } = "";
        public string Carne { get; set; } = "";
        public string Apellido1 { get; set; } = "";
        public string Apellido2 { get; set; } = "";
        public string Nombre { get; set; } = "";
        public decimal Salario { get; set; }
        public string Puesto { get; set; } = "";
        public DateTime FechaIngreso { get; set; }
    }
}
