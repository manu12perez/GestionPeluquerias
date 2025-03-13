using GestionPeluquerias.Filters;
using GestionPeluquerias.Interfaces;
using GestionPeluquerias.Models;
using GestionPeluquerias.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionPeluquerias.Controllers
{
    public class UsuariosController : Controller
    {
        private IRepositoryUsuarios repoUsuarios;

        public UsuariosController(IRepositoryUsuarios repoUsuarios)
        {
            this.repoUsuarios = repoUsuarios;
        }

        [CustomAuthorize(Policy = "AdminOnly")]
        public async Task<IActionResult> MostrarUsuarios()
        {
            var usuarios = await repoUsuarios.GetUsuariosAsync();
            return View(usuarios);
        }

        public async Task<IActionResult> EditarUsuario(int id)
        {
            Usuario usuario = await repoUsuarios.FindUsuarioAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> EditarUsuario(Usuario usuario)
        {
            await repoUsuarios.UpdateUsuarioAsync(usuario);
            return RedirectToAction("MostrarUsuarios");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await repoUsuarios.FindUsuarioAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            bool deleted = await repoUsuarios.DeleteUsuarioAsync(id);
            if (!deleted)
            {
                return BadRequest("No se pudo eliminar la peluquería.");
            }

            return RedirectToAction("MostrarUsuarios");
        }
    }
}
