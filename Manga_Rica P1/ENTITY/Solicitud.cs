using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manga_Rica_P1.Entity
{
    [Table("Solicitud", Schema = "dbo")]
    public class Solicitud
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

    }
}
