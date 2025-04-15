using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using inmobiliariaNortonNoe.Models;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace inmobiliariaNortonNoe.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly IRepositorioInmueble repositorio;
        private readonly IRepositorioPropietario repositorioPropietario;
        IWebHostEnvironment environment;

        public InmuebleController(IWebHostEnvironment environment, IRepositorioInmueble repo, IRepositorioPropietario repositorioPropietario)
        {
            this.environment = environment;
            this.repositorioPropietario = repositorioPropietario;
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble inmueble)
        {
            if (!ModelState.IsValid)
            {
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

                var listaPropietarios = repositorioPropietario.ObtenerTodos();

                if (listaPropietarios == null || listaPropietarios.Count == 0)
                {
                    TempData["Mensaje"] = "No hay propietarios disponibles";
                    return RedirectToAction("Index");
                }

                ViewBag.Propietarios = new SelectList(listaPropietarios, "Id", "Nombre");

                return View(inmueble);
            }


            repositorio.Alta(inmueble);
            TempData["Mensaje"] = "Inmueble creado correctamente";
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(int id)
        {

            var entidad = repositorio.ObtenerPorId(id);
            
            var propietario = repositorioPropietario.ObtenerPorId(entidad.Id_Propietario);
            ViewBag.NombrePropietario = propietario.Nombre + " " + propietario.Apellido;
            return View(entidad);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble entidad)
        {
            if (!ModelState.IsValid) return View(entidad);
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
            var p = repositorio.ObtenerPorId(id);
            if (p == null) return NotFound();

            p.Direccion = entidad.Direccion;
            p.Precio = entidad.Precio;
            p.Tipo = entidad.Tipo;
            p.Uso = entidad.Uso;
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
        public ActionResult Eliminar(int id, Inmueble entidad)
        {
            repositorio.Baja(id);
            TempData["Mensaje"] = "Inmueble eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }
    }
}
