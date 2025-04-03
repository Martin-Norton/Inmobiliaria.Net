using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaNortonNoe.Models
{
	public interface IRepositorioPagos : IRepositorio<Pago>
	{
		IList<Pago> ObtenerPorContrato(int contratoId);
		IList<Pago> ObtenerLista(int paginaNro, int tamPagina);
		int ObtenerCantidad();
	}
}
