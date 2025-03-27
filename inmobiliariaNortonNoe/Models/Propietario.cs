using System.ComponentModel.DataAnnotations;
namespace inmobiliariaNortonNoe.Models
{
public class Propietario
{
    [Key]
    [Display(Name = "Código Int")]
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; }
    [Required]
    public string Apellido { get; set; }
    
    [Required]
    public string Dni { get; set; }

    [Required]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

}
}
