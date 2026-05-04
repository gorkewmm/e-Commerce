using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MultiShop.WebUI.Models;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/Mail")]
    public class MailController : Controller
    {
        [HttpGet]
        [Route("SendMail")]
        public IActionResult SendMail()
        {
            ViewBag.v0 = "Mail İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Mail";
            ViewBag.v3 = "Mail Gönder";
            return View();
        }

        [HttpPost]
        [Route("SendMail")]
        public IActionResult SendMail(MailRequest mailRequest)
        {
            ViewBag.v0 = "Mail İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Mail";
            ViewBag.v3 = "Mail Gönder";

            try
            {
                MimeMessage mimeMessage = new MimeMessage();

                MailboxAddress mailboxAddressFrom = new MailboxAddress("MultiShop Admin", "halilgorkemuysal@gmail.com");
                mimeMessage.From.Add(mailboxAddressFrom);

                MailboxAddress mailboxAddressTo = new MailboxAddress("User", mailRequest.ReceiverMail);
                mimeMessage.To.Add(mailboxAddressTo);

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = mailRequest.MessageContent;
                mimeMessage.Body = bodyBuilder.ToMessageBody();
                mimeMessage.Subject = mailRequest.Subject;

                SmtpClient client = new SmtpClient();
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("halilgorkemuysal@gmail.com", "sfjmypzviyemkidp");
                client.Send(mimeMessage);
                client.Disconnect(true);

                TempData["MailSuccess"] = $"Mail başarıyla gönderildi: {mailRequest.ReceiverMail}";
            }
            catch (Exception ex)
            {
                TempData["MailError"] = $"Mail gönderilemedi: {ex.Message}";
            }

            return RedirectToAction("SendMail");
        }
    }
}
