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

        public async Task<IActionResult> CreatePeluquero(int idpeluqueria, bool redirectToSeleccionar = false)
        {
            ViewData["IDPELUQUERIA"] = idpeluqueria;
            ViewData["REDIRECTTOSELECCIONAR"] = redirectToSeleccionar;

            var todosPeluqueros = await repoPelu.GetPeluquerosAsync();
            var peluquerosAsignados = await repoPelu.GetPeluquerosByIdPeluqueria(idpeluqueria);
            var idsAsignados = peluquerosAsignados.Select(p => p.IdUsuario).ToList();
            var peluquerosDisponibles = todosPeluqueros.Where(p => !idsAsignados.Contains(p.IdUsuario)).ToList();

            ViewData["USUARIOSPELUQUEROS"] = peluquerosDisponibles;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreatePeluquero(Peluquero peluquero, bool redirectToSeleccionar = false)
        {
            // Verificar que los IDs existan, pero NO asignar las entidades
            var usuario = await this.repoUsuario.FindUsuarioAsync(peluquero.IdUsuario);
            if (usuario == null)
            {
                ModelState.AddModelError("IdUsuario", "El usuario no existe");
                ViewData["IDPELUQUERIA"] = peluquero.IdPeluqueria;
                ViewData["REDIRECTTOSELECCIONAR"] = redirectToSeleccionar;
                ViewData["USUARIOSPELUQUEROS"] = await repoPelu.GetPeluquerosAsync();
                return View(peluquero);
            }

            var peluqueria = await this.repoPelu.FindPeluqueriaAsync(peluquero.IdPeluqueria);
            if (peluqueria == null)
            {
                ModelState.AddModelError("IdPeluqueria", "La peluquería no existe");
                ViewData["IDPELUQUERIA"] = peluquero.IdPeluqueria;
                ViewData["REDIRECTTOSELECCIONAR"] = redirectToSeleccionar;
                ViewData["USUARIOSPELUQUEROS"] = await repoPelu.GetPeluquerosAsync();
                return View(peluquero);
            }

            await this.repoPelu.InsertPeluqueroAsync(peluquero);

            // Si el parámetro redirectToSeleccionar es true, volvemos a SeleccionarPeluquero
            if (redirectToSeleccionar)
            {
                return RedirectToAction("SeleccionarPeluquero", new { idPeluqueria = peluquero.IdPeluqueria });
            }

            // Si no, seguimos con el flujo normal hacia CreateServicio
            return RedirectToAction("CreateServicio", new { idPeluqueria = peluquero.IdPeluqueria });
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
