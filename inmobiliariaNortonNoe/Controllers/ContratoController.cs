using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inmobiliariaNortonNoe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace inmobiliariaNortonNoe.Controllers
{
    public class ContratoController : Controller
    {
        private readonly IRepositorioContrato repositorio;

        public ContratoController(IRepositorioContrato repo)
        {
            this.repositorio = repo;
        }

        // GET: Contrato
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

        // GET: Contrato/Details/5
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

        // GET: Contrato/Create
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // POST: Contrato/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato entidad)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(entidad);
                }

                repositorio.Alta(entidad);
                TempData["Mensaje"] = "Contrato creado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al guardar los datos.");
                return View(entidad);
            }
        }

        // GET: Contrato/Edit/5
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

        // POST: Contrato/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato entidad)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(entidad);
                }

                Contrato c = repositorio.ObtenerPorId(id);
                if (c == null)
                {
                    return NotFound();
                }

                c.InmuebleId = entidad.InmuebleId;
                c.LocatarioId = entidad.LocatarioId;
                c.FechaInicio = entidad.FechaInicio;
                c.FechaFin = entidad.FechaFin;
                c.MontoAlquiler = entidad.MontoAlquiler;

                repositorio.Modificacion(c);
                TempData["Mensaje"] = "Datos guardados correctamente";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al guardar los datos.");
                return View(entidad);
            }
        }

        // GET: Contrato/Delete/5
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

        // POST: Contrato/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, Contrato entidad)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
