using System;
using System.Collections.Generic;

namespace Manga_Rica_P1.Entity.Reports
{
    /// <summary>
    /// Línea del reporte de Entradas y Salidas (lo que se muestra en la tabla).
    /// </summary>
    public sealed class EntradasSalidasLineaVm
    {
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Carné del empleado.
        /// </summary>
        public string Carne { get; set; } = "";

        public string Apellidos { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Hora de entrada (solo la parte de hora se usará en el render, aunque venga como DateTime).
        /// </summary>
        public DateTime? HoraEntrada { get; set; }

        /// <summary>
        /// Hora de salida (solo la parte de hora se usará en el render, aunque venga como DateTime).
        /// </summary>
        public DateTime? HoraSalida { get; set; }

        /// <summary>
        /// Total de horas trabajadas en el día (TotalMinutos / 60, convertido a decimal).
        /// </summary>
        public decimal TotalHoras { get; set; }
    }

    /// <summary>
    /// ViewModel completo para el reporte de Entradas y Salidas.
    /// </summary>
    public sealed class ReporteEntradasSalidasVm
    {
        /// <summary>
        /// Título que verá el usuario/encabezado del reporte.
        /// </summary>
        public string Titulo { get; set; } = "Entradas y Salidas";

        /// <summary>
        /// Pie de página que usará el layout base (por ejemplo, rango de fechas).
        /// </summary>
        public string PieDePagina { get; set; } = string.Empty;

        /// <summary>
        /// Fecha inicial del rango consultado.
        /// </summary>
        public DateTime Desde { get; set; }

        /// <summary>
        /// Fecha final del rango consultado.
        /// </summary>
        public DateTime Hasta { get; set; }

        /// <summary>
        /// Filas que se mostrarán en la tabla del reporte.
        /// </summary>
        public IList<EntradasSalidasLineaVm> Lineas { get; } =
            new List<EntradasSalidasLineaVm>();
    }
}
