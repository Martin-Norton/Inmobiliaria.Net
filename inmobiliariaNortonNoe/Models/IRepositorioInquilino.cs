﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaNortonNoe.Models
{
	public interface IRepositorioInquilino : IRepositorio<Inquilino>
	{
		Inquilino ObtenerPorEmail(string email);
		IList<Inquilino> BuscarPorNombre(string nombre);
		IList<Inquilino> ObtenerLista(int paginaNro, int tamPagina);
		int ObtenerCantidad();
	}
}