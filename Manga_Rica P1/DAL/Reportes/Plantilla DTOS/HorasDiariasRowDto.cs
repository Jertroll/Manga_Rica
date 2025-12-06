using System;

namespace Manga_Rica_P1.DAL.Reports.Dtos
{
    /// <summary>
    /// DTO de lectura para el reporte de Horas Diarias.
    /// Refleja los tipos reales de la BD y se usa en el repository.
    /// </summary>
    public sealed class HorasDiariasRowDto
    {
        // Desde Acumulado_Diario
        public long IdEmpleado { get; set; }   // Id_Empleado (BIGINT)
        public DateTime Fecha { get; set; }   // Fecha (datetime)

        public float Normales { get; set; }   // Normales (float)
        public float Extras { get; set; }   // Extras   (float)
        public float Dobles { get; set; }   // Dobles   (float)
        public float Feriado { get; set; }   // Feriado  (float)

        // Desde Empleados
        public long Carne { get; set; }   // Carne (BIGINT)
        public string PrimerApellido { get; set; } = string.Empty;
        public string SegundoApellido { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
    }
}
