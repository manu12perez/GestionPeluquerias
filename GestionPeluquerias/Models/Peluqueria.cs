using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPeluquerias.Models
{
    [Table("Peluquerias")]
    public class Peluqueria
    {
        [Key]
        [Column("IdPeluqueria")]
        public int IdPeluqueria { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Column("Nombre")]
        [MaxLength(100)] // Límite según la BD
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [Column("Direccion")]
        [MaxLength(255)] // Límite según la BD
        public string Direccion { get; set; }

        [Required]
        [Column("Latitud")]
        public double Latitud { get; set; }

        [Required]
        [Column("Longitud")]
        public double Longitud { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [RegularExpression(@"^\d{9,15}$", ErrorMessage = "El teléfono debe tener entre 9 y 15 dígitos numéricos.")]
        [Column("Telefono")]
        [MaxLength(20)] // Límite según la BD
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La hora de apertura es obligatoria")]
        [Column("HorarioApertura")]
        public TimeSpan HorarioApertura { get; set; }

        [Required(ErrorMessage = "La hora de cierre es obligatoria")]
        [Column("HorarioCierre")]
        public TimeSpan HorarioCierre { get; set; }

        // Relación con el Administrador (Usuario que la gestiona)
        [Required(ErrorMessage = "Debe seleccionar un administrador")]
        [Column("IdUsuario")]
        [ForeignKey("Administrador")]
        public int IdUsuario { get; set; }
        public Usuario Administrador { get; set; }

        // Relación con Servicios (Una peluquería tiene varios servicios)
        public ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();

        // Relación con Peluqueros (Una peluquería tiene varios peluqueros)
        public ICollection<Peluquero> Peluqueros { get; set; } = new List<Peluquero>();
    }
}
