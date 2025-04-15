using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace inmobiliariaNortonNoe.Models
{
    public interface IRepositorioPago : IRepositorio<Pago>
    {
        IList<Pago> ObtenerPagosPorContrato(int idContrato);
        IList<Pago> ObtenerLista(int paginaNro, int tamPagina);
        int ObtenerCantidad();
    }
}
