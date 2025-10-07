using System.ComponentModel.DataAnnotations;

namespace apiEmail.Models
{
    public class EmailRequest
    {
        [Required]
        public string SenderName { get; set; }

        [Required]
        [EmailAddress]
        public string ToEmail { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
