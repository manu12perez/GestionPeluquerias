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
        public DbSet<VistaPeluqueriaDetalle> PeluqueriaDetalles { get; set; } // Vista PeluqueriaDetalle

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            

            // Relación 1 a 1: Usuario - Peluquero (No todos los usuarios son peluqueros)
            modelBuilder.Entity<Peluquero>()
                .HasOne(p => p.Usuario)
                .WithOne(u => u.Peluquero)
                .HasForeignKey<Peluquero>(p => p.IdUsuario)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación 1 a N: Peluquería - Peluquero (Cada peluquero pertenece a una peluquería)
            modelBuilder.Entity<Peluquero>()
                .HasOne(p => p.Peluqueria)
                .WithMany(pe => pe.Peluqueros)
                .HasForeignKey(p => p.IdPeluqueria)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación 1 a N: Peluquería -> Servicios
            modelBuilder.Entity<Servicio>()
                .HasOne(s => s.Peluqueria)
                .WithMany(p => p.Servicios)
                .HasForeignKey(s => s.IdPeluqueria)
                .OnDelete(DeleteBehavior.Cascade);

            

            // Relación 1 a N: Usuario - Citas
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Citas)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación 1 a N: Usuario - Peluquerías (Administra varias)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.PeluqueriasAdministradas)
                .WithOne(p => p.Administrador)
                .HasForeignKey(p => p.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación 1 a N: Peluquero - Citas
            modelBuilder.Entity<Peluquero>()
                .HasMany(p => p.Citas)
                .WithOne(c => c.Peluquero)
                .HasForeignKey(c => c.IdPeluquero)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación 1 a N: Cita - Servicio
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Servicio)
                .WithMany(s => s.Citas)
                .HasForeignKey(c => c.IdServicio)
                .OnDelete(DeleteBehavior.Cascade);

            // Datos iniciales (Opcional)
            modelBuilder.Entity<Rol>().HasData(
                new Rol { IdRol = 1, Nombre = "Administrador" },
                new Rol { IdRol = 2, Nombre = "Peluquero" },
                new Rol { IdRol = 3, Nombre = "Cliente" }
            );
        }
    }
}