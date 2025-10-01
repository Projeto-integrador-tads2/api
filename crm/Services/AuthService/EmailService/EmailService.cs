using Services;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Alure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(
            string emailDestino,
            string nomeDestino,
            string assunto,
            string mensagem,
            string textoBotao,
            string link
        )
        {
            var smtpHost = _configuration["Smtp:Host"];
            var smtpPort = int.Parse(_configuration["Smtp:Port"]);
            var smtpUser = _configuration["Smtp:User"];
            var smtpPass = _configuration["Smtp:Pass"];
            var remetente = _configuration["Smtp:From"];

            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(smtpUser, smtpPass);

                var bodyBuilder = new StringBuilder();
                bodyBuilder.AppendLine($"<p>Olá, {nomeDestino}</p>");
                bodyBuilder.AppendLine($"<p>{mensagem}</p>");
                bodyBuilder.AppendLine($"<p><a href='{link}' style='background:#007bff;color:#fff;padding:10px 20px;text-decoration:none;border-radius:5px;'>{textoBotao}</a></p>");

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(remetente, "Equipe Alure"),
                    Subject = assunto,
                    Body = bodyBuilder.ToString(),
                    IsBodyHtml = true
                };

                mailMessage.To.Add(emailDestino);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
