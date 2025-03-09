using Microsoft.EntityFrameworkCore;
using GestionPeluquerias.Data;
using GestionPeluquerias.Repositories;
using GestionPeluquerias.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

/*****************************************************************************************************************************************/

// Configuración de SQL Server
string connectionString = builder.Configuration.GetConnectionString("SqlPeluqueria");
builder.Services.AddDbContext<PeluqueriaContext>(options => options.UseSqlServer(connectionString));

// Registro de repositorios
builder.Services.AddTransient<IRepositoryPeluquerias, RepositoryPeluquerias>();
builder.Services.AddTransient<IRepositoryUsuarios, RepositoryUsuarios>();

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
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Peluqueria}/{action=Index}/{id?}");

app.Run();
