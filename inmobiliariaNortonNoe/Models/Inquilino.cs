using System.ComponentModel.DataAnnotations;
namespace inmobiliariaNortonNoe.Models
{
    public class Inquilino
{
    [Key]
    [Display(Name = "Código")]
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; }
    [Required]
    public string Apellido { get; set; }

    [Required]
    public string Dni { get; set; }

    [Required]
    public string Telefono { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

}

}
