
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using inmobiliariaNortonNoe.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IRepositorioPropietario, RepositorioPropietario>();

builder.Services.AddScoped<IRepositorioInquilino, RepositorioInquilino>();

builder.Services.AddScoped<IRepositorioInmueble, RepositorioInmueble>();

builder.Services.AddScoped<IRepositorioContrato, RepositorioContrato>();    

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
