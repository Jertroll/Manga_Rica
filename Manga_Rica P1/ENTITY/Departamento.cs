using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manga_Rica_P1.Entity {
    [Table("Departamentos", Schema = "dbo")]
    public class Departamento
    {

        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Departamento")]
        [Required(ErrorMessage = "El campo de departamento es requerido")]
        public string nombre { get; set; } = "";

        [Column("Codigo")]
        [Required(ErrorMessage = "El campo de codigo es requerido")]
        public string codigo { get; set; } = "";

    }
}
