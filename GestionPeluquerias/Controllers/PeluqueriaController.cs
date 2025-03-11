using Microsoft.AspNetCore.Mvc;
using GestionPeluquerias.Models;
using GestionPeluquerias.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionPeluquerias.Controllers
{
    public class PeluqueriaController : Controller
    {
        private IRepositoryPeluquerias repoPelu;
        private IRepositoryUsuarios repoUsuarios; // Para obtener la lista de administradores

        public PeluqueriaController(IRepositoryPeluquerias repoPeluquerias, IRepositoryUsuarios repoUsuarios)
        {
            this.repoPelu = repoPeluquerias;
            this.repoUsuarios = repoUsuarios;
        }

        // Mostrar lista de peluquerías
        public async Task<IActionResult> Index()
        {
            List<Peluqueria> peluquerias = await repoPelu.GetPeluqueriasAsync();
            return View(peluquerias);
        }

        public async Task<IActionResult> Detalles(int id)
        {
            var detalles = await repoPelu.GetPeluqueriaDetallesAsync(id);
            if (detalles == null || !detalles.Any())
            {
                return Json(new { error = "No hay información disponible para esta peluquería." });
            }

            // Opcionalmente, puedes procesar los datos antes de enviarlos
            // para manejar valores nulos con valores predeterminados
            var resultado = detalles.Select(d => new {
                d.Id,
                d.IdPeluqueria,
                d.NombrePeluqueria,
                d.Direccion,
                d.Telefono,
                d.HorarioApertura,
                d.HorarioCierre,
                d.NombreAdministrador,
                IdPeluquero = d.IdPeluquero ?? 0,
                NombrePeluquero = d.NombrePeluquero ?? "No asignado",
                IdServicio = d.IdServicio ?? 0,
                NombreServicio = d.NombreServicio ?? "No disponible",
                Descripcion = string.IsNullOrEmpty(d.Descripcion) ? "Sin descripción" : d.Descripcion,
                PrecioServicio = d.PrecioServicio ?? 0,
                d.Duracion
            });

            return Json(resultado);
        }



        // Vista para crear una nueva peluquería
        public async Task<IActionResult> Create()
        {
            ViewBag.Administradores = await GetAdministradoresSelectList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Peluqueria peluqueria)
        {
            await this.repoPelu.InsertPeluqueriaAsync(peluqueria);
            return RedirectToAction("CreatePeluquero", "Reserva", new { idpeluqueria = peluqueria.IdPeluqueria });
        }


        // Vista para editar una peluquería
        public async Task<IActionResult> Edit(int id)
        {
            Peluqueria peluqueria = await repoPelu.FindPeluqueriaAsync(id);
            if (peluqueria == null)
            {
                return NotFound();
            }

            ViewBag.Administradores = await GetAdministradoresSelectList();
            return View(peluqueria);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Peluqueria peluqueria)
        {
            await repoPelu.UpdatePeluqueriaAsync(peluqueria);
            return RedirectToAction("Index");
            
        }

        // Método para eliminar una peluquería
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var peluqueria = await repoPelu.FindPeluqueriaAsync(id);
            if (peluqueria == null)
            {
                return NotFound();
            }

            bool deleted = await repoPelu.DeletePeluqueriaAsync(id);
            if (!deleted)
            {
                return BadRequest("No se pudo eliminar la peluquería.");
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Obtiene la lista de administradores para un SelectList en la vista.
        /// </summary>
        private async Task<SelectList> GetAdministradoresSelectList()
        {
            List<Usuario> administradores = await repoUsuarios.GetAdministradoresAsync();
            return new SelectList(administradores, "IdUsuario", "Nombre");
        }
    }
}
