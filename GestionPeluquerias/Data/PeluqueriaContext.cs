using Microsoft.EntityFrameworkCore;
using GestionPeluquerias.Models;

namespace GestionPeluquerias.Data
{
    public class PeluqueriaContext : DbContext
    {
        public PeluqueriaContext(DbContextOptions<PeluqueriaContext> options) : base(options) { }

        public DbSet<Peluqueria> Peluquerias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Peluquero> Peluqueros { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<PeluqueroServicio> PeluqueroServicios { get; set; } // Tabla intermedia

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar clave primaria compuesta en la tabla intermedia PeluqueroServicios
            modelBuilder.Entity<PeluqueroServicio>()
                .HasKey(ps => new { ps.IdPeluquero, ps.IdServicio });
        }
    }
}
