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
        public string username { get; set; } = "";

        [Column("Clave")]
        public string password { get; set; } = "";

        [Column("Perfil")]
        public string? perfil { get; set; }

        [Column("Fecha")]
        public DateTime fecha { get; set; }


    }

}
