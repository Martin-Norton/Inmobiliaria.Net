using System.ComponentModel.DataAnnotations;

namespace inmobiliariaNortonNoe.Models
{
    public class Inmueble
    {
        [Key]
        [Display(Name = "Código Int")]
        public int Id { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(100, ErrorMessage = "La dirección no puede superar los 100 caracteres.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El tipo de inmueble es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo no puede superar los 50 caracteres.")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "El uso del inmueble es obligatorio.")]
        [StringLength(50, ErrorMessage = "El uso no puede superar los 50 caracteres.")]
        public string Uso { get; set; }

        [Required(ErrorMessage = "La cantidad de ambientes es obligatoria.")]
        [Range(1, 20, ErrorMessage = "La cantidad de ambientes debe estar entre 1 y 20.")]
        public int Ambientes { get; set; }

        [Required(ErrorMessage = "La superficie es obligatoria.")]
        [Range(1, 20, ErrorMessage = "La superficie debe estar")]
        public int Superficie { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El propietario es obligatorio.")]
        public int PropietarioId { get; set; }
    }
}
