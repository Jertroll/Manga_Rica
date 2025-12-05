using System;

namespace Manga_Rica_P1.DAL.Reports.Dtos
{
    /// <summary>
    /// DTO de lectura para el reporte de Comprobantes de Pago.
    /// Representa una fila resultante del SELECT en la base de datos.
    /// </summary>
    public sealed class ComprobantePagoRowDto
    {
        public int Semana { get; set; }

        /// <summary>Fecha del pago (Pagos.Fecha).</summary>
        public DateTime Fecha { get; set; }

        /// <summary>Fecha de inicio del período (Semanas.Fecha_Inicio).</summary>
        public DateTime FechaInicio { get; set; }

        /// <summary>Fecha de fin del período (Semanas.Fecha_Final).</summary>
        public DateTime FechaFin { get; set; }

        // --- Datos del empleado ---

        public long Carne { get; set; }
        public string Cedula { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string PrimerApellido { get; set; } = string.Empty;
        public string SegundoApellido { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;

        // --- Horas / conceptos de pago ---

        public decimal HorasNormales { get; set; }
        public decimal HorasExtras { get; set; }
        public decimal HorasDobles { get; set; }
        public decimal Feriados { get; set; }

        public decimal DeduccionSoda { get; set; }
        public decimal DeduccionUniforme { get; set; }
        public decimal DeduccionOtras { get; set; }

        public decimal SalarioBruto { get; set; }
        public decimal SalarioNeto { get; set; }
    }
}
