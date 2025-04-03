using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaNortonNoe.Models
{
	public interface IRepositorioContrato : IRepositorio<Contrato>
    {
        Contrato ObtenerPorInmueble(int idInmueble);
        IList<Contrato> ObtenerVigentes();
        IList<Contrato> ObtenerPorInquilino(int idInquilino);
        IList<Contrato> ObtenerLista(int paginaNro, int tamPagina);
        int ObtenerCantidad();
    }
}
