using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using inmobiliariaNortonNoe.Models;

namespace inmobiliariaNortonNoe.Controllers
{
    public class ContratoController : Controller
    {
        private readonly IRepositorioContrato repositorio;

        public ContratoController(IRepositorioContrato repo)
        {
            this.repositorio = repo;
        }

        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        public ActionResult Lista(int pagina = 1)
        {
            int tamaño = 5;
            var lista = repositorio.ObtenerLista(Math.Max(pagina, 1), tamaño);
            ViewBag.Pagina = pagina;
            int total = repositorio.ObtenerCantidad();
            ViewBag.TotalPaginas = total % tamaño == 0 ? total / tamaño : total / tamaño + 1;
            return View(lista);
        }

        public ActionResult Details(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            return View(entidad);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            if (!ModelState.IsValid) return View(contrato);

            repositorio.Alta(contrato);
            TempData["Mensaje"] = "Contrato creado correctamente";
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            return View(entidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato entidad)
        {
            if (!ModelState.IsValid) return View(entidad);

            var p = repositorio.ObtenerPorId(id);
            if (p == null) return NotFound();

            p.Fecha_Inicio = entidad.Fecha_Inicio;
            p.Fecha_Fin = entidad.Fecha_Fin;
            p.Monto_Alquiler = entidad.Monto_Alquiler;
            p.Multa = entidad.Multa;
            p.Estado = entidad.Estado;
            repositorio.Modificacion(p);
            TempData["Mensaje"] = "Datos actualizados correctamente";
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Eliminar(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            return View(entidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, Contrato entidad)
        {
            repositorio.Baja(id);
            TempData["Mensaje"] = "Contrato eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }

        [Route("[controller]/ObtenerPorInmueble/{id}")]
        public IActionResult ObtenerPorInmueble(int id)
        {
            var res = repositorio.ObtenerPorInmueble(id);
            return Json(new { Datos = res });
        }

        [Route("[controller]/ObtenerVigentes")]
        public IActionResult ObtenerVigentes()
        {
            var res = repositorio.ObtenerVigentes();
            return Json(new { Datos = res });
        }

        [Route("[controller]/ObtenerPorInquilino/{id}")]
        public IActionResult ObtenerPorInquilino(int id)
        {
            var res = repositorio.ObtenerPorInquilino(id);
            return Json(new { Datos = res });
        }
    }
}
