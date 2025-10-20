using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manga_Rica_P1.ENTITY
{
    [Table("Pagos", Schema = "dbo")]
    public class Pagos
    {
        
        [Key]
        [Column("Id")]
        public int Id { get; set; } 

       
        [Column("Id_Empleado")]
        [Required(ErrorMessage = "El campo Id_Empleado es requerido")]
        public long Id_Empleado { get; set; }

        
        [Column("Fecha")]
        [Required(ErrorMessage = "El campo Fecha es requerido")]
        public DateTime Fecha { get; set; }

        [Column("Id_Semana")]
        [Required(ErrorMessage = "El campo Id_Semana es requerido")]
        public int Id_Semana { get; set; }

       
        [Column("Horas_Normales")]
        [Required(ErrorMessage = "El campo Horas_Normales es requerido")]
        public float Horas_Normales { get; set; }

        
        [Column("Horas_Extras")]
        [Required(ErrorMessage = "El campo Horas_Extras es requerido")]
        public float Horas_Extras { get; set; }

        
        [Column("Horas_Dobles")]
        [Required(ErrorMessage = "El campo Horas_Dobles es requerido")]
        public float Horas_Dobles { get; set; }


        [Column("Feriadas")]
        [Required(ErrorMessage = "El campo Feriadas es requerido")]
        public float Feriadas { get; set; }

        
        [Column("Deduccion_Soda")]
        [Required(ErrorMessage = "El campo Deduccion_Soda es requerido")]
        public float Deduccion_Soda { get; set; }

        // Nueva implementacion
        [Column("Deduccion_Uniforme")]
        [Required(ErrorMessage = "El campo Deduccion_Uniforme es requerido")]
        public float Deduccion_Uniforme { get; set; }

        // Nueva implementacion
        [Column("Deduccion_Otras")]
        [Required(ErrorMessage = "El campo Deduccion_Otras es requerido")]
        public float Deduccion_Otras { get; set; }

        // Nueva implementacion
        [Column("Salario_Bruto")]
        [Required(ErrorMessage = "El campo Salario_Bruto es requerido")]
        public float Salario_Bruto { get; set; }

        // Nueva implementacion
        [Column("Salario_Neto")]
        [Required(ErrorMessage = "El campo Salario_Neto es requerido")]
        public float Salario_Neto { get; set; }

        // Nueva implementacion
        [Column("Id_Usuario")]
        [Required(ErrorMessage = "El campo Id_Usuario es requerido")]
        public int Id_Usuario { get; set; }

        // Nueva implementacion
        [Column("Registrado")]
        [Required(ErrorMessage = "El campo Registrado es requerido")]
        public bool Registrado { get; set; }
    }
}
