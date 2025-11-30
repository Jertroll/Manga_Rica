namespace Manga_Rica_P1.Entity.Reports
{
    public sealed class ReporteUniformesEmpleadoVm
    {
        public string Titulo { get; set; } = "";
        public string PieDePagina { get; set; } = "";

        // Datos del empleado (encabezado del reporte)
        public string Carne { get; set; } = "";
        public string NombreCompleto { get; set; } = "";
        public string Departamento { get; set; } = "";

        // Líneas de detalle (una por cada artículo/deducción)
        public List<UniformeEmpleadoLineaVm> Lineas { get; set; } = new();

        // Total general (suma de todos los totales de línea)
        public decimal TotalGeneral { get; set; }
    }

    /// <summary>
    /// Línea del detalle de uniformes para un empleado.
    /// </summary>
    public sealed class UniformeEmpleadoLineaVm
    {
        public DateTime Fecha { get; set; }        // Fecha de la deducción
        public long IdDeduccion { get; set; }      // N° de deducción (columna "Deducción")
        public int CodigoArticulo { get; set; }    // Código
        public string DescripcionArticulo { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal TotalLinea { get; set; }
    }
}
