using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaNortonNoe.Models
{
	public interface IRepositorioImagen : IRepositorio<Imagen>
	{
		IList<Imagen> BuscarPorInmueble(int inmuebleId);
	}
}