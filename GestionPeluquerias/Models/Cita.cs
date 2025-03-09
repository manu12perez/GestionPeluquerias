using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPeluquerias.Models
{
    [Table("Citas")]
    public class Cita
    {
        [Key]
        [Column("IdCita")]
        public int IdCita { get; set; }

        // Relación con Cliente (Usuario)
        [Required]
        [Column("IdUsuario")]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; } // Cliente que reserva la cita

        // Relación con Peluquería
        [Required]
        [Column("IdPeluqueria")]
        [ForeignKey("Peluqueria")]
        public int IdPeluqueria { get; set; }
        public Peluqueria Peluqueria { get; set; }

        // Relación con Peluquero
        [Required]
        [Column("IdPeluquero")]
        [ForeignKey("Peluquero")]
        public int IdPeluquero { get; set; }
        public Peluquero Peluquero { get; set; }

        // Relación con Servicio
        [Required]
        [Column("IdServicio")]
        [ForeignKey("Servicio")]
        public int IdServicio { get; set; }
        public Servicio Servicio { get; set; }

        [Required]
        [Column("FechaCita")]
        public DateTime FechaCita { get; set; }

        [Required]
        [Column("HoraCita")]
        public TimeSpan HoraCita { get; set; }

        [Required]
        [Column("Estado")]
        public bool Estado { get; set; } = true; // 1 = Confirmada, 0 = Cancelada
    }
}
