using System;
using System.Collections.Generic;

namespace Manga_Rica_P1.Entity.Reports
{
    
    public sealed class ReporteSodaGeneralVm
    {
        public string Titulo { get; set; } = "";
        public string PieDePagina { get; set; } = "";

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        /// <summary>Detalle plano: una fila por línea de soda.</summary>
        public List<SodaLineaVm> Lineas { get; set; } = new();

        /// <summary>Total de todas las líneas mostradas.</summary>
        public decimal TotalGeneral { get; set; }
    }

    public sealed class SodaLineaVm
    {
        public long Factura { get; set; }             // Id de Soda (Nº de factura)
        public DateTime Fecha { get; set; }

        public string Carne { get; set; } = "";       // lo mostramos tal cual, sin formato raro
        public string Nombre { get; set; } = "";      // nombre completo del empleado

        public string Descripcion { get; set; } = ""; // artículo
        public int Cantidad { get; set; }

        public decimal Precio { get; set; }
        public decimal Total { get; set; }
    }
}
