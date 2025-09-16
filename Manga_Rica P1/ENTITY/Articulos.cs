using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Manga_Rica_P1.Entity
{
    [Table("Articulos", Schema = "dbo")]
    public class Articulos
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Descripcion")]
        [Required(ErrorMessage = "El campo de Descripcion es requerido")]
        public string descripcion { get; set; } = "";

        [Column("Precio")]
        [Required(ErrorMessage = "El campo de Precio es requerido")]
        public float precio { get; set; }

        [Column("Existencia")]
        [Required(ErrorMessage = "El campo de Existencia es requerido")]
        public int existencia { get; set; }

        [Column("Categoria")]
        [Required(ErrorMessage = "El campo de Categoria es requerido")]
        public string categoria { get; set; } = "";
    }
}
