using System;
using System.ComponentModel.DataAnnotations;

namespace inmobiliariaNortonNoe.Models
{
    public class Contrato
    {
        [Key]
        [Display(Name = "Código Int")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El inmueble es obligatorio.")]
        public int InmuebleId { get; set; }
        public Inmueble Inmueble { get; set; }

        [Required(ErrorMessage = "El inquilino es obligatorio.")]
        public int InquilinoId { get; set; }
        public Inquilino Inquilino { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de finalización es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto debe ser un valor positivo.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El estado del contrato es obligatorio.")]
        public bool Activo { get; set; }
    }
}