using System.ComponentModel.DataAnnotations;

namespace inmobiliariaNortonNoe.Models
{
    public class Contrato
    {
        [Key]
        [Display(Name = "Código Contrato")]
        public int ID_Contrato { get; set; }

        [Required(ErrorMessage = "El inmueble es obligatorio.")]
        public int ID_Inmueble { get; set; }
        public Inmueble Inmueble { get; set; }

        [Required(ErrorMessage = "El inquilino es obligatorio.")]
        public int ID_Inquilino { get; set; }
        public Inquilino Inquilino { get; set; }
        
        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime Fecha_Inicio { get; set; }

        [Required(ErrorMessage = "La fecha de finalización es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime Fecha_Fin { get; set; }

        [Required(ErrorMessage = "El monto del alquiler es obligatorio.")]
        [Range(1, double.MaxValue, ErrorMessage = "El monto del alquiler debe ser mayor a 0.")]
        public decimal Monto_Alquiler { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "La multa no puede ser negativa.")]
        public decimal Multa { get; set; }

        [Required(ErrorMessage = "El estado del contrato es obligatorio.")]
        public string Estado { get; set; }

        public int EstadoLogico { get; set; } = 1;
        public int ID_UsuarioAlta { get; set; }
        public int? ID_UsuarioBaja { get; set; } 
    }
}
