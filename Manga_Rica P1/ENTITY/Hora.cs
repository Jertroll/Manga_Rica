// Nueva implementacion
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manga_Rica_P1.Entity
{
    [Table("Horas", Schema = "dbo")]
    public class Hora
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }                   // <- BIGINT

        [Column("Id_Empleado")]
        [Required(ErrorMessage = "El campo id_empleado es necesario")]
        public long Id_Empleado { get; set; }          // <- BIGINT

        [Column("Carne")]
        [Required(ErrorMessage = "El campo Carne es necesario")]
        public long Carne { get; set; }                // <- BIGINT (ya estaba OK)

        [Column("Fecha")]
        [Required]
        public DateTime Fecha { get; set; }            // datetime2(0)

        [Column("Hora_Entrada")]
        [Required]
        public DateTime Hora_Entrada { get; set; }     // datetime2(3)

        [Column("Hora_Salida")]
        public DateTime? Hora_Salida { get; set; }     // NULL permitido

        [Column("Total_Horas")]
        public decimal? Total_Horas { get; set; }      // decimal(6,2) → nullable

        [Column("Id_Usuario")]
        [Required]
        public int Id_Usuario { get; set; }

        [Column("Usuario_Salida_Id")]
        public int? Usuario_Salida_Id { get; set; }

        [NotMapped]
        public bool EstaAbierto => Hora_Salida == null;

        public void CerrarJornada(DateTime horaSalida, double maxHoras = 15)
        {
            if (horaSalida < Hora_Entrada)
                throw new InvalidOperationException("La hora de salida no puede ser menor que la de entrada.");

            var horas = (horaSalida - Hora_Entrada).TotalHours;
            if (horas > maxHoras)
                throw new InvalidOperationException($"Una jornada no puede exceder {maxHoras} horas continuas.");

            Hora_Salida = horaSalida;
            Total_Horas = Math.Round((decimal)horas, 2);
        }
    }
}
