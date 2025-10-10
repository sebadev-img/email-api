using System.ComponentModel.DataAnnotations;

namespace apiEmail.Models
{
    public class EmailRequest
    {
        [Required]
        [EmailAddress]
        public string ToEmail { get; set; }        

        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string Sede { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public string Dia {  get; set; }
        [Required]
        public string Hora { get; set; }
        [Required]
        public string Subgrupo { get; set; }
    }
}
