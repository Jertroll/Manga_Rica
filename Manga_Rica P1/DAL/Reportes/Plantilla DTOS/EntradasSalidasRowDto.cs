using System;

namespace Manga_Rica_P1.DAL.Reports.Dtos
{
    /// <summary>
    /// Fila cruda del reporte de Entradas y Salidas, tal como sale de ClockDb
    /// (Bit2.dbo.calculatedAttendance + Bit2.dbo.employees).
    /// </summary>
    public sealed class EntradasSalidasRowDto
    {
        /// <summary>
        /// Fecha del registro (día al que corresponde la asistencia).
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Carné del empleado (mapeado desde employees.code).
        /// Aunque en ClockDb es NVARCHAR, aquí lo tratamos como numérico.
        /// </summary>
        public string Carne { get; set; } = "";

        /// <summary>
        /// Apellidos del empleado (lastName en ClockDb).
        /// </summary>
        public string Apellidos { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del empleado (name en ClockDb).
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Fecha/hora de entrada (startEnroll).
        /// Puede venir nula si no hubo marca.
        /// </summary>
        public DateTime? HoraEntrada { get; set; }

        /// <summary>
        /// Fecha/hora de salida (endEnroll).
        /// Puede venir nula si no hubo marca.
        /// </summary>
        public DateTime? HoraSalida { get; set; }

        /// <summary>
        /// Total de minutos trabajados en el día (columna total en calculatedAttendance).
        /// </summary>
        public int TotalMinutos { get; set; }
    }
}
