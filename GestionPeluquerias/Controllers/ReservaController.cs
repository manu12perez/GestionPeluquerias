using Microsoft.AspNetCore.Mvc;
using GestionPeluquerias.Models;
using GestionPeluquerias.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionPeluquerias.Repositories;

namespace GestionPeluquerias.Controllers
{
    public class ReservaController : Controller
    {
        private IRepositoryPeluquerias repoPelu;

        public ReservaController(IRepositoryPeluquerias repoPeluquerias)
        {
            repoPelu = repoPeluquerias;
        }

        [Route("Reserva/SeleccionarPeluquero/{idPeluqueria}")]
        public async Task<IActionResult> SeleccionarPeluquero(int idPeluqueria)
        {
            if (idPeluqueria <= 0)
            {
                return BadRequest("ID de peluquería no válido.");
            }

            var peluqueria = await repoPelu.FindPeluqueriaAsync(idPeluqueria);
            if (peluqueria == null)
            {
                return NotFound();
            }

            ViewBag.Peluqueria = peluqueria;
            return View(peluqueria.Peluqueros);
        }
        public async Task<IActionResult> MostrarServicios(int idpeluqueria)
        {
            List<Servicio> servicios = await repoPelu.GetServiciosByIdPeluqueria(idpeluqueria);

            return View(servicios);
        }

    }
}
