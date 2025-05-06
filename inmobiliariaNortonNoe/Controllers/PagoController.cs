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
        private readonly IRepositorioInmueble repositorioInmueble;
        private readonly IRepositorioInquilino repositorioInquilino;
        private readonly IRepositorioUsuario repoUsuario;

        public PagoController(IRepositorioPago repositorioPago, IRepositorioContrato repositorioContrato, IRepositorioUsuario repoUsuario)
        {
            this.repoUsuario = repoUsuario;
            this.repositorioPago = repositorioPago;
            this.repositorioContrato = repositorioContrato;
            this.repositorioInmueble = repositorioInmueble;
            this.repositorioInquilino = repositorioInquilino;
        }

        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult PagosPorContrato(int idContrato)
        {
            try
            {
                var pagos = repositorioPago.ObtenerPagosPorContrato(idContrato);
                ViewBag.IdContrato = idContrato;
                return View("PagosPorContrato", pagos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No se pudieron cargar los pagos.";
                return RedirectToAction("Details", "Contrato", new { id = idContrato });
            }
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult PagosDeBajaPorContrato(int idContrato)
        {
            try
            {
                var pagos = repositorioPago.ObtenerPagosDeBajaPorContrato(idContrato);
                ViewBag.IdContrato = idContrato;
                return View("PagosDeBajaPorContrato", pagos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No se pudieron cargar los pagos dados de baja.";
                return RedirectToAction("Details", "Contrato", new { id = idContrato });
            }
        }

        [Authorize(Roles = "Administrador , Inmobiliaria")]
        public IActionResult PagosPagadosPorContrato(int idContrato)
        {
            try
            {
                var pagos = repositorioPago.ObtenerPagosPagadosPorContrato(idContrato);
                ViewBag.IdContrato = idContrato;
                return View("PagosPagadosPorContrato", pagos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No se pudieron cargar los pagos abonados.";
                return RedirectToAction("Details", "Contrato", new { id = idContrato });
            }
        }
        [Authorize(Roles = "Administrador , Inmobiliaria")]
        public IActionResult PagosImpagosPorContrato(int idContrato)
        {
            try
            {
                var pagos = repositorioPago.ObtenerPagosImpagosPorContrato(idContrato);
                ViewBag.IdContrato = idContrato;
                return View("PagosImpagosPorContrato", pagos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No se pudieron cargar los pagos no abonados.";
                return RedirectToAction("Details", "Contrato", new { id = idContrato });
            }
        }

        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult Details(int id)
        {
            var entidad = repositorioPago.ObtenerPorId(id);
            if (entidad == null) return NotFound();

            var usuarioAlta = repoUsuario.ObtenerPorId(entidad.Id_UsuarioAlta);
            ViewBag.UsuarioEmailAlta = usuarioAlta?.Email ?? "No disponible";
            entidad.Contrato = repositorioContrato.ObtenerPorId(entidad.Id_Contrato);
            if (entidad.Contrato != null)
            {
                var inquilino = entidad.Contrato.Inquilino;
                ViewBag.InquilinoNombre = inquilino?.Nombre ?? "No disponible" + " " + (inquilino?.Apellido ?? "No disponible");
                var inmueble = entidad.Contrato.Inmueble;
                ViewBag.InmuebleDireccion = inmueble?.Direccion ?? "No disponible";
            }
            else
            {
                ViewBag.InquilinoNombre = null;
                ViewBag.InmuebleDireccion = null;
            }
            if (entidad.Id_UsuarioBaja != null)
            {
                var usuarioBaja = repoUsuario.ObtenerPorId(entidad.Id_UsuarioBaja.Value);
                ViewBag.UsuarioEmailBaja = usuarioBaja?.Email ?? "No disponible";
            }
            else
            {
                ViewBag.UsuarioEmailBaja = null;
            }
            return View(entidad);
        }

        // GET: Pago/Create
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult Create(int? idContrato)
        {
            if (!idContrato.HasValue)
            {
                TempData["Error"] = "Debe seleccionar un contrato válido.";
                return RedirectToAction("Index", "Contrato");
            }

            var pago = new Pago
            {
                Id_Contrato = idContrato.Value,
                Fecha_Pago = null,
                Periodo_Pago = DateTime.Today
            };

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
                    ModelState.AddModelError("", "Campos vacíos o incorrectos.");
                    return View(pago);
                }

                var email = User.Identity?.Name;
                var usuario = repoUsuario.ObtenerPorEmail(email);
                if (usuario == null)
                    return RedirectToAction("Login", "Usuarios");

                pago.Id_UsuarioAlta = usuario.Id;

                repositorioPago.Alta(pago, usuario.Id);

                TempData["Mensaje"] = "Pago creado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Campos vacíos o incorrectos.");
                    return View(pago);
                }
                ModelState.AddModelError("", "Error al crear el pago: " + ex.Message);
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

            var contratos = repositorioContrato.ObtenerTodos();
            ViewBag.Contratos = new SelectList(contratos, "ID_Contrato", "ID_Contrato", pago.Id_Contrato);

            return View(pago);
        }

        // POST: Pago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult Edit(int id, Pago pago)
        {
            if (id != pago.Id)
                return NotFound();

            try
            {
                if (ModelState.IsValid)
                {
                    repositorioPago.Modificacion(pago);
                    TempData["Mensaje"] = "Pago actualizado correctamente";
                    return RedirectToAction(nameof(PagosPorContrato), new { idContrato = pago.Id_Contrato });
                }

                var contratos = repositorioContrato.ObtenerTodos();
                ViewBag.Contratos = new SelectList(contratos, "ID_Contrato", "ID_Contrato", pago.Id_Contrato);
                return View(pago);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al actualizar el pago: " + ex.Message);
                var contratos = repositorioContrato.ObtenerTodos();
                ViewBag.Contratos = new SelectList(contratos, "ID_Contrato", "ID_Contrato", pago.Id_Contrato);
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
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult GenerarPagosPorContrato(int idContrato)
        {
            var contrato = repositorioContrato.ObtenerPorId(idContrato);
            if (contrato == null)
            {
                TempData["Error"] = "Contrato no encontrado";
                return RedirectToAction("PagosPorContrato", new { idContrato = contrato.ID_Contrato });
            }
            if (contrato.Estado != "Vigente")
            {
                TempData["Error"] = "El contrato no está en estado 'Vigente'. No se pueden generar pagos.";
                return RedirectToAction("PagosPorContrato", new { idContrato = contrato.ID_Contrato });
            }
                var pagosExistentes = repositorioPago.ObtenerPagosPorContrato(idContrato);
                var pagosNuevos = new List<Pago>();
                var fechaInicio = contrato.Fecha_Inicio;
                var fechaFin = contrato.Fecha_Fin;
                var montoMensual = contrato.Monto_Alquiler;

                var email = User.Identity.Name;
                var usuario = repoUsuario.ObtenerPorEmail(email);
                if (usuario == null)
                    return RedirectToAction("Login", "Usuarios");

                DateTime fechaActual = new DateTime(fechaInicio.Year, fechaInicio.Month, 1);
                while (fechaActual <= fechaFin)
                {
                    if (!pagosExistentes.Any(p => p.Periodo_Pago.Year == fechaActual.Year && p.Periodo_Pago.Month == fechaActual.Month))
                    {
                        var nuevoPago = new Pago
                        {
                            Id_Contrato = idContrato,
                            Fecha_Pago = null,
                            Periodo_Pago = fechaActual,
                            Monto = montoMensual,
                            Pagado = false,
                            EsMulta = false,
                            Descripcion = null,
                            Estado = 1,
                            Id_UsuarioAlta = usuario.Id
                        };
                        repositorioPago.Alta(nuevoPago, usuario.Id);
                        pagosNuevos.Add(nuevoPago);
                    }
                    fechaActual = fechaActual.AddMonths(1);
                }

                TempData["Mensaje"] = $"Se generaron {pagosNuevos.Count} pagos nuevos para el contrato.";
                return RedirectToAction("PagosPorContrato", new { idContrato });
            
        }
    }
}
