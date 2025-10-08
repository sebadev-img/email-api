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
    }
}
