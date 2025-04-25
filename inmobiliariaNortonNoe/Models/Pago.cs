using System.ComponentModel.DataAnnotations;

namespace inmobiliariaNortonNoe.Models
{
    public class Pago
    {
        [Key]
        [Display(Name = "Código de Pago")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El contrato es obligatorio.")]
        [Display(Name = "Contrato")]
        public int Id_Contrato { get; set; }

        [Required(ErrorMessage = "La fecha de pago es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Pago")]
        public DateTime Fecha_Pago { get; set; }

        [Required(ErrorMessage = "El importe es obligatorio.")]
        [Range(1, double.MaxValue, ErrorMessage = "El importe debe ser mayor a 0.")]
        public decimal Monto { get; set; }

        public int Estado { get; set; } = 1;

        public int Id_UsuarioAlta { get; set; }

        public int Id_UsuarioBaja { get; set; }
    }
}
