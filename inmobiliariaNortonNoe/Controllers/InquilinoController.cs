using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inmobiliariaNortonNoe.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;

namespace inmobiliariaNortonNoe.Controllers
{
	public class InquilinoController : Controller
	{
		private readonly IRepositorioInquilino repositorio;
		public InquilinoController(IRepositorioInquilino repo)
		{
			this.repositorio = repo;
		}

		// GET: Inquilino
		[Route("[controller]/Index")]
		public ActionResult Index()
		{
			try
			{
				var lista = repositorio.ObtenerTodos();
				ViewBag.Id = TempData["Id"];
				if (TempData.ContainsKey("Mensaje"))
					ViewBag.Mensaje = TempData["Mensaje"];
				return View(lista);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		// GET: Inquilino
		[Route("[controller]/Lista")]
		public ActionResult Lista(int pagina=1)
		{
			try
			{
				var tamaño = 5;
				var lista = repositorio.ObtenerLista(Math.Max(pagina, 1), tamaño);
				ViewBag.Pagina = pagina;
				var total = repositorio.ObtenerCantidad();
				ViewBag.TotalPaginas = total % tamaño == 0 ? total / tamaño : total / tamaño + 1;
				
				ViewBag.Id = TempData["Id"];
				if (TempData.ContainsKey("Mensaje"))
					ViewBag.Mensaje = TempData["Mensaje"];
				return View(lista);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		// GET: Inquilino/Details/5
		public ActionResult Details(int id)
		{
			try
			{
				var entidad = repositorio.ObtenerPorId(id);
				return View(entidad);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		// GET: Inquilino/Busqueda
		public IActionResult Busqueda()
		{
			try
			{
				return View();
			}
			catch (Exception ex)
			{//poner breakpoints para detectar errores
				throw;
			}
		}

		// GET: Inquilino/Buscar/5
		[Route("[controller]/Buscar/{q}", Name = "BuscarInquilino")]
		public IActionResult Buscar(string q)
		{
			try
			{
				var res = repositorio.BuscarPorNombre(q);
				return Json(new { Datos = res });
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}

		// GET: Inquilino/Create
		public ActionResult Create()
		{
			try
			{
				return View();
			}
			catch (Exception ex)
			{//poner breakpoints para detectar errores
				throw;
			}
		}

		// POST: Inquilino/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Inquilino Inquilino)
		{
			try
			{
				if (ModelState.IsValid)// Pregunta si el modelo es válido
				{
					// Reemplazo de clave plana por clave con hash
					repositorio.Alta(Inquilino);
					TempData["Id"] = Inquilino.Id;
					return RedirectToAction(nameof(Index));
				}
				else
					return View(Inquilino);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		// GET: Inquilino/Edit/5
		public ActionResult Edit(int id)
		{
			try
			{
				var entidad = repositorio.ObtenerPorId(id);
				return View(entidad);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		// POST: Inquilino/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Inquilino entidad)
		{
			// Si en lugar de IFormCollection ponemos Inquilino, el enlace de datos lo hace el sistema
			Inquilino p = null;
			try
			{
				p = repositorio.ObtenerPorId(id);
				// En caso de ser necesario usar: 
				//
				//Convert.ToInt32(collection["CAMPO"]);
				//Convert.ToDecimal(collection["CAMPO"]);
				//Convert.ToDateTime(collection["CAMPO"]);
				//int.Parse(collection["CAMPO"]);
				//decimal.Parse(collection["CAMPO"]);
				//DateTime.Parse(collection["CAMPO"]);
				////////////////////////////////////////
				p.Nombre = entidad.Nombre;
				p.Apellido = entidad.Apellido;
				p.Dni = entidad.Dni;
				p.Email = entidad.Email;
				p.Telefono = entidad.Telefono;
				repositorio.Modificacion(p);
				TempData["Mensaje"] = "Datos guardados correctamente";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{//poner breakpoints para detectar errores
				throw;
			}
		}
		// GET: Inquilino/Delete/5
		public ActionResult Eliminar(int id)
		{
			try
			{
				var entidad = repositorio.ObtenerPorId(id);
				return View(entidad);
			}
			catch (Exception ex)
			{//poner breakpoints para detectar errores
				throw;
			}
		}

		// POST: Inquilino/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Eliminar(int id, Inquilino entidad)
		{
			try
			{
				repositorio.Baja(id);
				TempData["Mensaje"] = "Eliminación realizada correctamente";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{//poner breakpoints para detectar errores
				throw;
			}
		}
	}
}