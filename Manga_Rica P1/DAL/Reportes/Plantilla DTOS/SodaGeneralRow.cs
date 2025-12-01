using System;

namespace Manga_Rica_P1.DAL.Reports.Dtos
{
    public sealed class SodaGeneralRow
    {
        public long Factura { get; set; }
        public DateTime Fecha { get; set; }
        public string Carne { get; set; } = "";
        public string NombreCompleto { get; set; } = "";
        public string DescripcionArticulo { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal TotalLinea { get; set; }
    }
}
