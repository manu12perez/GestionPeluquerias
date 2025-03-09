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

        public async Task<IActionResult> Details(int id)
        {
            var peluqueria = await repoPelu.FindPeluqueriaAsync(id);
            if (peluqueria == null)
            {
                return NotFound();
            }

            var administrador = await repoUsuarios.FindUsuarioAsync(peluqueria.IdUsuario);

            return Json(new
            {
                direccion = peluqueria.Direccion,
                latitud = peluqueria.Latitud,
                longitud = peluqueria.Longitud,
                telefono = peluqueria.Telefono,
                horarioApertura = peluqueria.HorarioApertura.ToString(@"hh\:mm"),
                horarioCierre = peluqueria.HorarioCierre.ToString(@"hh\:mm"),
                administradorNombre = (administrador != null) ? administrador.Nombre : "Desconocido"
        });
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
            return RedirectToAction("Index");
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
