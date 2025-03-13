using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GestionPeluquerias.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var routeValues = context.RouteData.Values;

            string controller = routeValues["controller"]?.ToString();
            string action = routeValues["action"]?.ToString();

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = this.GetRoute("Managed", "Login");
            }
            else if (controller == "Managed" && (action == "Login" || action == "Register"))
            {
                context.Result = this.GetRoute("Usuario", "Index");
            }
        }


        //Como tenderemos multiples redirecciones nos crearemos un metodo para
        //facilitar el codigo

        private RedirectToRouteResult GetRoute(string controller, string action)
        {
            RouteValueDictionary ruta = new RouteValueDictionary(
                new { controller = controller, action = action }
            );
            RedirectToRouteResult result = new RedirectToRouteResult(ruta);
            return result;
        }
    }
}