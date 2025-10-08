namespace apiEmail.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string senderName, string toEmail, string subject, string body);
        Task<string> LoadEmailTemplate(string templateName, Dictionary<string, string> replacements);
    }
}
