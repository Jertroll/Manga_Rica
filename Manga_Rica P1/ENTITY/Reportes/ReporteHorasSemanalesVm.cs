using System;
using System.Collections.Generic;

namespace Manga_Rica_P1.Entity.Reports
{
    /// <summary>
    /// ViewModel principal para el reporte de Horas Semanales.
    /// Representa el encabezado y el conjunto de filas del reporte.
    /// </summary>
    public sealed class ReporteHorasSemanalesVm
    {
        /// <summary>Título que se muestra en la parte superior del reporte.</summary>
        public string Titulo { get; set; } = "Horas Laboradas";

        /// <summary>Texto del pie de página.</summary>
        public string PieDePagina { get; set; } = string.Empty;

        /// <summary>Fecha inicial del rango utilizado para el reporte.</summary>
        public DateTime Desde { get; set; }

        /// <summary>Fecha final del rango utilizado para el reporte.</summary>
        public DateTime Hasta { get; set; }

        /// <summary>Listado de registros (una fila por empleado).</summary>
        public List<HorasSemanalesLineaVm> Lineas { get; } = new();

        // Totales generales por tipo de hora en el rango
        public decimal TotalNormales { get; set; }
        public decimal TotalExtras { get; set; }
        public decimal TotalDobles { get; set; }
        public decimal TotalFeriado { get; set; }
    }

    /// <summary>
    /// Fila del reporte de Horas Semanales (equivalente a una línea en la tabla).
    /// </summary>
    public sealed class HorasSemanalesLineaVm
    {
        /// <summary>Carné del empleado.</summary>
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

        /// <summary>Total de horas en días feriados en el rango semanal.</summary>
        public decimal HorasFeriado { get; set; }
    }
}
