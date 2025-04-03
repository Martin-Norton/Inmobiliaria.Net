using System;
using System.ComponentModel.DataAnnotations;

namespace inmobiliariaNortonNoe.Models
{
    public class Pago
    {
        [Key]
        [Display(Name = "Código Int")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El contrato es obligatorio.")]
        public int ContratoId { get; set; }
        public Contrato Contrato { get; set; }

        [Required(ErrorMessage = "El número de pago es obligatorio.")]
        public int NumeroPago { get; set; }

        [Required(ErrorMessage = "La fecha de pago es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaPago { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto debe ser un valor positivo.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El estado del pago es obligatorio.")]
        public bool Pagado { get; set; }
    }
}
