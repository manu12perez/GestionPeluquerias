using GestionPeluquerias.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionPeluquerias.Interfaces
{
    public interface IRepositoryUsuarios
    {
        Task<List<Usuario>> GetUsuariosAsync();
        Task<Usuario?> FindUsuarioAsync(int idUsuario);
        Task<List<Usuario>> GetAdministradoresAsync();
        Task InsertUsuarioAsync(Usuario usuario);
        Task<bool> UpdateUsuarioAsync(Usuario usuario); 
        Task<bool> DeleteUsuarioAsync(int idUsuario); 
    }
}
