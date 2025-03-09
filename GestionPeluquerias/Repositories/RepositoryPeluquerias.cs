using Microsoft.EntityFrameworkCore;
using GestionPeluquerias.Data;
using GestionPeluquerias.Models;
using GestionPeluquerias.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

#region Vistas y Procedimientos Almacenados
/*

CREATE VIEW vw_ServiciosPeluquerosPeluquerias AS
SELECT 
    s.Nombre AS ServicioNombre,
    s.Descripcion AS ServicioDescripcion,
    s.Precio AS ServicioPrecio,
    s.Duracion AS ServicioDuracion,
    u.Nombre AS PeluqueroNombre,
    u.Apellido AS PeluqueroApellido,
    p.Nombre AS PeluqueriaNombre
FROM Servicios s
JOIN Peluquerias p ON s.IdPeluqueria = p.IdPeluqueria
JOIN Peluqueros pe ON p.IdPeluqueria = pe.IdPeluqueria
JOIN Usuarios u ON pe.IdUsuario = u.IdUsuario;
GO

*/
#endregion

namespace GestionPeluquerias.Repositories
{
    public class RepositoryPeluquerias : IRepositoryPeluquerias
    {
        private  PeluqueriaContext context;

        public RepositoryPeluquerias(PeluqueriaContext context)
        {
            this.context = context;
        }

        public async Task<List<Peluqueria>> GetPeluqueriasAsync()
        {
            return await context.Peluquerias
                .Include(p => p.Administrador) // Carga la relación con el usuario administrador
                .Include(p => p.Servicios) // Carga los servicios ofrecidos por la peluquería
                .Include(p => p.Peluqueros) // Carga los peluqueros asociados a la peluquería
                .ToListAsync();
        }

        public async Task<Peluqueria?> FindPeluqueriaAsync(int idPeluqueria)
        {
            return await context.Peluquerias
                .AsNoTracking() // Mejora el rendimiento en lecturas
                .Include(p => p.Administrador)
                .Include(p => p.Servicios)
                .Include(p => p.Peluqueros)
                    .ThenInclude(pq => pq.Usuario)
                .FirstOrDefaultAsync(p => p.IdPeluqueria == idPeluqueria);
        }

        public async Task<int> GetMaxIdPeluqueriaAsync()
        {
            if (!await context.Peluquerias.AnyAsync())
            {
                return 0; // Devuelve 0 para que el primer ID sea 1 y no 2
            }
            return await context.Peluquerias.MaxAsync(p => p.IdPeluqueria);
        }

        public async Task InsertPeluqueriaAsync(Peluqueria peluqueria)
        {
            int maxId = await context.Peluquerias.AnyAsync()
                ? await context.Peluquerias.MaxAsync(p => p.IdPeluqueria)
                : 0;

            peluqueria.IdPeluqueria = maxId + 1;

            await context.Peluquerias.AddAsync(peluqueria);
            await context.SaveChangesAsync();
        }


        public async Task UpdatePeluqueriaAsync(Peluqueria peluqueria)
        {
            var existingPeluqueria = await context.Peluquerias.FindAsync(peluqueria.IdPeluqueria);
            if (existingPeluqueria != null)
            {
                existingPeluqueria.Nombre = peluqueria.Nombre;
                existingPeluqueria.Direccion = peluqueria.Direccion;
                existingPeluqueria.Telefono = peluqueria.Telefono;
                existingPeluqueria.IdUsuario = peluqueria.IdUsuario; // Corregido: No existe IdAdministrador
                existingPeluqueria.Latitud = peluqueria.Latitud;
                existingPeluqueria.Longitud = peluqueria.Longitud;
                existingPeluqueria.HorarioApertura = peluqueria.HorarioApertura;
                existingPeluqueria.HorarioCierre = peluqueria.HorarioCierre;

                context.Peluquerias.Update(existingPeluqueria);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeletePeluqueriaAsync(int idPeluqueria)
        {
            var peluqueria = await context.Peluquerias.FindAsync(idPeluqueria);
            if (peluqueria == null)
            {
                return false; // No se encontró la peluquería
            }

            context.Peluquerias.Remove(peluqueria);
            await context.SaveChangesAsync();
            return true; // Eliminación exitosa
        }

        public async Task<List<Servicio>> GetServiciosByIdPeluqueria(int idPelqueria)
        {
            var consulta = from datos in this.context.Servicios
                           where datos.IdPeluqueria == idPelqueria
                           select datos;

            return await consulta.ToListAsync();
        }
    }
}
