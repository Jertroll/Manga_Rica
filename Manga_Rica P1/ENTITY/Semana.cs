using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Manga_Rica_P1.Entity
{
    [Table("Semanas", Schema = "dbo")]
    public class Semana
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Semana")]
        [Required(ErrorMessage = "El campo de Semana es requerido")]
        public int semana { get; set; }

        [Column("Fecha_Inicio")]
        [Required(ErrorMessage = "El campo de Fecha inicial es requerido")]
        public DateTime fecha_Inicio { get; set; }

        [Column("Fecha_Final")]
        [Required(ErrorMessage = "El campo de Fecha final es requerido")]
        public DateTime fecha_Final { get; set; }


    }
}
