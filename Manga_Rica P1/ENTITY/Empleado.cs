using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manga_Rica_P1.Entity
{
    [Table("Empleados", Schema = "dbo")]   // <-- plural
    public class Empleado
    {
        [Key][Column("Id")] public int Id { get; set; }       // BIGINT -> int en app

        [Column("Carne")] public long Carne { get; set; }    
        [Column("Cedula")][Required] public string Cedula { get; set; } = "";

        [Column("Primer_Apellido")][Required] public string Primer_Apellido { get; set; } = "";
        [Column("Segundo_Apellido")][Required] public string Segundo_Apellido { get; set; } = ""; // NOT NULL en BD
        [Column("Nombre")][Required] public string Nombre { get; set; } = "";

        [Column("Fecha_Nacimiento")][Required] public DateTime Fecha_Nacimiento { get; set; }
        [Column("Estado_Civil")][Required] public string Estado_Civil { get; set; } = "";

        [Column("Telefono")][Required] public string Telefono { get; set; } = "";
        [Column("Celular")][Required] public string Celular { get; set; } = "";

        [Column("Nacionalidad")][Required] public string Nacionalidad { get; set; } = "";

        [Column("Laboro")][Required] public int Laboro { get; set; }

        [Column("Direccion")][Required] public string Direccion { get; set; } = "";

        [Column("Id_Departamento")][Required] public int Id_Departamento { get; set; }

        [Column("Salario")][Required] public float Salario { get; set; }

        [Column("Puesto")][Required] public string Puesto { get; set; } = "";

        [Column("Fecha_Ingreso")][Required] public DateTime Fecha_Ingreso { get; set; }
        [Column("Fecha_Salida")][Required] public DateTime Fecha_Salida { get; set; }   // NOT NULL en BD

        [Column("Foto")] public string Foto { get; set; } = ""; // NOT NULL (cadena vacía es válida)
        [Column("Activo")][Required] public int Activo { get; set; }

        [Column("MC_Numero")] public long MC_Numero { get; set; }
    }
}
