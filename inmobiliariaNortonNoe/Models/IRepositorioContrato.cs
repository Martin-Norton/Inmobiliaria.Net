using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaNortonNoe.Models
{
	public interface IRepositorioContrato : IRepositorio<Contrato>
	{
		IList<Contrato> ObtenerPorInquilino(int inquilinoId);
		IList<Contrato> ObtenerPorInmueble(int inmuebleId);
		IList<Contrato> ObtenerVigentes();
		IList<Contrato> ObtenerLista(int paginaNro, int tamPagina);
		int ObtenerCantidad();
	}
}
