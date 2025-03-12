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
            await this.repoPelu.InsertCitaAsync(cita);
            return RedirectToAction("Index", "Peluqueria");
        }
    }
}
