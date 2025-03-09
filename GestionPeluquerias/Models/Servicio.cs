using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPeluquerias.Models
{
    [Table("Servicios")]
    public class Servicio
    {
        [Key]
        [Column("IdServicio")]
        public int IdServicio { get; set; }

        [Required]
        [Column("Nombre")]
        [MaxLength(100)] // Límite según la BD
        public string Nombre { get; set; }

        [Column("Descripcion")]
        [MaxLength(255)] // Límite según la BD
        public string? Descripcion { get; set; } // Puede ser NULL en la BD

        [Required]
        [Column("Precio")]
        public decimal Precio { get; set; }

        [Required]
        [Column("Duracion")]
        public int Duracion { get; set; } // Minutos

        // Relación con Peluquería
        [Required]
        [Column("IdPeluqueria")]
        [ForeignKey("Peluqueria")]
        public int IdPeluqueria { get; set; }
        public Peluqueria Peluqueria { get; set; }

        // Relación con Peluqueros (Muchos peluqueros pueden ofrecer este servicio)
        public ICollection<PeluqueroServicio> PeluqueroServicios { get; set; }

        // Relación con Citas (Muchas citas pueden estar asociadas a este servicio)
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
