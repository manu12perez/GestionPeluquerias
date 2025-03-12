using GestionPeluquerias.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionPeluquerias.Interfaces
{
    #region Interface de Pelquerías
    public interface IRepositoryPeluquerias
    {
        Task<List<Peluqueria>> GetPeluqueriasAsync();
        Task<Peluqueria> FindPeluqueriaAsync(int idPeluqueria);
        Task<Peluqueria> InsertPeluqueriaAsync(Peluqueria peluqueria);
        Task UpdatePeluqueriaAsync(Peluqueria peluqueria);
        Task<bool> DeletePeluqueriaAsync(int idPeluqueria);
        Task<List<Servicio>> GetServiciosByIdPeluqueria(int idPelqueria);
        Task<List<VistaPeluqueriaDetalle>> GetPeluqueriaDetallesAsync(int idPeluqueria);
        Task InsertPeluqueroAsync(Peluquero peluquero);
        Task InserServicioAsync(Servicio servicio);
        Task InsertCitaAsync(Cita cita);
        Task<List<Peluquero>> GetPeluquerosByIdPeluqueria(int idPeluqueria);
    }
    #endregion

    #region Interface de Usuarios
    public interface IRepositoryUsuarios
    {
        Task<List<Usuario>> GetUsuariosAsync();
        Task<Usuario?> FindUsuarioAsync(int idUsuario);
        Task<List<Usuario>> GetAdministradoresAsync();
        Task InsertUsuarioAsync(Usuario usuario);
        Task<bool> UpdateUsuarioAsync(Usuario usuario);
        Task<bool> DeleteUsuarioAsync(int idUsuario);
    }
    #endregion
}
