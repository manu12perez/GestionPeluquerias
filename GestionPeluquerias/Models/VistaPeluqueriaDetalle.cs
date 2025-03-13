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


        public TimeSpan? HorarioApertura { get; set; }
        public TimeSpan? HorarioCierre { get; set; }
        public string NombreAdministrador { get; set; }


        public int? IdPeluquero { get; set; }
        public int? IdUsuarioPeluquero { get; set; }
        public string NombrePeluquero { get; set; }
        public int? IdServicio { get; set; }
        public string NombreServicio { get; set; }
        public string Descripcion { get; set; }
        public decimal? PrecioServicio { get; set; }
        public int? Duracion { get; set; }
    }
}