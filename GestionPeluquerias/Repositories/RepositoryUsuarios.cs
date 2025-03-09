using Microsoft.EntityFrameworkCore;
using GestionPeluquerias.Data;
using GestionPeluquerias.Models;
using GestionPeluquerias.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionPeluquerias.Repositories
{
    public class RepositoryUsuarios : IRepositoryUsuarios
    {
        private readonly PeluqueriaContext _context;

        public RepositoryUsuarios(PeluqueriaContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Rol) // Cargar el rol del usuario
                .ToListAsync();
        }

        public async Task<Usuario?> FindUsuarioAsync(int idUsuario)
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
        }

        public async Task<List<Usuario>> GetAdministradoresAsync()
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Where(u => u.IdRol == 1) // Suponiendo que el rol 1 es Administrador
                .ToListAsync();
        }

        public async Task InsertUsuarioAsync(Usuario usuario)
        {
            int maxId = await _context.Usuarios.AnyAsync()
                ? await _context.Usuarios.MaxAsync(u => u.IdUsuario)
                : 0;

            usuario.IdUsuario = maxId + 1;

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateUsuarioAsync(Usuario usuario)
        {
            var existingUsuario = await _context.Usuarios.FindAsync(usuario.IdUsuario);
            if (existingUsuario == null)
            {
                return false;
            }

            existingUsuario.Nombre = usuario.Nombre;
            existingUsuario.Apellido = usuario.Apellido;
            existingUsuario.Email = usuario.Email;
            existingUsuario.Password = usuario.Password;
            existingUsuario.Telefono = usuario.Telefono;
            existingUsuario.IdRol = usuario.IdRol;

            _context.Usuarios.Update(existingUsuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUsuarioAsync(int idUsuario)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);
            if (usuario == null)
            {
                return false;
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
