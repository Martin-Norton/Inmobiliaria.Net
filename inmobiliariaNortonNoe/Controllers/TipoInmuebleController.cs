using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using inmobiliariaNortonNoe.Models;
using System;
using System.Collections.Generic;

namespace inmobiliariaNortonNoe.Controllers
{
    [Authorize]
    public class TipoInmuebleController : Controller
    {
        private readonly IRepositorioTipoInmueble repositorio;

        public TipoInmuebleController(IRepositorioTipoInmueble repositorio)
        {
            this.repositorio = repositorio;
        }

        public IActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        public IActionResult Details(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            if (entidad == null) return NotFound();
            return View(entidad);
        }

        [Authorize(Roles = "Administrador, Inmobiliaria")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Inmobiliaria")]
        public IActionResult Create(TipoInmueble tipo)
        {
            try
            {
                if (!ModelState.IsValid) return View(tipo);
                repositorio.Alta(tipo);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(tipo);
            }
        }

        [Authorize(Roles = "Administrador, Inmobiliaria")]
        public IActionResult Edit(int id)
        {
            var tipo = repositorio.ObtenerPorId(id);
            if (tipo == null) return NotFound();
            return View(tipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Inmobiliaria")]
        public IActionResult Edit(int id, TipoInmueble tipo)
        {
            try
            {
                if (id != tipo.Id) return NotFound();
                repositorio.Modificacion(tipo);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(tipo);
            }
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var tipo = repositorio.ObtenerPorId(id);
            if (tipo == null) return NotFound();
            return View(tipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id, TipoInmueble tipo)
        {
            try
            {
                repositorio.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(tipo);
            }
        }
    }
}
