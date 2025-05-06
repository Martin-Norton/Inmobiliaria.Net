using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Pago")]
        public DateTime ? Fecha_Pago { get; set; }

        [Required(ErrorMessage = "El periodo correspondiente es obligatorio.")]
        [DataType(DataType.Date)]
        [Display(Name = "Periodo de Pago")]
        public DateTime Periodo_Pago { get; set; }


        [Required(ErrorMessage = "El importe es obligatorio.")]
        [Range(0.01, 1000000)]
        [Display(Name = "Monto")]
        public decimal Monto { get; set; }

        [Display(Name = "Pagado")]
        public bool Pagado { get; set; } = false;

        [Display(Name = "Es Multa")]
        public bool EsMulta { get; set; } = false;

        [MaxLength(200)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Display(Name = "Estado")]
        public int Estado { get; set; } = 1;

        [Display(Name = "Usuario Alta")]
        public int Id_UsuarioAlta { get; set; }

        [Display(Name = "Usuario Baja")]
        public int? Id_UsuarioBaja { get; set; }

        [ForeignKey("Id_Contrato")]
        public Contrato? Contrato { get; set; }
    }
}
