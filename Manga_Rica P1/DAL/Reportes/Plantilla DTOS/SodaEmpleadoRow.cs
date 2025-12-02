using System;

namespace Manga_Rica_P1.DAL.Reports.Dtos
{
    /// <summary>
    /// Fila “plana” que sale del SQL de Soda por empleado.
    /// </summary>
    public sealed class SodaEmpleadoRow
    {
        public long Factura { get; set; }
        public DateTime Fecha { get; set; }
        public long Carne { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string DescripcionArticulo { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal TotalLinea { get; set; }
    }
}
