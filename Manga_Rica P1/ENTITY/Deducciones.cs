using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Rica_P1.Entity
{
    [Table("Deducciones", Schema = "dbo")]
    public class Deducciones
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }

        [Column("Id_Empleado")]
        [Required(ErrorMessage = "El campo de Id_Empleado es requerido")]
        public long Id_Empleado { get; set; }

        [Column("Total")]
        [Required(ErrorMessage = "El campo de Total es requerido")]
        public double Total { get; set; }

        [Column("Saldo")]
        [Required(ErrorMessage = "El campo de Saldo es requerido")]
        public double Saldo { get; set; }

        [Column("Id_Usuario")]
        [Required(ErrorMessage = "El campo de Id_Usuario es requerido")]
        public int Id_Usuario { get; set; }

        [Column("Anulada")]
        public bool Anulada { get; set; }

        [Column("Fecha")]
        [Required(ErrorMessage = "El campo de Fecha es requerido")]
        public DateTime Fecha { get; set; }
    }
}
