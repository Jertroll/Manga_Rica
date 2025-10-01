using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Rica_P1.ENTITY
{
    [Table("Deducciones_Detalles", Schema = "dbo")]
    public class Deducciones_Detalles
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Id_Deduccion")]
        [Required(ErrorMessage = "El campo de Id_Soda es requerido")]
        public int Id_Soda { get; set; }

        [Column("Codigo_Articulo")]
        [Required(ErrorMessage = "El campo de Codigo_Articulo es requerido")]
        public int Codigo_Articulo { get; set; }

        [Column("Cantidad")]
        [Required(ErrorMessage = "El campo de Cantidad es requerido")]
        public int Cantidad { get; set; }

        [Column("Precio")]
        [Required(ErrorMessage = "El campo de Precio es requerido")]
        public float Precio { get; set; }

        [Column("Total")]
        [Required(ErrorMessage = "El campo de Total es requerido")]
        public float Total { get; set; }
    }
}
