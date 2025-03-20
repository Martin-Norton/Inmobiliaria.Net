using System.ComponentModel.DataAnnotations;

public class Propietario
{
    [Key]
    [Display(Name = "Código Int")]
    public int Id { get; set; }

    [Required]
    public string Dni { get; set; }

    [Required]
    public string Nombre { get; set; }
    [Required]
    public string Apellido { get; set; }

    [Required]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    public int estado { get; set; }
    public override string ToString()
		{
			//return $"{Apellido}, {Nombre}";
			//return $"{Nombre} {Apellido}";
			var res = $"{Nombre} {Apellido}";
			if(!String.IsNullOrEmpty(Dni)) {
				res += $" ({Dni})";
			}
			return res;
		}
}
