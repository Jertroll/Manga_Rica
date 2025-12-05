using System;
using System.Collections.Generic;

namespace Manga_Rica_P1.Entity.Reports
{
    /// <summary>
    /// ViewModel principal para el reporte de Comprobantes de Pago.
    /// Representa el conjunto de comprobantes emitidos para una semana específica.
    /// </summary>
    public sealed class ReporteComprobantesPagoVm
    {
        /// <summary>
        /// Título que se mostrará en el encabezado del reporte.
        /// </summary>
        public string Titulo { get; set; } = "Comprobantes de Pago";

        /// <summary>
        /// Texto opcional para mostrar en el pie de página.
        /// </summary>
        public string PieDePagina { get; set; } = string.Empty;

        /// <summary>
        /// Número de semana a la que pertenecen los comprobantes.
        /// (Corresponde a Pagos.Id_Semana).
        /// </summary>
        public int Semana { get; set; }

        /// <summary>
        /// Fecha de inicio de la semana (si aplica).
        /// Usualmente proviene de la tabla de semanas.
        /// </summary>
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Fecha de fin de la semana (si aplica).
        /// </summary>
        public DateTime FechaFin { get; set; }

        /// <summary>
        /// Listado de comprobantes individuales que se mostrarán en el reporte.
        /// </summary>
        public List<ComprobantePagoLineaVm> Comprobantes { get; } = new();
    }

    /// <summary>
    /// Representa un comprobante de pago individual (un empleado en una semana).
    /// Se alimenta a partir de las tablas Pagos, Empleados y Departamentos.
    /// </summary>
    public sealed class ComprobantePagoLineaVm
    {
        /// <summary>
        /// Número de semana al que corresponde este comprobante.
        /// </summary>
        public int Semana { get; set; }

        /// <summary>
        /// Fecha del registro de pago (campo Pagos.Fecha).
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Carné / código interno del empleado (campo Empleados.Carne).
        /// </summary>
        public long Carne { get; set; }

        /// <summary>
        /// Cédula del empleado.
        /// </summary>
        public string Cedula { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del empleado.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Primer apellido del empleado.
        /// </summary>
        public string PrimerApellido { get; set; } = string.Empty;

        /// <summary>
        /// Segundo apellido del empleado.
        /// </summary>
        public string SegundoApellido { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del departamento al que pertenece el empleado.
        /// (JOIN a Departamentos.Departamento).
        /// </summary>
        public string Departamento { get; set; } = string.Empty;

        /// <summary>
        /// Horas normales trabajadas en la semana.
        /// </summary>
        public decimal HorasNormales { get; set; }

        /// <summary>
        /// Horas extras trabajadas en la semana.
        /// </summary>
        public decimal HorasExtras { get; set; }

        /// <summary>
        /// Horas dobles trabajadas en la semana.
        /// </summary>
        public decimal HorasDobles { get; set; }

        /// <summary>
        /// Horas trabajadas en días feriados.
        /// </summary>
        public decimal Feriados { get; set; }

        /// <summary>
        /// Total de deducción por soda.
        /// </summary>
        public decimal DeduccionSoda { get; set; }

        /// <summary>
        /// Total de deducción por uniforme.
        /// </summary>
        public decimal DeduccionUniforme { get; set; }

        /// <summary>
        /// Otras deducciones aplicadas.
        /// </summary>
        public decimal DeduccionOtras { get; set; }

        /// <summary>
        /// Salario bruto calculado para la semana.
        /// </summary>
        public decimal SalarioBruto { get; set; }

        /// <summary>
        /// Salario neto calculado para la semana.
        /// </summary>
        public decimal SalarioNeto { get; set; }
    }
}
