using System.Security.Claims;
using GestionPeluquerias.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionPeluquerias.Models;
using GestionPeluquerias.Interfaces;

namespace GestionPeluquerias.Controllers
{
    public class ManagedController : Controller
    {
        private IRepositoryUsuarios repoUsuarios;

        public ManagedController(IRepositoryUsuarios repoUsuarios)
        {
            this.repoUsuarios = repoUsuarios;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var usuario = await repoUsuarios.GetUsuariosAsync();
            var user = usuario.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View();
            }

            // Guardar datos del usuario en la sesión
            HttpContext.Session.SetInt32("IdUsuario", user.IdUsuario);
            HttpContext.Session.SetString("Nombre", user.Nombre);
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("Rol", user.Rol.Nombre);

            // Crear Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nombre),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Rol.Nombre),
                new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Peluqueria");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Usuario usuario, int rolusuario)
        {
            
            usuario.IdRol = rolusuario;
            await repoUsuarios.InsertUsuarioAsync(usuario);
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            // Limpiar la sesión antes de cerrar sesión
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult VerificarSession()
        {
            if (HttpContext.Session.GetInt32("IdUsuario") != null)
            {
                return Content($"Usuario logueado: {HttpContext.Session.GetString("Nombre")} {HttpContext.Session.GetString("Apellido")} - Rol: {HttpContext.Session.GetString("Rol")}");
            }
            else
            {
                return Content("No hay usuario en sesión.");
            }
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }

    }
}
