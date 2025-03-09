using GestionPeluquerias.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionPeluquerias.Interfaces
{
    public interface IRepositoryPeluquerias
    {
        Task<List<Peluqueria>> GetPeluqueriasAsync();
        Task<Peluqueria> FindPeluqueriaAsync(int idPeluqueria);
        Task InsertPeluqueriaAsync(Peluqueria peluqueria);
        Task UpdatePeluqueriaAsync(Peluqueria peluqueria);
        Task<bool> DeletePeluqueriaAsync(int idPeluqueria);
        Task<List<Servicio>> GetServiciosByIdPeluqueria(int idPelqueria);
    }

}
