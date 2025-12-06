using System;
using System.Collections.Generic;

namespace Manga_Rica_P1.Entity.Reports
{
    /// <summary>
    /// ViewModel principal para el reporte de Horas Diarias.
    /// Representa el encabezado y el conjunto de filas del reporte.
    /// </summary>
    public sealed class ReporteHorasDiariasVm
    {
        /// <summary>Título que se muestra en la parte superior del reporte.</summary>
        public string Titulo { get; set; } = "Horas Laboradas Día";

        /// <summary>Texto del pie de página.</summary>
        public string PieDePagina { get; set; } = string.Empty;

        /// <summary>Fecha para la cual se generó el reporte.</summary>
        public DateTime Fecha { get; set; }

        /// <summary>Listado de registros (una fila por empleado).</summary>
        public List<HorasDiariasLineaVm> Lineas { get; } = new();

        // Totales generales por tipo de hora
        public decimal TotalNormales { get; set; }
        public decimal TotalExtras { get; set; }
        public decimal TotalDobles { get; set; }
        public decimal TotalFeriado { get; set; }
    }

    /// <summary>
    /// Fila del reporte de Horas Diarias (equivalente a una línea en la tabla).
    /// </summary>
    public sealed class HorasDiariasLineaVm
    {
        /// <summary>Carné del empleado.</summary>
        public long Carne { get; set; }

        /// <summary>Apellidos del empleado (Primer + Segundo).</summary>
        public string Apellidos { get; set; } = string.Empty;

        /// <summary>Nombre del empleado.</summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>Horas normales trabajadas en el día.</summary>
        public decimal HorasNormales { get; set; }

        /// <summary>Horas extras trabajadas en el día.</summary>
        public decimal HorasExtras { get; set; }

        /// <summary>Horas dobles trabajadas en el día.</summary>
        public decimal HorasDobles { get; set; }

        /// <summary>Horas en día feriado trabajadas en el día.</summary>
        public decimal HorasFeriado { get; set; }
    }
}
