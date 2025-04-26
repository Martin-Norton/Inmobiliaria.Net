using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaNortonNoe.Models
{
	public interface IRepositorioContrato : IRepositorio<Contrato>
    {
        int Alta(Contrato entidad, int idUsuario);
        int Baja(int id, int idUsuario);
        int Baja(int id);
        IList<Contrato> ObtenerTodosBaja();
        Contrato ObtenerPorInmueble(int idInmueble);
        IList<Contrato> ObtenerVigentes();
        Contrato ObtenerPorInquilino(int idInquilino);
        IList<Contrato> ObtenerLista(int paginaNro, int tamPagina);
        int ObtenerCantidad();
        bool ExisteContratoSuperpuestoE(int idInmueble, DateTime fechaInicio, DateTime fechaFin);
        bool ExisteContratoSuperpuesto(int idInmueble, DateTime fechaInicio, DateTime fechaFin);
    }
}
