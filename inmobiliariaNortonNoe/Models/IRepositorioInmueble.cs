using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaNortonNoe.Models
{
	public interface IRepositorioInmueble : IRepositorio<Inmueble>
	{
		IList<Inmueble> ObtenerPorIdPropietario(int propietarioId);
	}
}
