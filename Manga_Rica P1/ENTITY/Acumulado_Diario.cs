using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manga_Rica_P1.Entity
{
    [Table("Acumulado_Diario", Schema = "dbo")]
    public class Acumulado_Diario
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }                       // PK (BIGINT)

        [Required]
        [Column("Id_Empleado")]
        public long Id_Empleado { get; set; }              // FK -> Empleados.Id (BIGINT)

        [Required]
        [Column("Fecha")]
        public DateTime Fecha { get; set; }                // datetime

        [Required]
        [Column("Normales")]
        public float Normales { get; set; }                // float (SQL Server)

        [Required]
        [Column("Extras")]
        public float Extras { get; set; }                  // float

        [Required]
        [Column("Dobles")]
        public float Dobles { get; set; }                  // float

        [Required]
        [Column("Feriado")]
        public float Feriado { get; set; }                 // float
    }
}
