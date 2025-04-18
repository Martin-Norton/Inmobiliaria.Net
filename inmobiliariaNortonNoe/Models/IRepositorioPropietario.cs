﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaNortonNoe.Models
{
	public interface IRepositorioPropietario : IRepositorio<Propietario>
	{
		 IList<Propietario> ObtenerTodos();
		Propietario ObtenerPorEmail(string email);
		IList<Propietario> BuscarPorNombre(string nombre);
		IList<Propietario> ObtenerLista(int paginaNro, int tamPagina);
		int ObtenerCantidad();
	}
}