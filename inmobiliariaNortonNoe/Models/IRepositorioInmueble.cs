using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaNortonNoe.Models
{
	public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        IList<Inmueble> BuscarPorTipo(int idTipoInmueble);
        IList<Inmueble> BuscarPorUso(string uso);
        IList<Inmueble> ObtenerPorEstado(string estado);
        IList<Inmueble> ObtenerLista(int paginaNro, int tamPagina);
        IList<Inmueble> ObtenerPorPropietario(int idPropietario);
        IList<Inmueble> ObtenerInmueblesDisponiblesPorFechas(DateTime fechaInicio, DateTime fechaFin);
        int ObtenerCantidad();
        int ModificarPortada(int InmuebleId, string ruta);
    }

}
