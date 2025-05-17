using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace LMS.Service.Services.Shared
{
    public class EmailService : IEmailService
    {
        private readonly string _gmailAddress = "yazanrabae78@gmail.com";
        private readonly string _appPassword = "wclz oiiz wkgl ijxc";
        public async Task SendEmailAsync(string receiverName, string receiverMail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("LMS", _gmailAddress));
            message.To.Add(new MailboxAddress(receiverName, receiverMail));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using var client = new SmtpClient();

            await client.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(_gmailAddress, _appPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
