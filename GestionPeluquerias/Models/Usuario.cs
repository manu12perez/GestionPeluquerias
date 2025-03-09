using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPeluquerias.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [Column("IdUsuario")]
        public int IdUsuario { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("Apellido")]
        public string Apellido { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [Column("Telefono")]
        public string Telefono { get; set; }

        // Relación con Roles
        [Column("IdRol")]
        [ForeignKey("Rol")]
        public int IdRol { get; set; }
        public Rol Rol { get; set; }

        // Relación con Peluquería (Si es Administrador)
        public ICollection<Peluqueria> PeluqueriasAdministradas { get; set; }

        // Relación con Peluquero (Si es un peluquero)
        public Peluquero Peluquero { get; set; }

        // Relación con Citas (Si es un cliente)
        public ICollection<Cita> Citas { get; set; }
    }
}
