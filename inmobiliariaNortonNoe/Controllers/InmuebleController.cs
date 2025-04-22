using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using inmobiliariaNortonNoe.Models;
using Microsoft.AspNetCore.Authorization;


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
        public IActionResult Buscar(string q)
        {
            var res = repositorio.BuscarPorTipo(q);
            return Json(new { Datos = res });
        }

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
        [Authorize(Roles = "Inmobiliaria, Administrador")]
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

        [Authorize(Roles = "Inmobiliaria, Administrador")]
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
        [Authorize(Roles = "Inmobiliaria, Administrador")]
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

// using System;
// using System.Collections.Generic;
// using System.IO;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using Microsoft.AspNetCore.Authorization;
// using inmobiliariaNortonNoe.Models;
// // using Microsoft.AspNetCore.Authorization; // Duplicado, puedes eliminar uno
// using Microsoft.AspNetCore.Hosting; // Añadir esto si IWebHostEnvironment no se resuelve

// namespace inmobiliariaNortonNoe.Controllers
// {
//     public class InmuebleController : Controller
//     {
//         private readonly IRepositorioInmueble repositorio;
//         private readonly IRepositorioPropietario repositorioPropietario;
//         private readonly IRepositorioImagen repositorioImagen; // <-- Añadir esto
//         private readonly IWebHostEnvironment environment;

//         // Modificar el constructor para aceptar IRepositorioImagen
//         public InmuebleController(IWebHostEnvironment environment, IRepositorioInmueble repo, IRepositorioPropietario repositorioPropietario, IRepositorioImagen repositorioImagen)
//         {
//             this.environment = environment;
//             this.repositorioPropietario = repositorioPropietario;
//             this.repositorio = repo;
//             this.repositorioImagen = repositorioImagen; // <-- Asignarlo
//         }

//         [Authorize(Roles = "Inmobiliaria, Administrador")]
//         public ActionResult Index()
//         {
//             var lista = repositorio.ObtenerTodos();
//             if (TempData.ContainsKey("Id"))
//                 ViewBag.Id = TempData["Id"];
//             if (TempData.ContainsKey("Mensaje"))
//                 ViewBag.Mensaje = TempData["Mensaje"];
//             if (TempData.ContainsKey("Error"))
//                 ViewBag.Error = TempData["Error"];
//             return View(lista);
//         }

//         [Authorize(Roles = "Inmobiliaria, Administrador")]
//         public ActionResult Lista(int pagina = 1)
//         {
//             int tamaño = 5;
//             var lista = repositorio.ObtenerLista(Math.Max(pagina, 1), tamaño);
//             ViewBag.Pagina = pagina;
//             int total = repositorio.ObtenerCantidad();
//             ViewBag.TotalPaginas = total % tamaño == 0 ? total / tamaño : total / tamaño + 1;
//             return View(lista);
//         }

//         [Authorize(Roles = "Inmobiliaria, Administrador")]
//         public ActionResult Details(int id)
//         {
//             var entidad = repositorio.ObtenerPorId(id); // Obtener el Inmueble
//             if (entidad == null)
//             {
//                 return NotFound(); // Manejar el caso si el Inmueble no se encuentra
//             }

//             // Cargar las imágenes asociadas
//             entidad.Imagenes = repositorioImagen.BuscarPorInmueble(id); // <-- Añadir esta línea

//             // Esta parte parece redundante si estás cargando imágenes arriba
//             // var propietario = repositorioPropietario.ObtenerPorId(entidad.Id_Propietario);
//             // ViewBag.NombrePropietario = propietario?.Nombre + " " + propietario?.Apellido;


//             return View(entidad);
//         }

//         [Authorize(Roles = "Inmobiliaria, Administrador")]
//         public IActionResult Busqueda()
//         {
//             return View();
//         }

//         [Route("[controller]/Buscar/{q}")]
//         [Authorize(Roles = "Inmobiliaria, Administrador")]
//         public IActionResult Buscar(string q)
//         {
//             var res = repositorio.BuscarPorTipo(q);
//             return Json(new { Datos = res });
//         }

//         [Authorize(Roles = "Inmobiliaria, Administrador")]
//         public IActionResult Create()
//         {
//             try
//             {
//                 var listaPropietarios = repositorioPropietario.ObtenerTodos();
//                 if (listaPropietarios == null || listaPropietarios.Count == 0)
//                 {
//                     TempData["Mensaje"] = "No hay propietarios disponibles.";
//                     return RedirectToAction("Index");
//                 }

//                 // Asumiendo que el modelo Propietario tiene una propiedad NombreCompleto
//                 ViewBag.Propietarios = new SelectList(listaPropietarios, "Id", "NombreCompleto");

//                 if (TempData.ContainsKey("Mensaje"))
//                     ViewBag.Mensaje = TempData["Mensaje"];

//                 return View(new Inmueble());
//             }
//             catch (Exception ex)
//             {
//                 ViewBag.Error = ex.Message;
//                 return RedirectToAction(nameof(Index));
//             }
//         }

//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         [Authorize(Roles = "Inmobiliaria, Administrador")]
//         public async Task<ActionResult> Create(Inmueble inmueble)
//         {
//             if (!ModelState.IsValid)
//             {
//                  var listaPropietarios = repositorioPropietario.ObtenerTodos();
//                  // Asumiendo que el modelo Propietario tiene una propiedad NombreCompleto
//                  ViewBag.Propietarios = new SelectList(listaPropietarios, "Id", "NombreCompleto");
//                 return View(inmueble);
//             }

//             if (inmueble.PortadaFile != null && inmueble.PortadaFile.Length > 0)
//             {
//                 var uploads = Path.Combine(environment.WebRootPath, "Uploads", "Portadas");
//                 if (!Directory.Exists(uploads))
//                     Directory.CreateDirectory(uploads);

//                 var archivoNombre = Guid.NewGuid().ToString() + Path.GetExtension(inmueble.PortadaFile.FileName);
//                 var rutaArchivo = Path.Combine(uploads, archivoNombre);

//                 using (var stream = new FileStream(rutaArchivo, FileMode.Create))
//                 {
//                     await inmueble.PortadaFile.CopyToAsync(stream);
//                 }
//                 inmueble.Portada = $"/Uploads/Portadas/{archivoNombre}";
//             }
//             // Asegurar que Portada no sea nulo si no se subió ningún archivo
//             else
//             {
//                  inmueble.Portada = null; // O string.Empty, dependiendo de tu modelo
//             }

//             repositorio.Alta(inmueble);
//             TempData["Mensaje"] = "Inmueble creado correctamente";
//             TempData["Id"] = inmueble.Id;
//             return RedirectToAction(nameof(Index));
//         }

//         [Authorize(Roles = "Inmobiliaria, Administrador")]
//         public ActionResult Edit(int id)
//         {
//             var entidad = repositorio.ObtenerPorId(id);
//              if (entidad == null)
//             {
//                 return NotFound(); // Manejar el caso si el Inmueble no se encuentra
//             }
//             // La obtención del propietario aquí es para mostrarlo en la vista de edición, no es estrictamente necesaria para cargar imágenes
//             // var propietario = repositorioPropietario.ObtenerPorId(entidad.Id_Propietario);
//             // ViewBag.NombrePropietario = propietario?.Nombre + " " + propietario?.Apellido;

//             // Asumiendo que el modelo Propietario tiene una propiedad NombreCompleto
//             ViewBag.Propietarios = new SelectList(repositorioPropietario.ObtenerTodos(), "Id", "NombreCompleto", entidad.Id_Propietario);

//             if (TempData.ContainsKey("Mensaje"))
//                 ViewBag.Mensaje = TempData["Mensaje"];
//             if (TempData.ContainsKey("Error"))
//                 ViewBag.Error = TempData["Error"];

//             return View(entidad);
//         }

//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         [Authorize(Roles = "Inmobiliaria, Administrador")]
//         public async Task<ActionResult> Edit(int id, Inmueble inmueble)
//         {
//             if (id != inmueble.Id)
//             {
//                 return BadRequest();
//             }

//             var entidadExistente = repositorio.ObtenerPorId(id);
//             if (entidadExistente == null) return NotFound();


//             if (!ModelState.IsValid)
//             {
//                  var listaPropietarios = repositorioPropietario.ObtenerTodos();
//                  // Asumiendo que el modelo Propietario tiene una propiedad NombreCompleto
//                  ViewBag.Propietarios = new SelectList(listaPropietarios, "Id", "NombreCompleto", inmueble.Id_Propietario);
//                 return View(inmueble);
//             }


//             if (inmueble.PortadaFile != null && inmueble.PortadaFile.Length > 0)
//             {
//                 // Ruta consistente con Create
//                 var uploads = Path.Combine(environment.WebRootPath, "Uploads", "Portadas");
//                 if (!Directory.Exists(uploads))
//                     Directory.CreateDirectory(uploads);

//                 var archivoNombre = Guid.NewGuid().ToString() + Path.GetExtension(inmueble.PortadaFile.FileName);
//                 var rutaArchivo = Path.Combine(uploads, archivoNombre);

//                 // Opcional: Eliminar el archivo de portada antiguo antes de guardar el nuevo
//                 if (!string.IsNullOrEmpty(entidadExistente.Portada))
//                 {
//                      var oldPortadaPath = Path.Combine(environment.WebRootPath, entidadExistente.Portada.TrimStart('/'));
//                      if (System.IO.File.Exists(oldPortadaPath))
//                      {
//                          System.IO.File.Delete(oldPortadaPath);
//                      }
//                 }


//                 using (var stream = new FileStream(rutaArchivo, FileMode.Create))
//                 {
//                     await inmueble.PortadaFile.CopyToAsync(stream);
//                 }

//                 entidadExistente.Portada = $"/Uploads/Portadas/{archivoNombre}"; // Actualizar la ruta de la portada
//             }
//             // Si no se subió un nuevo archivo, mantener la ruta de Portada existente.
//             // Si quisieras permitir quitar la portada, necesitarías un mecanismo específico.


//             entidadExistente.Direccion = inmueble.Direccion;
//             entidadExistente.Precio = inmueble.Precio;
//             entidadExistente.Tipo = inmueble.Tipo;
//             entidadExistente.Uso = inmueble.Uso;
//             entidadExistente.Id_Propietario = inmueble.Id_Propietario;
//             // entidadExistente.Portada se actualiza arriba si se subió un nuevo archivo

//             repositorio.Modificacion(entidadExistente);
//             TempData["Mensaje"] = "Datos actualizados correctamente";
//             return RedirectToAction(nameof(Index));
//         }

//         [Authorize(Roles = "Inmobiliaria, Administrador")]
//         public ActionResult Eliminar(int id)
//         {
//             var entidad = repositorio.ObtenerPorId(id);
//              if (entidad == null)
//             {
//                 return NotFound(); // Manejar el caso si el Inmueble no se encuentra
//             }
//             return View(entidad);
//         }

//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         [Authorize(Roles = "Inmobiliaria, Administrador")]
//         public ActionResult Eliminar(int id, Inmueble entidad) // Nota: 'entidad' aquí probablemente viene del formulario, no de la base de datos
//         {
//              // Volver a obtener la entidad para asegurar que tienes el estado más reciente, incluyendo las rutas de archivo
//              var entidadEliminar = repositorio.ObtenerPorId(id);
//              if (entidadEliminar == null)
//              {
//                  return NotFound();
//              }

//             try
//             {
//                  // Opcional: Eliminar archivos asociados (portada e imágenes de galería)
//                  if (!string.IsNullOrEmpty(entidadEliminar.Portada))
//                  {
//                       // Necesitas determinar la ruta correcta según cómo fue guardada originalmente
//                       // Si fue guardada en /Uploads/Portadas:
//                       string portadaPath = Path.Combine(environment.WebRootPath, entidadEliminar.Portada.TrimStart('/'));
//                       if (System.IO.File.Exists(portadaPath))
//                       {
//                            System.IO.File.Delete(portadaPath);
//                       }
//                  }
//                  var galleryPath = Path.Combine(environment.WebRootPath, "Uploads", "Inmuebles", id.ToString());
//                  if (Directory.Exists(galleryPath))
//                  {
//                       Directory.Delete(galleryPath, true);
//                  }
                
//                 repositorio.Baja(id);

//                 TempData["Mensaje"] = "Inmueble eliminado correctamente";
//                 return RedirectToAction(nameof(Index));
//             }
//             catch (Exception ex)
//             {
//                  var entidadVista = repositorio.ObtenerPorId(id);
//                  ViewBag.Error = ex.Message;
//                  return View(entidadVista ?? entidad); 
//             }
//         }
//     }
// }