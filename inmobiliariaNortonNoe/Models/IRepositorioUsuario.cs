using System.Collections.Generic;

namespace inmobiliariaNortonNoe.Models
{
	public interface IRepositorioUsuario : IRepositorio<Usuario>
	{
		Usuario? ObtenerPorEmail(string email); 
	}
}