using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using inmobiliariaNortonNoe.Models;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace inmobiliariaNortonNoe.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly IRepositorioInmueble repositorio;
        private readonly IRepositorioPropietario repositorioPropietario;
        private readonly IWebHostEnvironment environment;
        private readonly IRepositorioTipoInmueble repositorioTipoInmueble;

        public InmuebleController(IWebHostEnvironment environment, IRepositorioInmueble repo, IRepositorioPropietario repositorioPropietario, IRepositorioTipoInmueble repositorioTipoInmueble)
        {
            this.environment = environment;
            this.repositorioPropietario = repositorioPropietario;
            this.repositorio = repo;
            this.repositorioTipoInmueble = repositorioTipoInmueble;
        }

        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
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
            return View(entidad);
        }

        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult Busqueda()
        {
            return View();
        }

        [Route("[controller]/Buscar/{q}")]
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult Buscar(int q)
        {
            var res = repositorio.BuscarPorTipo(q);
            return Json(new { Datos = res });
        }

        //region Busquedas

        [Route("[controller]/BuscarPorPropietario")]
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult BuscarPorPropietario(int propietarioId = 0)
        {
            var propietarios = repositorioPropietario.ObtenerTodos().ToList();
            ViewBag.Propietarios = propietarios;

            List<Inmueble> inmuebles = new List<Inmueble>();

            if (propietarioId > 0)
            {
                inmuebles = repositorio.ObtenerPorPropietario(propietarioId).ToList();
            }

            return View("BuscarPorPropietario", inmuebles);
        }

        [Route("[controller]/Buscar")]
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult BuscarDisponibles(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return View("Buscar", new List<InmueblePropietarioViewModel>());
            }

            var inmuebles = repositorio.ObtenerPorEstado(q).ToList();
            var resultados = new List<InmueblePropietarioViewModel>();

            foreach (var inmueble in inmuebles)
            {
                var propietario = repositorioPropietario.ObtenerPorId(inmueble.Id_Propietario);
                if (propietario != null && inmueble != null)
                {
                    resultados.Add(new InmueblePropietarioViewModel
                    {
                        Inmueble = inmueble,
                        Propietario = propietario
                    });
                }
            }

            return View("Buscar", resultados);
        }

        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult BuscarDisponibles()
        {
            return View("Buscar", null);
        }

        [Route("[controller]/BuscarDisponiblesPorFechas")]
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult BuscarDisponiblesPorFechas(DateTime fechaDesde, DateTime fechaHasta)
        {
            if (fechaDesde == DateTime.MinValue || fechaHasta == DateTime.MinValue)
            {
                return View("BuscarDisponiblesPorFechas", new List<Inmueble>());
            }

            var inmuebles = repositorio.ObtenerInmueblesDisponiblesPorFechas(fechaDesde, fechaHasta).ToList();

            return View("BuscarDisponiblesPorFechas", inmuebles);
        }

        //endregion Busquedas

        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public IActionResult Create()
        {
            try
            {
                var listaPropietarios = repositorioPropietario.ObtenerTodos();
                if (listaPropietarios == null || listaPropietarios.Count == 0)
                {
                    TempData["Mensaje"] = "No hay propietarios disponibles.";
                    return RedirectToAction("Index");
                }

                var listaTiposInmueble = repositorioTipoInmueble.ObtenerTodos();
                ViewBag.Propietarios = new SelectList(listaPropietarios, "Id", "Nombre");
                ViewBag.TiposInmueble = new SelectList(listaTiposInmueble, "Id", "Nombre");
                Console.WriteLine("se cargaron los view bag, enviado desde el get de create");
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                    Console.WriteLine("mensaje desde el if de tempdata: " + TempData["Mensaje"]);
            return View(new Inmueble());
            }
            catch (Exception ex)
            {
                Console.WriteLine("se lanzo la excepcion desde el catch del get create: " + ex.Message);
                ViewBag.Error = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public async Task<ActionResult> Create(Inmueble inmueble)
        {
            ModelState.Remove("Tipo");
            if (!ModelState.IsValid)
            {
                var listaPropietarios = repositorioPropietario.ObtenerTodos();
                var listaTiposInmueble = repositorioTipoInmueble.ObtenerTodos();
                ViewBag.Propietarios = new SelectList(listaPropietarios, "Id", "Nombre");
                ViewBag.TiposInmueble = new SelectList(listaTiposInmueble, "Id", "Nombre");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Error en el campo '{state.Key}': {error.ErrorMessage}");
                    }
                }

                return View(inmueble);
            }

            if (inmueble.PortadaFile != null && inmueble.PortadaFile.Length > 0)
            {
                var uploads = Path.Combine(environment.WebRootPath, "Uploads", "Portadas");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                var archivoNombre = Guid.NewGuid().ToString() + Path.GetExtension(inmueble.PortadaFile.FileName);
                var rutaArchivo = Path.Combine(uploads, archivoNombre);

                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    await inmueble.PortadaFile.CopyToAsync(stream);
                }
                inmueble.Portada = $"/Uploads/Portadas/{archivoNombre}";
            }

            repositorio.Alta(inmueble);
            TempData["Mensaje"] = "Inmueble creado correctamente";
            TempData["Id"] = inmueble.Id;
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult Edit(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            var propietario = repositorioPropietario.ObtenerPorId(entidad.Id_Propietario);
            var tiposInmueble = repositorioTipoInmueble.ObtenerTodos();
            ViewBag.NombrePropietario = propietario?.Nombre + " " + propietario?.Apellido;
            ViewBag.Propietarios = new SelectList(repositorioPropietario.ObtenerTodos(), "Id", "Nombre", entidad.Id_Propietario);
            ViewBag.TiposInmueble = new SelectList(tiposInmueble, "Id", "Nombre", entidad.Id_TipoInmueble);

            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];

            return View(entidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public async Task<ActionResult> Edit(int id, Inmueble inmueble)
        {
            ModelState.Remove("Tipo");

            if (!ModelState.IsValid)
            {
                var tiposInmueble = repositorioTipoInmueble.ObtenerTodos();
                ViewBag.TiposInmueble = new SelectList(tiposInmueble, "Id", "Nombre", inmueble.Id_TipoInmueble);
                ViewBag.Propietarios = new SelectList(repositorioPropietario.ObtenerTodos(), "Id", "Nombre");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Error en el campo '{state.Key}': {error.ErrorMessage}");
                    }
                }
                Console.WriteLine("se lanzo la excepcion desde el catch del post edit: " + ModelState.Values.ToString());
                return View(inmueble);
            }

            if (inmueble.PortadaFile != null && inmueble.PortadaFile.Length > 0)
            {
                var uploads = Path.Combine(environment.WebRootPath, "Uploads", "Portadas");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                var archivoNombre = Guid.NewGuid().ToString() + Path.GetExtension(inmueble.PortadaFile.FileName);
                var rutaArchivo = Path.Combine(uploads, archivoNombre);

                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    await inmueble.PortadaFile.CopyToAsync(stream);
                }

                inmueble.Portada = $"/Uploads/Portadas/{archivoNombre}";
            }

            var entidadExistente = repositorio.ObtenerPorId(id);
            if (entidadExistente == null) return NotFound();

            entidadExistente.Direccion = inmueble.Direccion;
            entidadExistente.Precio = inmueble.Precio;
            entidadExistente.Tipo = inmueble.Tipo;
            entidadExistente.Uso = inmueble.Uso;
            entidadExistente.Id_Propietario = inmueble.Id_Propietario;
            entidadExistente.Id_TipoInmueble = inmueble.Id_TipoInmueble;
            entidadExistente.Portada = inmueble.Portada ?? entidadExistente.Portada;

            repositorio.Modificacion(entidadExistente);
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
        public ActionResult Eliminar(int id, Inmueble entidad)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Inmueble eliminado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(entidad);
            }
        }

        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult Imagenes(int id, [FromServices] IRepositorioImagen repoImagen)
        {
            var entidad = repositorio.ObtenerPorId(id);
            entidad.Imagenes = repoImagen.BuscarPorInmueble(id);
            return View(entidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Inmobiliaria, Administrador")]
        public ActionResult Portada(Imagen entidad, [FromServices] IWebHostEnvironment environment)
        {
            try
            {
                var inmueble = repositorio.ObtenerPorId(entidad.InmuebleId);
                if (inmueble != null && inmueble.Portada != null)
                {
                    string rutaEliminar = Path.Combine(environment.WebRootPath, "Uploads", "Inmuebles", Path.GetFileName(inmueble.Portada));
                    System.IO.File.Delete(rutaEliminar);
                }

                if (entidad.Archivo != null)
                {
                    string path = Path.Combine(environment.WebRootPath, "Uploads", "Inmuebles");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string fileName = "portada_" + entidad.InmuebleId + Path.GetExtension(entidad.Archivo.FileName);
                    string rutaFisicaCompleta = Path.Combine(path, fileName);

                    using (var stream = new FileStream(rutaFisicaCompleta, FileMode.Create))
                    {
                        entidad.Archivo.CopyTo(stream);
                    }

                    entidad.Url = Path.Combine("/Uploads/Inmuebles", fileName);
                }
                else
                {
                    entidad.Url = string.Empty;
                }

                repositorio.ModificarPortada(entidad.InmuebleId, entidad.Url);
                TempData["Mensaje"] = "Portada actualizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Imagenes), new { id = entidad.InmuebleId });
            }
        }
    }
}
