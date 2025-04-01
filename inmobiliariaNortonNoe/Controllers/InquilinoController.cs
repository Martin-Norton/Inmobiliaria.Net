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
		public ActionResult Lista(int pagina = 1)
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
				if (!ModelState.IsValid)
				{
					return View(Inquilino);
				}

				repositorio.Alta(Inquilino);
				TempData["Id"] = Inquilino.Id;
				TempData["Mensaje"] = "Propietario creado correctamente";
				return RedirectToAction(nameof(Index));

			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "Ocurrió un error al guardar los datos.");
				return View(Inquilino);
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
			try
			{
				if (!ModelState.IsValid)
				{
					return View(entidad);
				}

				Inquilino p = repositorio.ObtenerPorId(id);
				if (p == null)
				{
					return NotFound();
				}
				p = repositorio.ObtenerPorId(id);
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
			{
				ModelState.AddModelError("", "Ocurrió un error al guardar los datos.");
				return View(entidad);
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
			{
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