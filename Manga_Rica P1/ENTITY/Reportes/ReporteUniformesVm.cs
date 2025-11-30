namespace Manga_Rica_P1.Entity.Reports
{
    public sealed class ReporteUniformesVm
    {
        public string Titulo { get; set; } = "";
        public string PieDePagina { get; set; } = "";

        // Lista de empleados con sus líneas
        public List<UniformeEmpleadoVm> Empleados { get; set; } = new();
    }

    public sealed class UniformeEmpleadoVm
    {
        public string Carne { get; set; } = "";
        public string Apellidos { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Departamento { get; set; } = "";

        public decimal TotalDeducciones { get; set; }
        public decimal SaldoPendiente { get; set; }

        public List<UniformeDetalleVm> Detalles { get; set; } = new();
    }

    public sealed class UniformeDetalleVm
    {
        public long IdDeduccion { get; set; }
        public int Codigo { get; set; }
        public string Descripcion { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Total { get; set; }
        public decimal SaldoDeduccion { get; set; } // para mostrar saldo en cada fila si quieres
    }
}
