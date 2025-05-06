using System.Collections.Generic;

namespace inmobiliariaNortonNoe.Models
{
    public interface IRepositorioTipoInmueble
    {
        int Alta(TipoInmueble t);
        int Baja(int id);
        int Modificacion(TipoInmueble t);
        TipoInmueble ObtenerPorId(int id);
        IList<TipoInmueble> ObtenerTodos();
    }
}
