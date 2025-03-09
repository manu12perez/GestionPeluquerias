using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPeluquerias.Models
{
    [Table("PeluqueroServicios")]
    public class PeluqueroServicio
    {
        [Required]
        [Column("IdPeluquero")]
        [ForeignKey("Peluquero")]
        public int IdPeluquero { get; set; }
        public Peluquero Peluquero { get; set; }

        [Required]
        [Column("IdServicio")]
        [ForeignKey("Servicio")]
        public int IdServicio { get; set; }
        public Servicio Servicio { get; set; }
    }
}
