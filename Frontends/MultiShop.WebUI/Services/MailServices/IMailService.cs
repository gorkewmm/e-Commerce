using MultiShop.WebUI.Models;

namespace MultiShop.WebUI.Services.MailServices
{
    public interface IMailService
    {
        Task SendAsync(MailRequest mailRequest, bool isHtml = false);
    }
}
