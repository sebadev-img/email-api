namespace apiEmail.Models
{
    public class EmailSendResult
    {
        public string ToEmail { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
