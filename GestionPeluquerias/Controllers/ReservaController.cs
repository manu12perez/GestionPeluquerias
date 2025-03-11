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
        private IRepositoryUsuarios repoUsuario;

        public ReservaController(IRepositoryPeluquerias repoPeluquerias, IRepositoryUsuarios repoUsuario)
        {
            this.repoPelu = repoPeluquerias;
            this.repoUsuario = repoUsuario;
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

        public async Task<IActionResult> CreatePeluquero(int idpeluqueria)
        {
            ViewData["IDPELUQUERIA"] = idpeluqueria;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePeluquero(Peluquero peluquero)
        {
            // Verificar que los IDs existan, pero NO asignar las entidades
            var usuario = await this.repoUsuario.FindUsuarioAsync(peluquero.IdUsuario);
            if (usuario == null)
            {
                ModelState.AddModelError("IdUsuario", "El usuario no existe");
                ViewData["IDPELUQUERIA"] = peluquero.IdPeluqueria;
                return View(peluquero);
            }

            var peluqueria = await this.repoPelu.FindPeluqueriaAsync(peluquero.IdPeluqueria);
            if (peluqueria == null)
            {
                ModelState.AddModelError("IdPeluqueria", "La peluquería no existe");
                ViewData["IDPELUQUERIA"] = peluquero.IdPeluqueria;
                return View(peluquero);
            }

            await this.repoPelu.InsertPeluqueroAsync(peluquero);
            return RedirectToAction("CreateServicio", new { idpeluqueria = peluquero.IdPeluqueria });
        }

        public async Task<IActionResult> CreateServicio(int idpeluqueria)
        {
            ViewData["IDPELUQUERIA"] = idpeluqueria;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateServicio(Servicio servicio)
        {            
            await this.repoPelu.InserServicioAsync(servicio);
            return RedirectToAction("Index", "Peluqueria");
        }

    }
}
