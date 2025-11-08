using System.ComponentModel.DataAnnotations;

namespace apiEmail.Models
{
    public class InscripcionEmailRequest
    {
        [Required]
        [EmailAddress]
        public string ToEmail { get; set; }

        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string Dni { get; set; }
        [Required]
        public string Colegio { get; set; }      
        
    }
}
