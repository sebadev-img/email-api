using apiEmail.Models;
using apiEmail.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace apiEmail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
       
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            try
            {
                var replacements = new Dictionary<string, string>
            {
                { "Nombre", request.Nombre },
                { "Apellido", request.Apellido },
                { "Sede", request.Sede },
                { "Direccion", request.Direccion },
                { "Subgrupo", request.Subgrupo },
                { "Dia", request.Dia },
                { "Hora", request.Hora },
                { "CurrentYear", DateTime.Now.Year.ToString() }
            };

                string emailBody = await _emailService.LoadEmailTemplate("email/comprobante.html", replacements);
                string senderName = "Congreso Docente 2025";
                string subject = "Inscripción Congreso Docente 2025";

                await _emailService.SendEmailAsync(senderName, request.ToEmail, subject, emailBody);
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {                
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("sendList")]
        public async Task<IActionResult> SendEmailList([FromBody] List<EmailRequest> requests)
        {
            var results = new List<EmailSendResult>();

            foreach (var request in requests)
            {
                try
                {
                    var replacements = new Dictionary<string, string>
                {
                    { "Nombre", request.Nombre },
                    { "Apellido", request.Apellido },
                    { "Sede", request.Sede },
                    { "Direccion", request.Direccion },
                    { "Subgrupo", request.Subgrupo },
                    { "Dia", request.Dia },
                    { "Hora", request.Hora },
                    { "CurrentYear", DateTime.Now.Year.ToString() }
                };

                    string emailBody = await _emailService.LoadEmailTemplate("email/comprobante.html", replacements);
                    string senderName = "Congreso Docente 2025";
                    string subject = "Inscripción Congreso Docente 2025";

                    await _emailService.SendEmailAsync(senderName, request.ToEmail, subject, emailBody);

                    results.Add(new EmailSendResult { ToEmail = request.ToEmail, Success = true });
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging purposes (optional but recommended)
                    // _logger.LogError(ex, $"Failed to send email to {request.ToEmail}");

                    results.Add(new EmailSendResult { ToEmail = request.ToEmail, Success = false, ErrorMessage = ex.Message });
                }
            }

            // You can decide on the overall status code based on the results.
            // For partial success, 200 OK with a detailed body is a common practice.
            // Another option for partial success is the 207 Multi-Status code.
            return Ok(results);
        }

        [HttpPost("sendConfirmacion")]
        public async Task<IActionResult> SendConfirmacion([FromBody] EmailRequest request)
        {
            try
            {
                var replacements = new Dictionary<string, string>
            {
                { "Nombre", request.Nombre },
                { "Apellido", request.Apellido },
                { "Sede", request.Sede },
                { "Direccion", request.Direccion },
                { "Subgrupo", request.Subgrupo },
                { "Dia", request.Dia },
                { "Hora", request.Hora },
                { "CurrentYear", DateTime.Now.Year.ToString() }
            };

                string emailBody = await _emailService.LoadEmailTemplate("email/confirmacion.html", replacements);
                string senderName = "Congreso Docente 2025";
                string subject = "Inscripción Congreso Docente 2025";

                var pdfPath = Path.Combine("wwwroot", "templates", "pdf", "cronograma.pdf");
                byte[] pdfBytes = await System.IO.File.ReadAllBytesAsync(pdfPath);

                await _emailService.SendEmailWithPdfAsync(senderName, request.ToEmail, subject, emailBody, pdfBytes, "Cronograma Congreso Internacional de Educación 2025.pdf");
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("sendInvitacionCierre")]
        public async Task<IActionResult> SendInvitacion([FromBody] InvitacionEmailRequest request)
        {
            try
            {
                var replacements = new Dictionary<string, string>
            {                
                { "CurrentYear", DateTime.Now.Year.ToString() }
            };

                string emailBody = await _emailService.LoadEmailTemplate("email/invitacion.html", replacements);
                string senderName = "Congreso Docente 2025";
                string subject = "Cierre Congreso Docente 2025";

                var pdfPath = Path.Combine("wwwroot", "templates", "pdf", "cronograma.pdf");
                byte[] pdfBytes = await System.IO.File.ReadAllBytesAsync(pdfPath);

                await _emailService.SendEmailWithPdfAsync(senderName, request.ToEmail, subject, emailBody, pdfBytes, "Cronograma Congreso Internacional de Educación 2025.pdf");
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("sendCertificado")]
        public async Task<IActionResult> SendCertificado([FromBody] InvitacionEmailRequest request)
        {
            try
            {
                var replacements = new Dictionary<string, string>
            {
                { "CurrentYear", DateTime.Now.Year.ToString() }
            };

                string emailBody = await _emailService.LoadEmailTemplate("email/certificado.html", replacements);
                string senderName = "Congreso Docente 2025";
                string subject = "Certificado Congreso Docente 2025";

                //var pdfPath = Path.Combine("wwwroot", "templates", "pdf", "cronograma.pdf");
                //byte[] pdfBytes = await System.IO.File.ReadAllBytesAsync(pdfPath);

                await _emailService.SendEmailWithPdfAsync(senderName, request.ToEmail, subject, emailBody, null, "Cronograma Congreso Internacional de Educación 2025.pdf");
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
