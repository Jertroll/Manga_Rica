using System;
using System.Collections.Generic;

namespace Manga_Rica_P1.Entity.Reports
{
    /// <summary>
    /// ViewModel del reporte "Soda por Empleado".
    /// Coincide con lo que mostraba el Crystal Report antiguo:
    /// encabezado con Carne / Nombre / rango de fechas
    /// y tabla de facturas con totales.
    /// </summary>
    public sealed class ReporteSodaEmpleadoVm
    {
        // Encabezado / metadatos
        public string Titulo { get; set; } = "Detalle de Soda";
        public string PieDePagina { get; set; } = string.Empty;

        /// <summary>Carne del empleado mostrado en el encabezado.</summary>
        public string Carne { get; set; } = string.Empty;

        /// <summary>Nombre completo del empleado (apellidos + nombre).</summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>Fecha inicial del rango del reporte.</summary>
        public DateTime FechaInicio { get; set; }

        /// <summary>Fecha final del rango del reporte.</summary>
        public DateTime FechaFin { get; set; }

        /// <summary>Líneas (facturas de soda) del empleado en el rango.</summary>
        public List<SodaEmpleadoLineaVm> Lineas { get; set; } = new();

        /// <summary>Total de la columna "Total" de todas las líneas.</summary>
        public decimal TotalGeneral { get; set; }
    }

    /// <summary>
    /// Línea del reporte de soda por empleado.
    /// Corresponde a una factura / detalle específico.
    /// </summary>
    public sealed class SodaEmpleadoLineaVm
    {
        /// <summary>Consecutivo de Soda (Soda.Id).</summary>
        public long Factura { get; set; }

        /// <summary>Fecha de la factura (Soda.Fecha).</summary>
        public DateTime Fecha { get; set; }

        /// <summary>Descripción del artículo de soda (Articulos.Descripcion).</summary>
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>Cantidad vendida (Soda_Detalles.Cantidad).</summary>
        public int Cantidad { get; set; }

        /// <summary>Precio unitario (Soda_Detalles.Precio).</summary>
        public decimal Precio { get; set; }

        /// <summary>Total de la línea (Soda_Detalles.Total).</summary>
        public decimal Total { get; set; }
    }
}
