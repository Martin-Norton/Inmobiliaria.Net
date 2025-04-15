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

        public IActionResult PagosPorContrato(int idContrato)
        {
            var pagos = repositorioPago.ObtenerPagosPorContrato(idContrato);
            ViewBag.IdContrato = idContrato;
            return View(pagos);
        }

        public IActionResult Create2(int? idContrato)
        {
            var contratos = repositorioContrato.ObtenerTodos();
            ViewBag.Contratos = new SelectList(contratos, "ID_Contrato", "ID_Contrato");

            var pago = new Pago();
            if (idContrato.HasValue)
            {
                pago.Id_Contrato = idContrato.Value;
            }

            return View(pago);
        }

        // GET: Pago/Create
        public IActionResult Create(int? idContrato)
        {
            var contratos = repositorioContrato.ObtenerTodos();
            ViewBag.Contratos = new SelectList(contratos, "ID_Contrato", "ID_Contrato");

            var pago = new Pago();
            if (idContrato.HasValue)
            {
                pago.Id_Contrato = idContrato.Value;
            }

            return View(pago);
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
                    repositorioPago.Alta(pago);
                    return RedirectToAction(nameof(Index));
                }

                var contratos = repositorioContrato.ObtenerTodos();
                ViewBag.Contratos = new SelectList(contratos, "ID_Contrato", "ID_Contrato");
                return View(pago);
            }
            catch
            {
                var contratos = repositorioContrato.ObtenerTodos();
                ViewBag.Contratos = new SelectList(contratos, "ID_Contrato", "ID_Contrato");
                return View(pago);
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
                return RedirectToAction("PagosPorContrato", new { idContrato = pago.Id_Contrato });
            }
            catch
            {
                return View(pago);
            }
        }
    }
}
