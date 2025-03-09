using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPeluquerias.Models
{
    [Table("Roles")]
    public class Rol
    {
        [Key]
        [Column("IdRol")]
        public int IdRol { get; set; }

        [Required]  // Asegura que no sea nulo en la base de datos
        [Column("Nombre")]
        public string Nombre { get; set; }

        // Relación con Usuarios (inicializada para evitar null references)
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
