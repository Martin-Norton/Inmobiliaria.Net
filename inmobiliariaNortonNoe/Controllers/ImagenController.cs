﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using inmobiliariaNortonNoe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace inmobiliariaNortonNoe.Controllers
{
	public class ImagenesController : Controller
	{
		private readonly IRepositorioImagen repositorio;

		public ImagenesController(IRepositorioImagen repositorio)
		{
			this.repositorio = repositorio;
		}
		[HttpPost]
		public async Task<IActionResult> Alta(int id, List<IFormFile> imagenes, [FromServices] IWebHostEnvironment environment)
		{
			if (imagenes == null || imagenes.Count == 0)
				return BadRequest("No se recibieron archivos.");
			string wwwPath = environment.WebRootPath;
			string path = Path.Combine(wwwPath, "Uploads");
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			path = Path.Combine(path, "Inmuebles");
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			path = Path.Combine(path, id.ToString());
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			foreach (var file in imagenes)
			{
				if (file.Length > 0)
				{
					var extension = Path.GetExtension(file.FileName);
					var nombreArchivo = $"{Guid.NewGuid()}{extension}";
					var rutaArchivo = Path.Combine(path, nombreArchivo);

					using (var stream = new FileStream(rutaArchivo, FileMode.Create))
					{
						await file.CopyToAsync(stream);
					}
					Imagen imagen = new Imagen
					{
						InmuebleId = id,
						Url = $"/Uploads/Inmuebles/{id}/{nombreArchivo}",
					};
					repositorio.Alta(imagen);
					var resultado = repositorio.Alta(imagen);
					if (resultado == 0)
					{
						return BadRequest("No se insertó la imagen en la base de datos.");
					}
				}
			}
			return Ok(repositorio.BuscarPorInmueble(id));
		}
		[HttpPost]
		public ActionResult Eliminar(int id)
		{
			try
			{
				var entidad = repositorio.ObtenerPorId(id);
				repositorio.Baja(id);
				return Ok(repositorio.BuscarPorInmueble(entidad.InmuebleId));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}