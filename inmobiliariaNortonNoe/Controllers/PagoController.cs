using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using inmobiliariaNortonNoe.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace inmobiliariaNortonNoe.Controllers
{
    public class PagoController : Controller
    {
        private readonly IRepositorioPago repositorioPago;
        private readonly IRepositorioContrato repositorioContrato;

        public PagoController(IRepositorioPago repositorioPago, IRepositorioContrato repositorioContrato)
        {
            this.repositorioPago = repositorioPago;
            this.repositorioContrato = repositorioContrato;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Pago/Details/5
        public IActionResult Details(int id)
        {
            var pago = repositorioPago.ObtenerPorId(id);
            if (pago == null)
            {
                return NotFound();
            }
            return View(pago);
        }

        // GET: Pago/Create
        public IActionResult Create()
        {
            ViewBag.Contratos = repositorioContrato.ObtenerTodos();
            return View();
        }

        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pago pago)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Console.WriteLine("Modelo válido"); 
                    repositorioPago.Alta(pago);

                    return RedirectToAction(nameof(Index));
                }
                
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Pago/Edit/5
        public IActionResult Edit(int id)
        {
            var pago = repositorioPago.ObtenerPorId(id);
            if (pago == null)
            {
                return NotFound();
            }
            ViewBag.Contratos = repositorioContrato.ObtenerTodos();
            return View(pago);
        }

        // POST: Pago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Pago pago)
        {
            try
            {
                if (id != pago.Id)
                    return NotFound();

                if (ModelState.IsValid)
                {
                    repositorioPago.Modificacion(pago);
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Contratos = repositorioContrato.ObtenerTodos();
                return View(pago);
            }
            catch
            {
                ViewBag.Contratos = repositorioContrato.ObtenerTodos();
                return View(pago);
            }
        }

        // GET: Pago/Delete/5
        public IActionResult Delete(int id)
        {
            var pago = repositorioPago.ObtenerPorId(id);
            if (pago == null)
            {
                return NotFound();
            }
            return View(pago);
        }

        // POST: Pago/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Pago pago)
        {
            try
            {
                repositorioPago.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(pago);
            }
        }
    }
}
