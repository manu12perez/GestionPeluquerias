using System.Security.Claims;
using GestionPeluquerias.Interfaces;
using GestionPeluquerias.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionPeluquerias.Controllers
{
    public class CitasController : Controller
    {

        private IRepositoryPeluquerias repoPelu;
        private IRepositoryUsuarios repoUsuarios;

        public CitasController(IRepositoryPeluquerias repoPeluquerias, IRepositoryUsuarios repoUsuarios)
        {
            this.repoPelu = repoPeluquerias;
            this.repoUsuarios = repoUsuarios;
        }


        public async Task<IActionResult> AgregarCita(int idpeluqueria, int idservicio)
        {
            ViewData["IDPELUQUERIA"] = idpeluqueria;
            ViewData["IDSERVICIO"] = idservicio;
            List<Peluquero> peluqueros = await this.repoPelu.GetPeluquerosByIdPeluqueria(idpeluqueria);
            ViewData["PELUQUEROS"] = peluqueros;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AgregarCita(Cita cita)
        {
            var idUsuario = GetUserId();
            cita.IdUsuario = idUsuario;
            await this.repoPelu.InsertCitaAsync(cita);
            return RedirectToAction("Index", "Peluqueria");
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(userIdClaim) ? 0 : int.Parse(userIdClaim);
        }

        public async Task<IActionResult> Citas()
        {
            var idusuario = GetUserId();
            List<Cita> citas = new List<Cita>();

            var rol = HttpContext.Session.GetString("Rol");
            if(rol == "Peluquero")
            {
                citas = await this.repoPelu.GetCitasPeluqeros(idusuario);
            }
            else
            {
                citas = await this.repoPelu.GetCitasByIdAsync(idusuario);
            }
            return View(citas);
        }

        public async Task<IActionResult> ListadoCitas()
        {
            var idusuario = GetUserId();
            List<Cita> citas = new List<Cita>();

            var rol = HttpContext.Session.GetString("Rol");
            if (rol == "Peluquero")
            {
                citas = await this.repoPelu.GetCitasPeluqeros(idusuario);
            }
            else
            {
                citas = await this.repoPelu.GetCitasByIdAsync(idusuario);
            }
            return View(citas);
        }
    }
}
