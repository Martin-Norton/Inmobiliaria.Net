using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaNortonNoe.Models
{
	public interface IRepositorioInmueble : IRepositorio<Inmueble>
	{
		IList<Inmueble> ObtenerPorPropietario(int propietarioId);
		IList<Inmueble> BuscarPorDireccion(string direccion);
		IList<Inmueble> ObtenerDisponibles();
		IList<Inmueble> ObtenerLista(int paginaNro, int tamPagina);
		int ObtenerCantidad();
	}
}
