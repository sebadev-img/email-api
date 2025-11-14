using System.ComponentModel.DataAnnotations;

namespace apiEmail.Models
{
    public class InvitacionEmailRequest
    {
        [Required]
        [EmailAddress]
        public string ToEmail { get; set; }
    }
}
