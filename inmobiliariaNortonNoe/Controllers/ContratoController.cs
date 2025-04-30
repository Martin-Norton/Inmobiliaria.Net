using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using inmobiliariaNortonNoe.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace inmobiliariaNortonNoe.Controllers
{
    public class ContratoController : Controller
    {
        private readonly IRepositorioInmueble repoInmueble;
        private readonly IRepositorioInquilino repoInquilino;
        private readonly IRepositorioContrato repositorio;
        private readonly IRepositorioUsuario repoUsuario;
        private readonly IRepositorioPago repositorioPago;

        public ContratoController(IRepositorioContrato repo, IRepositorioInmueble repoInmueble, IRepositorioInquilino repoInquilino, IRepositorioUsuario repoUsuario, IRepositorioPago repositorioPago)
        {
            this.repositorio = repo;
            this.repoInmueble = repoInmueble;
            this.repoInquilino = repoInquilino;
            this.repoUsuario = repoUsuario;
            this.repositorioPago = repositorioPago;
        }

        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult Lista(int pagina = 1)
        {
            int tamaño = 5;
            var lista = repositorio.ObtenerLista(Math.Max(pagina, 1), tamaño);
            ViewBag.Pagina = pagina;
            int total = repositorio.ObtenerCantidad();
            ViewBag.TotalPaginas = total % tamaño == 0 ? total / tamaño : total / tamaño + 1;
            return View(lista);
        }
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult Details(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            if (entidad == null) return NotFound();

            var usuarioAlta = repoUsuario.ObtenerPorId(entidad.ID_UsuarioAlta);
            ViewBag.UsuarioEmailAlta = usuarioAlta?.Email ?? "No disponible";

            if (entidad.ID_UsuarioBaja != null)
            {
                var usuarioBaja = repoUsuario.ObtenerPorId(entidad.ID_UsuarioBaja.Value);
                ViewBag.UsuarioEmailBaja = usuarioBaja?.Email ?? "No disponible";
            }
            else
            {
                ViewBag.UsuarioEmailBaja = null;
            }
            return View(entidad);
        }
    //zona Busquedas
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult Busqueda(DateTime fechaInicio, DateTime fechaFin)
        {
            var lista = repositorio.ObtenerPorFechas(fechaInicio,fechaFin);
            return View(lista);
        }
        
        [HttpGet]
        public IActionResult BuscarPorInmueble()
        {
            var listaInmuebles = repoInmueble.ObtenerTodos();
            ViewBag.Inmuebles = listaInmuebles;
            return View();
        }

        [HttpPost]
        public IActionResult BuscarPorInmueble(int idInmueble)
        {
            var listaInmuebles = repoInmueble.ObtenerTodos();
            ViewBag.Inmuebles = listaInmuebles;
            
            var contratos = repositorio.ObtenerPorInmueble(idInmueble);
            return View(contratos);
        }
    //FinZona Busquedas

    //zona contratos baja
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult IndexBajas()
        {
            var lista = repositorio.ObtenerTodosBaja();
            return View(lista);
        }
    //fin zona contratos baja
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult Create()
        {
            var listaInmuebles = repoInmueble.ObtenerTodos();
            var listaInquilinos = repoInquilino.ObtenerTodos();

            if (listaInmuebles == null || listaInmuebles.Count == 0)
            {
                TempData["Mensaje"] = "No hay inmuebles disponibles.";
                return RedirectToAction("Index");
            }
            if (listaInquilinos == null || listaInquilinos.Count == 0)
            {
                TempData["Mensaje"] = "No hay inquilinos disponibles.";
                return RedirectToAction("Index");
            }

            ViewBag.inmuebles = new SelectList(listaInmuebles, "Id", "Direccion");
            ViewBag.Inquilinos = new SelectList(listaInquilinos, "Id", "Nombre");

            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];

            return View(new Contrato());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Campos vacios o incorrectos");
                    ViewBag.inmuebles = new SelectList(repoInmueble.ObtenerTodos(), "Id", "Direccion");
                    ViewBag.Inquilinos = new SelectList(repoInquilino.ObtenerTodos(), "Id", "Nombre");
                    return View(contrato);
                }

                if (repositorio.ExisteContratoSuperpuesto(contrato.ID_Inmueble, contrato.Fecha_Inicio, contrato.Fecha_Fin))
                {
                    ModelState.AddModelError("", "Ya existe un contrato para este inmueble en las fechas seleccionadas.");
                    ViewBag.inmuebles = new SelectList(repoInmueble.ObtenerTodos(), "Id", "Direccion");
                    ViewBag.Inquilinos = new SelectList(repoInquilino.ObtenerTodos(), "Id", "Nombre");
                    return View(contrato);
                }

                var email = User.Identity.Name;
                var usuario = repoUsuario.ObtenerPorEmail(email);

                if (usuario == null)
                    return RedirectToAction("Login", "Usuarios");

                contrato.ID_UsuarioAlta = usuario.Id;

                repositorio.Alta(contrato, usuario.Id);

                TempData["Mensaje"] = "Contrato creado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear el contrato: " + ex.Message);
                ViewBag.inmuebles = new SelectList(repoInmueble.ObtenerTodos(), "Id", "Direccion");
                ViewBag.Inquilinos = new SelectList(repoInquilino.ObtenerTodos(), "Id", "Nombre");
                return View(contrato);
            }
        }


        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult Edit(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            return View(entidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult Edit(int id, Contrato entidad)
        {
            if (!ModelState.IsValid) return View(entidad);

            if (repositorio.ExisteContratoSuperpuestoE(entidad.ID_Inmueble, entidad.Fecha_Inicio, entidad.Fecha_Fin))
            {
                ModelState.AddModelError("", "Ya existe un contrato para este inmueble en las fechas seleccionadas.");
                ViewBag.inmuebles = new SelectList(repoInmueble.ObtenerTodos(), "Id", "Direccion");
                ViewBag.Inquilinos = new SelectList(repoInquilino.ObtenerTodos(), "Id", "Nombre");
                return View(entidad);
            }

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

        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult Eliminar(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            return View(entidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult Eliminar(int id, Contrato entidad)
        {
            var email = User.Identity.Name;
            var usuario = repoUsuario.ObtenerPorEmail(email);

            if (usuario == null)
                return RedirectToAction("Login", "Usuarios");

            entidad.ID_UsuarioAlta = usuario.Id;
            repositorio.Baja(id, usuario.Id);

            TempData["Mensaje"] = "Contrato eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }

        [Route("[controller]/ObtenerPorInmueble/{id}")]
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult ObtenerPorInmueble(int id)
        {
            var res = repositorio.ObtenerPorInmueble(id);
            return Json(new { Datos = res });
        }

        [Route("[controller]/ObtenerVigentes")]
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult ObtenerVigentes()
        {
            var res = repositorio.ObtenerVigentes();
            return Json(new { Datos = res });
        }

        [Route("[controller]/ObtenerPorInquilino/{id}")]
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult ObtenerPorInquilino(int id)
        {
            var res = repositorio.ObtenerPorInquilino(id);
            return Json(new { Datos = res });
        }

    //zona renovar contrato
        [Route("[controller]/Renovar/{id}")]
        [HttpGet]
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult Renovar(int id)
        {
            var contratoViejo = repositorio.ObtenerPorId(id);
            if (contratoViejo == null)
            {
                TempData["Mensaje"] = "Contrato no encontrado.";
                return RedirectToAction("Index");
            }

            var inmueble = repoInmueble.ObtenerPorId(contratoViejo.ID_Inmueble);
            var inquilino = repoInquilino.ObtenerPorId(contratoViejo.ID_Inquilino);

            if (inmueble == null || inquilino == null)
            {
                TempData["Mensaje"] = "El inmueble o el inquilino del contrato original no están disponibles.";
                return RedirectToAction("Index");
            }

            var nuevoContrato = new Contrato
            {
                ID_Inmueble = contratoViejo.ID_Inmueble,
                ID_Inquilino = contratoViejo.ID_Inquilino,
                Fecha_Inicio = contratoViejo.Fecha_Fin.AddDays(1),
                Fecha_Fin = contratoViejo.Fecha_Fin.AddDays(1).AddYears(1),
                Monto_Alquiler = contratoViejo.Monto_Alquiler,
                Multa = 0,
                Estado = "Vigente",
                EstadoLogico = 1
            };

            ViewBag.InmuebleDireccion = inmueble.Direccion;
            ViewBag.InquilinoNombre = $"{inquilino.Nombre} {inquilino.Apellido}";

            return View("Renovar", nuevoContrato);
        }

        [HttpPost]
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult Renovar(Contrato contrato)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Campos vacios o incorrectos");
                    CargarViewBags(contrato);

                    return View(contrato);
                }

                if (repositorio.ExisteContratoSuperpuesto(contrato.ID_Inmueble, contrato.Fecha_Inicio, contrato.Fecha_Fin))
                {
                    ModelState.AddModelError("", "Ya existe un contrato para este inmueble en las fechas seleccionadas.");
                    CargarViewBags(contrato);

                    return View(contrato);
                }

                var email = User.Identity.Name;
                var usuario = repoUsuario.ObtenerPorEmail(email);

                if (usuario == null)
                    return RedirectToAction("Login", "Usuarios");

                contrato.ID_UsuarioAlta = usuario.Id;

                repositorio.Alta(contrato, usuario.Id);

                TempData["Mensaje"] = "Contrato creado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear el contrato: " + ex.Message);
                CargarViewBags(contrato);

                return View(contrato);
            }
        }
        private void CargarViewBags(Contrato contrato)
        {
            var inmueble = repoInmueble.ObtenerPorId(contrato.ID_Inmueble);
            var inquilino = repoInquilino.ObtenerPorId(contrato.ID_Inquilino);

            ViewBag.InmuebleDireccion = inmueble?.Direccion ?? "—";
            ViewBag.InquilinoNombre = inquilino != null
                ? $"{inquilino.Nombre} {inquilino.Apellido}"
                : "—";
        }
    //fin zona renovar contrato
    //zona multas
        [Route("[controller]/FinalizarAnticipadamente/{id}")]
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult FinalizarAnticipadamente(int id)
        {
            var contrato = repositorio.ObtenerPorId(id);
            if (contrato == null) return null;

            var multa = contrato.Monto_Alquiler * 1.5m;

            ViewBag.Multa = multa;
            return View(contrato);
        }
        [HttpPost]
        public ActionResult FinalizarAnticipadamentePost(int id)
        {
            var email = User.Identity.Name;
            var usuario = repoUsuario.ObtenerPorEmail(email);

            var contrato = repositorio.ObtenerPorId(id);

            if (contrato == null) return null;

            contrato.Fecha_Fin = DateTime.Today;
            contrato.Multa = contrato.Monto_Alquiler * 1.5m;
            contrato.Estado = "Finalizado";
            repositorio.Modificacion(contrato);
            repositorio.Baja(id, usuario.Id);
            return RedirectToAction("Index");
        }
    //fin zona multas
    }
}
