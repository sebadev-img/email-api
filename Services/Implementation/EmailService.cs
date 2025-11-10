using apiEmail.Models;
using apiEmail.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace apiEmail.Services.Implementation
{
    public class EmailService: IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmailService(IOptions<EmailSettings> emailSettings, IWebHostEnvironment webHostEnvironment)
        {
            _emailSettings = emailSettings.Value;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task SendEmailAsync(string senderName, string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(senderName, _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            //var bodyBuilder = new BodyBuilder { HtmlBody = body };
            //message.Body = bodyBuilder.ToMessageBody();

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        public async Task<string> LoadEmailTemplate(string templateName, Dictionary<string, string> replacements)
        {
            // Construct the full path to the template file
            // e.g., templateName could be "email/WelcomeEmail.html"
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", templateName);

            if (!File.Exists(filePath))
            {
                // Handle the case where the template file doesn't exist
                throw new FileNotFoundException($"Template file not found at {filePath}");
            }

            // Read the entire file content
            string templateContent = await File.ReadAllTextAsync(filePath);

            // Perform placeholder replacements
            if (replacements != null)
            {
                foreach (var replacement in replacements)
                {
                    templateContent = templateContent.Replace($"{{{{{replacement.Key}}}}}", replacement.Value);
                }
            }

            return templateContent;
        }

        public async Task SendEmailWithPdfAsync(string senderName, string toEmail, string subject, string body, byte[]? pdfBytes = null, string? pdfFilename = null)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(senderName, _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;
            
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
           
            if (pdfBytes != null && pdfFilename != null)
            {
                bodyBuilder.Attachments.Add(
                    pdfFilename,
                    pdfBytes,
                    new ContentType("application", "pdf")
                );
            }

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(
                    _emailSettings.SmtpServer,
                    _emailSettings.SmtpPort,
                    SecureSocketOptions.StartTls
                );

                await client.AuthenticateAsync(
                    _emailSettings.SenderEmail,
                    _emailSettings.Password
                );

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
