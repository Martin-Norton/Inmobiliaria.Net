using System.ComponentModel.DataAnnotations;

namespace inmobiliariaNortonNoe.Models
{
    public class Inmueble
    {
        [Key]
        [Display(Name = "Código Inmueble")]
        public int Id { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(100, ErrorMessage = "La dirección no puede superar los 100 caracteres.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El uso es obligatorio.")]
        public string Uso { get; set; } // Comercial o Residencial

        [Required(ErrorMessage = "El tipo es obligatorio.")]
        public string Tipo { get; set; } // Local, Depósito, Casa, Departamento, etc.

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad de ambientes debe ser mayor a 0.")]
        public int Cantidad_Ambientes { get; set; }

        public string Coordenadas { get; set; } // Ubicación geográfica opcional

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(1, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string Estado { get; set; } // Disponible, No disponible, Suspendido

        [Required(ErrorMessage = "El propietario es obligatorio.")]
        public int Id_Propietario { get; set; }
    }
}