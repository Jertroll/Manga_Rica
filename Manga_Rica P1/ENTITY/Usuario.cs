using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manga_Rica_P1.Entity {
    [Table("Usuarios", Schema = "dbo")]
    public class Usuario
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Nombre")]
        [Required(ErrorMessage = "El campo de nombre es requerido")]
        public string username { get; set; } = "";

        [Column("Clave")]
        [Required(ErrorMessage = "El campo de password es requerido")]
        public string password { get; set; } = "";

        [Column("Perfil")]
        [Required(ErrorMessage = "El campo de perfil es requerido")]
        public string? perfil { get; set; }

        [Column("Fecha")]
        [Required(ErrorMessage = "El campo de Fecha es requerido")]
        public DateTime fecha { get; set; }


    }

}
