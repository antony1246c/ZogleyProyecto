using System.Net;
using System.Net.Mail;
using ProyectoZOGLEY.Models;

namespace ProyectoZOGLEY.Services
{
    public class CorreoFacturaService
    {
        private readonly IConfiguration _configuration;

        public CorreoFacturaService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> EnviarFacturaAsync(UserInfo usuario, Factura factura, byte[] pdf)
        {
            var host = _configuration["Email:SmtpHost"];
            var from = _configuration["Email:From"];
            var user = _configuration["Email:User"];
            var password = _configuration["Email:Password"];
            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(from) ||
                string.IsNullOrWhiteSpace(usuario.Email))
                return false;

            if (!int.TryParse(_configuration["Email:Port"], out var port))
                port = 587;

            try
            {
                using var mensaje = new MailMessage();
                mensaje.From = new MailAddress(from, "Zogley");
                mensaje.To.Add(new MailAddress(usuario.Email, usuario.NombreApellido ?? ""));
                mensaje.Subject = $"Factura {factura.NumeroFactura} - Zogley";
                mensaje.IsBodyHtml = true;
                mensaje.Body = $"""
                    <h2>Gracias por tu compra, {WebUtility.HtmlEncode(usuario.NombreApellido)}.</h2>
                    <p>Adjuntamos tu factura <strong>{WebUtility.HtmlEncode(factura.NumeroFactura)}</strong>.</p>
                    <p>Total: <strong>${factura.Total:F2}</strong></p>
                    <p>Conserva este correo como comprobante de tu compra.</p>
                    """;
                mensaje.Attachments.Add(new Attachment(new MemoryStream(pdf), $"Factura-{factura.NumeroFactura}.pdf", "application/pdf"));

                using var smtp = new SmtpClient(host, port)
                {
                    EnableSsl = !string.Equals(_configuration["Email:EnableSsl"], "false", StringComparison.OrdinalIgnoreCase),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 15000
                };
                if (!string.IsNullOrWhiteSpace(user))
                    smtp.Credentials = new NetworkCredential(user, password);
                await smtp.SendMailAsync(mensaje);
                return true;
            }
            catch
            {
                // La compra ya fue confirmada. El fallo de correo no debe revertir ni duplicar la factura.
                return false;
            }
        }
    }
}
