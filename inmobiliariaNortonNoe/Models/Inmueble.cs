using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


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
        public string Uso { get; set; }

        [Required(ErrorMessage = "El tipo es obligatorio.")]
        [Display(Name = "Tipo de Inmueble")]
        public int Id_TipoInmueble { get; set; }

        [NotMapped]
        public TipoInmueble Tipo { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad de ambientes debe ser mayor a 0.")]
        public int Cantidad_Ambientes { get; set; }

        public string Coordenadas { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(1, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "El propietario es obligatorio.")]
        public int Id_Propietario { get; set; }

        public string? Portada { get; set; } 
        public IFormFile? PortadaFile { get; set; }
        public IList<Imagen> Imagenes { get; set; } = new List<Imagen>();
    }
}