using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliariaNortonNoe.Models
{
	public enum enRoles
	{
		Administrador = 1,
		Inmobiliaria = 2,
	}

	public class Usuario
	{
		[Key]
		[Display(Name = "Código Int")]
		public int Id { get; set; }

		[Required]
		public string Nombre { get; set; }

		[Required]
		public string Apellido { get; set; }

		[Required, EmailAddress]
		public string Email { get; set; }

		[Required, DataType(DataType.Password)]
		public string Clave { get; set; }

		public string? Avatar { get; set; }

		[NotMapped] 
		public IFormFile? AvatarFile { get; set; }

		[Required]
		public int Rol { get; set; }

		[NotMapped]
		public string RolNombre => Rol > 0 ? ((enRoles)Rol).ToString() : "";

		public static IDictionary<int, string> ObtenerRoles()
		{
			SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
			Type tipoEnumRoles = typeof(enRoles);
			foreach (var valor in Enum.GetValues(tipoEnumRoles))
			{
				roles.Add((int)valor, Enum.GetName(tipoEnumRoles, valor));
			}
			return roles;
		}
	}
}