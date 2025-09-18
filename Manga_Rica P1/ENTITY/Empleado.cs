using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
namespace Manga_Rica_P1.Entity
{
    [Table("Empleado", Schema = "dbo")]
    public class Empleado
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Cedula")]
        [Required(ErrorMessage = "El campo de Cedula es requerido")]
        public string Cedula { get; set; } = "";

        [Column("Primer_Apellido")]
        [Required(ErrorMessage = "El campo de Primer_Apellido es requerido")]
        public string Primer_Apellido { get; set; } = "";

        [Column("Segundo_Apellido")]
        [Required(ErrorMessage = "El campo de Segundo_Apellido es requerido")]
        public string? Segundo_Apellido { get; set; }

        [Column("Nombre")]
        [Required(ErrorMessage = "El campo de Nombre es requerido")]
        public string Nombre { get; set; } = "";

        [Column("Fecha_Nacimiento")]
        [Required(ErrorMessage = "El campo de Fecha_Nacimiento es requerido")]
        public DateTime Fecha_Nacimiento { get; set; }

        [Column("Estado_Civil")]
        [Required(ErrorMessage = "El campo de Estado_Civil es requerido")]
        public string Estado_Civil { get; set; } = "";

        [Column("Celular")]
        [Required(ErrorMessage = "El campo de Celular es requerido")]
        public string Celular { get; set; } = "";

        [Column("Nacionalidad")]
        [Required(ErrorMessage = "El campo de Nacionalidad es requerido")]
        public string Nacionalidad { get; set; } = "";

        [Column("Laboro")]
        [Required(ErrorMessage = "El campo de Laboro es requerido")]
        public int Laboro { get; set; }

        [Column("Direccion")]
        [Required(ErrorMessage = "El campo de Direccion es requerido")]
        public string Direccion { get; set; } = "";

        [Column("Id_Departamento")]
        [Required(ErrorMessage = "El campo de Id_Departamento es requerido")]
        public int Id_Departamento { get; set; }

        [Column("Salario")]
        [Required(ErrorMessage = "El campo de Salario es requerido")]
        public float Salario { get; set; }

        [Column("Puesto")]
        [Required(ErrorMessage = "El campo de Puesto es requerido")]
        public string Puesto { get; set; } = "";

        [Column("Fecha_Ingreso")]
        [Required(ErrorMessage = "El campo de Fecha_Ingreso es requerido")]
        public DateTime Fecha_Ingreso { get; set; }

        [Column("Fecha_Salida")]
        [Required(ErrorMessage = "El campo de Fecha_Salida es requerido")]
        public DateTime Fecha_Salida { get; set; }

        [Column("Foto")]
        public string Foto { get; set; } = "";

        [Column("Activo")]
        [Required(ErrorMessage = "El campo de Activo es requerido")]
        public int Activo { get; set; }

        [Column("MC_Numero")]
        public long MC_Numero { get; set; }

    }
}
