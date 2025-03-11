using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GestionPeluquerias.Models
{
    [Table("vw_DetallesPeluqueria")]
    public class VistaPeluqueriaDetalle
    {
        [Key]
        public int Id { get; set; }
        public int IdPeluqueria { get; set; }
        public string NombrePeluqueria { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        // Propiedades string ya son referencias y pueden ser null naturalmente

        // Tipos de valor que deben ser explícitamente marcados como nullable
        public TimeSpan? HorarioApertura { get; set; }
        public TimeSpan? HorarioCierre { get; set; }
        public string NombreAdministrador { get; set; }

        // Propiedades que pueden ser nulas debido a los LEFT JOIN
        public int? IdPeluquero { get; set; }
        public int? IdUsuarioPeluquero { get; set; }
        public string NombrePeluquero { get; set; }
        public int? IdServicio { get; set; }
        public string NombreServicio { get; set; }
        public string Descripcion { get; set; }
        public decimal? PrecioServicio { get; set; }
        public int? Duracion { get; set; } // A pesar del ISNULL, es mejor marcarlo nullable
    }
}