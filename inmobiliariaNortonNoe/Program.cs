
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using inmobiliariaNortonNoe.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/Usuarios/Login";
		options.LogoutPath = "/Usuarios/Logout";
		options.AccessDeniedPath = "/Home/Restringido"; 
	});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Administrador", policy => policy.RequireRole(nameof(enRoles.Administrador)));
	options.AddPolicy("Inmobiliaria", policy => policy.RequireRole(nameof(enRoles.Inmobiliaria), nameof(enRoles.Administrador)));

});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IRepositorioPropietario, RepositorioPropietario>();

builder.Services.AddScoped<IRepositorioInquilino, RepositorioInquilino>();

builder.Services.AddScoped<IRepositorioInmueble, RepositorioInmueble>();

builder.Services.AddScoped<IRepositorioContrato, RepositorioContrato>();    

builder.Services.AddScoped<IRepositorioPago, RepositorioPago>();

builder.Services.AddScoped<IRepositorioImagen, RepositorioImagen>();

builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
