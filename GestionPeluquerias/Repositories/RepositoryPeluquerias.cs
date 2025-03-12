using Microsoft.EntityFrameworkCore;
using GestionPeluquerias.Data;
using GestionPeluquerias.Models;
using GestionPeluquerias.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

#region Vistas y Procedimientos Almacenados
/*
CREATE VIEW vw_DetallesPeluqueria AS
SELECT 
    CAST(ROW_NUMBER() OVER (ORDER BY p.IdPeluqueria) AS INT) AS Id, 
    CAST(p.IdPeluqueria AS INT) AS IdPeluqueria,
    p.Nombre AS NombrePeluqueria,
    p.Direccion,
    p.Telefono,
    p.HorarioApertura,
    p.HorarioCierre,
    u.Nombre AS NombreAdministrador,
    CAST(pe.IdPeluquero AS INT) AS IdPeluquero,
    CAST(pe.IdUsuario AS INT) AS IdUsuarioPeluquero,
    up.Nombre AS NombrePeluquero,
    CAST(s.IdServicio AS INT) AS IdServicio,
    s.Nombre AS NombreServicio,
    s.Descripcion,
    s.Precio AS PrecioServicio,
    CAST(ISNULL(s.Duracion, 0) AS INT) AS Duracion
FROM Peluquerias p
LEFT JOIN Usuarios u ON p.IdUsuario = u.IdUsuario
LEFT JOIN Peluqueros pe ON p.IdPeluqueria = pe.IdPeluqueria
LEFT JOIN Usuarios up ON pe.IdUsuario = up.IdUsuario
LEFT JOIN Servicios s ON p.IdPeluqueria = s.IdPeluqueria;



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
    #region Repository de Peluqerías
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

        public async Task<List<VistaPeluqueriaDetalle>> GetPeluqueriaDetallesAsync(int idPeluqueria)
        {
            string sql = "SELECT * FROM vw_DetallesPeluqueria WHERE IdPeluqueria = @p0";
            return await context.PeluqueriaDetalles.FromSqlRaw(sql, idPeluqueria).ToListAsync();
        }


        public async Task<int> GetMaxIdPeluqueriaAsync()
        {
            if (!await context.Peluquerias.AnyAsync())
            {
                return 0; // Devuelve 0 para que el primer ID sea 1 y no 2
            }
            return await context.Peluquerias.MaxAsync(p => p.IdPeluqueria);
        }

        public async Task<Peluqueria> InsertPeluqueriaAsync(Peluqueria peluqueria)
        {
            int maxId = await context.Peluquerias.AnyAsync()
                ? await context.Peluquerias.MaxAsync(p => p.IdPeluqueria)
                : 0;

            peluqueria.IdPeluqueria = maxId + 1;

            await context.Peluquerias.AddAsync(peluqueria);
            await context.SaveChangesAsync();

            return peluqueria;
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
            // Buscar la peluquería y sus entidades relacionadas
            var peluqueria = await context.Peluquerias
                .Include(p => p.Peluqueros) // Incluir los peluqueros relacionados
                .Include(p => p.Servicios) // Incluir los servicios relacionados
                .FirstOrDefaultAsync(p => p.IdPeluqueria == idPeluqueria);

            if (peluqueria == null)
            {
                return false; // No se encontró la peluquería
            }

            // Primero eliminar los peluqueros relacionados
            if (peluqueria.Peluqueros.Any())
            {
                context.Peluqueros.RemoveRange(peluqueria.Peluqueros);
            }

            // Luego eliminar los servicios relacionados
            if (peluqueria.Servicios.Any())
            {
                context.Servicios.RemoveRange(peluqueria.Servicios);
            }

            // Ahora eliminar la peluquería
            context.Peluquerias.Remove(peluqueria);
            await context.SaveChangesAsync();

            return true; // Eliminación exitosa
        }



        public async Task<List<Servicio>> GetServiciosByIdPeluqueria(int idPeluqueria)
        {
            var consulta = from datos in this.context.Servicios
                           where datos.IdPeluqueria == idPeluqueria
                           select datos;

            return await consulta.ToListAsync();
        }

        public async Task InsertPeluqueroAsync(Peluquero peluquero)
        {
            int maxId = await context.Peluqueros.AnyAsync()
                ? await context.Peluqueros.MaxAsync(p => p.IdPeluquero)
                : 0;

            peluquero.IdPeluquero = maxId + 1;
            this.context.Peluqueros.Add(peluquero);
            await this.context.SaveChangesAsync();
        }

        public async Task InserServicioAsync(Servicio servicio)
        {
            int maxId = await context.Servicios.AnyAsync()
                ? await context.Servicios.MaxAsync(p => p.IdServicio)
                : 0;

            servicio.IdServicio = maxId + 1;
            this.context.Servicios.Add(servicio);
            await this.context.SaveChangesAsync();
        }

        public async Task InsertCitaAsync(Cita cita)
        {
            int maxId = await context.Citas.AnyAsync()
               ? await context.Citas.MaxAsync(p => p.IdCita)
               : 0;

            cita.IdCita = maxId + 1;
            this.context.Citas.Add(cita);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Peluquero>> GetPeluquerosByIdPeluqueria(int idPeluqueria)
        {
            var consulta = await this.context.Peluqueros
                .Include(p => p.Usuario) // Asegura que Usuario se incluya
                .Where(p => p.IdPeluqueria == idPeluqueria)
                .ToListAsync();

            return consulta;
        }
    }
    #endregion

    #region Repository de Usuarios
    public class RepositoryUsuarios : IRepositoryUsuarios
    {
        private PeluqueriaContext context;

        public RepositoryUsuarios(PeluqueriaContext context)
        {
            this.context = context;
        }

        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            return await context.Usuarios
                .Include(u => u.Rol)
                .ToListAsync();
        }

        public async Task<Usuario?> FindUsuarioAsync(int idUsuario)
        {
            return await context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
        }

        public async Task<List<Usuario>> GetAdministradoresAsync()
        {
            return await context.Usuarios
                .AsNoTracking()
                .Where(u => u.IdRol == 1)
                .ToListAsync();
        }

        public async Task InsertUsuarioAsync(Usuario usuario)
        {
            usuario.IdUsuario = await context.Usuarios.AnyAsync()
                ? await context.Usuarios.MaxAsync(u => u.IdUsuario) + 1
                : 1;

            await context.Usuarios.AddAsync(usuario);
            await context.SaveChangesAsync();
        }

        public async Task<bool> UpdateUsuarioAsync(Usuario usuario)
        {
            var existingUsuario = await context.Usuarios.FindAsync(usuario.IdUsuario);
            if (existingUsuario == null)
                return false;

            existingUsuario.Nombre = usuario.Nombre;
            existingUsuario.Apellido = usuario.Apellido;
            existingUsuario.Email = usuario.Email;
            existingUsuario.Password = usuario.Password;
            existingUsuario.Telefono = usuario.Telefono;
            existingUsuario.IdRol = usuario.IdRol;

            context.Usuarios.Update(existingUsuario);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUsuarioAsync(int idUsuario)
        {
            var usuario = await context.Usuarios.FindAsync(idUsuario);
            if (usuario == null)
                return false;

            context.Usuarios.Remove(usuario);
            await context.SaveChangesAsync();
            return true;
        }
    }
    #endregion
}






