using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BusinessLogicLayer.Helper
{
    public class EmailServices
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailServices(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public void SendEmail(string emailTo, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_smtpSettings.FromEmail));
            email.To.Add(MailboxAddress.Parse(emailTo));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = body
            };

            using var smtp = new SmtpClient();
            try
            {
                smtp.Connect(
                    _smtpSettings.Host,
                    _smtpSettings.Port,
                    SecureSocketOptions.StartTls
                );
                smtp.Authenticate(_smtpSettings.Username, _smtpSettings.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Email sending failed: " + ex.Message);
            }
        }
    }
}
