using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using inmobiliariaNortonNoe.Models;

namespace inmobiliariaNortonNoe.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly IRepositorioInmueble repositorio;
        private readonly IRepositorioPropietario repositorioPropietario;
        private readonly IWebHostEnvironment environment;

        public InmuebleController(IWebHostEnvironment environment, IRepositorioInmueble repo, IRepositorioPropietario repositorioPropietario)
        {
            this.environment = environment;
            this.repositorioPropietario = repositorioPropietario;
            this.repositorio = repo;
        }

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

        public IActionResult Busqueda()
        {
            return View();
        }

        [Route("[controller]/Buscar/{q}")]
        public IActionResult Buscar(string q)
        {
            var res = repositorio.BuscarPorTipo(q);
            return Json(new { Datos = res });
        }

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

                ViewBag.Propietarios = new SelectList(listaPropietarios, "Id", "Nombre");

                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];

                return View(new Inmueble());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Inmueble inmueble)
        {
            if (!ModelState.IsValid)
            {
                var listaPropietarios = repositorioPropietario.ObtenerTodos();
                ViewBag.Propietarios = new SelectList(listaPropietarios, "Id", "Nombre");

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

        public ActionResult Edit(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            var propietario = repositorioPropietario.ObtenerPorId(entidad.Id_Propietario);
            ViewBag.NombrePropietario = propietario?.Nombre + " " + propietario?.Apellido;
            ViewBag.Propietarios = new SelectList(repositorioPropietario.ObtenerTodos(), "Id", "Nombre", entidad.Id_Propietario);

            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];

            return View(entidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Inmueble inmueble)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Propietarios = new SelectList(repositorioPropietario.ObtenerTodos(), "Id", "Nombre");
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
            entidadExistente.Portada = inmueble.Portada ?? entidadExistente.Portada;

            repositorio.Modificacion(entidadExistente);
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

        public ActionResult Imagenes(int id, [FromServices] IRepositorioImagen repoImagen)
        {
            var entidad = repositorio.ObtenerPorId(id);
            entidad.Imagenes = repoImagen.BuscarPorInmueble(id);
            return View(entidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
