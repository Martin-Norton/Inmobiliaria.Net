using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaNortonNoe.Models
{
	public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        IList<Inmueble> BuscarPorTipo(string tipo);
        IList<Inmueble> BuscarPorUso(string uso);
        IList<Inmueble> ObtenerPorEstado(string estado);
        IList<Inmueble> ObtenerLista(int paginaNro, int tamPagina);
        int ObtenerCantidad();
    }

}
