using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPeluquerias.Models
{
    [Table("Peluqueros")]
    public class Peluquero
    {
        [Key]
        [Column("IdPeluquero")]
        public int IdPeluquero { get; set; }

        // Relación con Usuario (Cada peluquero tiene un usuario)
        [Required]
        [Column("IdUsuario")]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        // Relación con Peluquería (Cada peluquero trabaja en una peluquería)
        [Required]
        [Column("IdPeluqueria")]
        [ForeignKey("Peluqueria")]
        public int IdPeluqueria { get; set; }
        public Peluqueria Peluqueria { get; set; }

        // Relación con Citas (Un peluquero tiene muchas citas)
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
