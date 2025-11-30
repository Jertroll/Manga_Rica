// DAL/Reports/Dtos/UniformeReportRow.cs
namespace Manga_Rica_P1.DAL.Reports.Dtos
{
    
    public sealed class UniformeReportRow
    {
        public string Departamento { get; set; } = "";
        public string Carne { get; set; } = "";
        public string Apellidos { get; set; } = "";
        public string Nombre { get; set; } = "";

        public long IdDeduccion { get; set; }           
        public int CodigoArticulo { get; set; }
        public string DescripcionArticulo { get; set; } = "";
        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }
        public decimal TotalLinea { get; set; }
        public decimal SaldoDeduccion { get; set; }
    }
}
