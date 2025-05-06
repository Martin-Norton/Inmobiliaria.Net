using System.ComponentModel.DataAnnotations;

namespace inmobiliariaNortonNoe.Models
{
    public class TipoInmueble
    {
        [Key]
        [Display(Name = "Código de Tipo de Inmueble")]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int Estado { get; set; } = 1;
    }
}
