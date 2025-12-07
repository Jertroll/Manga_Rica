using System;

namespace Manga_Rica_P1.DAL.Reports.Dtos
{
    /// <summary>
    /// Fila cruda devuelta desde la base de datos para el reporte
    /// de Horas Semanales. Representa las horas acumuladas por empleado
    /// en un rango de fechas.
    /// </summary>
    public sealed class HorasSemanalesRowDto
    {
        /// <summary>Carné del empleado (Empleados.Carne).</summary>
        public long Carne { get; set; }

        /// <summary>Apellidos del empleado (Primer + Segundo).</summary>
        public string Apellidos { get; set; } = string.Empty;

        /// <summary>Nombre del empleado.</summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>Total de horas normales en el rango semanal.</summary>
        public decimal HorasNormales { get; set; }

        /// <summary>Total de horas extras en el rango semanal.</summary>
        public decimal HorasExtras { get; set; }

        /// <summary>Total de horas dobles en el rango semanal.</summary>
        public decimal HorasDobles { get; set; }

        /// <summary>Total de horas en feriado en el rango semanal.</summary>
        public decimal HorasFeriado { get; set; }
    }
}
