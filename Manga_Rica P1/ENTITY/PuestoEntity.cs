using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Rica_P1.Entity
{
    [Table("Puesto", Schema = "dbo")]
    public class PuestoEntity
    {
            [Key]
            [Column("Id")]
            public int Id { get; set; }

            [Column("Puesto")]
            [Required(ErrorMessage = "El campo de puesto es requerido")]
            public string puesto { get; set; } = "";

            [Column("Descripcion")]
            public string descripcion { get; set; } = "";
   
    }
}
