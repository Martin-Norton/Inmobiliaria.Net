using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaNortonNoe.Models
{
	public interface IRepositorioContrato : IRepositorio<Contrato>
	{
		IList<Contrato> Alta(Contrato c);
		IList<Contrato> Baja(int Id);
		IList<Contrato> ObtenerTodos();
		IList<Contrato> Modificacion(Contrato c);

	}
}
