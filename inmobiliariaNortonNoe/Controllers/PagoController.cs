using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using inmobiliariaNortonNoe.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;


namespace inmobiliariaNortonNoe.Controllers
{
    public class PagoController : Controller
    {
        private readonly IRepositorioPago repositorioPago;
        private readonly IRepositorioContrato repositorioContrato;
        private readonly IRepositorioUsuario repoUsuario;

        public PagoController(IRepositorioPago repositorioPago, IRepositorioContrato repositorioContrato, IRepositorioUsuario repoUsuario)
        {
            this.repoUsuario = repoUsuario;
            this.repositorioPago = repositorioPago;
            this.repositorioContrato = repositorioContrato;
        }

        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult PagosPorContrato(int idContrato)
        {
            var pagos = repositorioPago.ObtenerPagosPorContrato(idContrato);
            ViewBag.IdContrato = idContrato;
            return View(pagos);
        }
        [Authorize(Roles = "Inmobiliaria, Administrador")]
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
        [Authorize(Roles = "Inmobiliaria, Administrador")]
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
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult Create(Pago pago)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Campos vacíos o incorrectos");
                    Console.WriteLine("Error en el modelo");
                    var contratos = repositorioContrato.ObtenerTodos();
                    ViewBag.Contratos = new SelectList(contratos, "ID_Contrato", "ID_Contrato");
                    return View(pago);
                }

                var email = User.Identity.Name;
                var usuario = repoUsuario.ObtenerPorEmail(email);
                Console.WriteLine("Usuario: " + usuario);
                if (usuario == null)
                    return RedirectToAction("Login", "Usuarios");

                pago.Id_UsuarioAlta = usuario.Id;
                Console.WriteLine("EL pago tiene cargado al usuario: " + pago.Id_UsuarioAlta);
                repositorioPago.Alta(pago, usuario.Id);

                TempData["Mensaje"] = "Pago creado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar el pago. enviado desde el catch");
                ModelState.AddModelError("", "Error al crear el pago: " + ex.Message);
                var contratos = repositorioContrato.ObtenerTodos();
                ViewBag.Contratos = new SelectList(contratos, "ID_Contrato", "ID_Contrato");
                return View(pago);
            }
        }


        // GET: Pago/Edit/5
        [Authorize(Roles = "Inmobiliaria, Administrador")]
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
        [Authorize(Roles = "Inmobiliaria, Administrador")]
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
        [Authorize(Roles = "Inmobiliaria, Administrador")]
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
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult Delete(int id, Pago pago)
        {
            try
            {
                var email = User.Identity.Name;
                var usuario = repoUsuario.ObtenerPorEmail(email);
                Console.WriteLine("Desde delete pago controll. Usuario: " + usuario);

                if (usuario == null)
                return RedirectToAction("Login", "Usuarios");

                repositorioPago.Baja(id, usuario.Id);

                return RedirectToAction("PagosPorContrato", new { idContrato = pago.Id_Contrato });
            }
            catch
            {
                Console.WriteLine("Error al eliminar el pago. enviado desde el catch");
                return View(pago);
            }
        }
    }
}
