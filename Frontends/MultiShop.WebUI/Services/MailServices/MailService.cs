using MailKit.Net.Smtp;
using MimeKit;
using MultiShop.WebUI.Models;

namespace MultiShop.WebUI.Services.MailServices
{
    public class MailService : IMailService
    {
        private const string SmtpHost = "smtp.gmail.com";
        private const int SmtpPort = 587;
        private const string SenderName = "MultiShop";
        private const string SenderMail = "halilgorkemuysal@gmail.com";
        private const string SenderPassword = "sfjmypzviyemkidp";

        public async Task SendAsync(MailRequest mailRequest, bool isHtml = false)
        {
            if (mailRequest == null) return;
            if (string.IsNullOrWhiteSpace(mailRequest.ReceiverMail)) return;

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(SenderName, SenderMail));
            mimeMessage.To.Add(new MailboxAddress("Müşteri", mailRequest.ReceiverMail));
            mimeMessage.Subject = mailRequest.Subject ?? "MultiShop";

            var bodyBuilder = new BodyBuilder();
            if (isHtml)
            {
                bodyBuilder.HtmlBody = mailRequest.MessageContent;
            }
            else
            {
                bodyBuilder.TextBody = mailRequest.MessageContent;
            }
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(SmtpHost, SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(SenderMail, SenderPassword);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }
    }
}
