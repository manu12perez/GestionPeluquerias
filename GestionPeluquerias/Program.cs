using Microsoft.EntityFrameworkCore;
using GestionPeluquerias.Data;
using GestionPeluquerias.Repositories;
using GestionPeluquerias.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

/*****************************************************************************************************************************************/
// Add services to the container.
builder.Services.AddControllersWithViews(options => options.EnableEndpointRouting = false).AddSessionStateTempDataProvider();
//Las politicas se agregan a Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Configuración de SQL Server
string connectionString = builder.Configuration.GetConnectionString("SqlPeluqueria");
builder.Services.AddDbContext<PeluqueriaContext>(options => options.UseSqlServer(connectionString));

// Registro de repositorios
builder.Services.AddTransient<IRepositoryPeluquerias, RepositoryPeluquerias>();
builder.Services.AddTransient<IRepositoryUsuarios, RepositoryUsuarios>();

//Añadimos Session y memoria distribuida
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

//Añadimos authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(
    CookieAuthenticationDefaults.AuthenticationScheme,
    config =>
    {
        config.AccessDeniedPath = "/Managed/ErrorAcceso";
    });
/*****************************************************************************************************************************************/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 días. Puedes modificarlo según el entorno de producción.
    app.UseHsts();
}

app.UseHttpsRedirection();
/*****************************************************************************************************************************************/
//app.UseRouting();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

//app.MapStaticAssets();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}")
//    .WithStaticAssets();
app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");
});
/*****************************************************************************************************************************************/
app.Run();
