using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.MailServices;

namespace MultiShop.WebUI.Controllers
{
    public class PaymentController : Controller
    {
        private const decimal TaxRate = 0.10m;
        private static readonly CultureInfo TrCulture = CultureInfo.GetCultureInfo("tr-TR");

        private readonly IUserService _userService;
        private readonly IBasketService _basketService;
        private readonly IMailService _mailService;

        public PaymentController(IUserService userService, IBasketService basketService, IMailService mailService)
        {
            _userService = userService;
            _basketService = basketService;
            _mailService = mailService;
        }

        public IActionResult Index()
        {
            ViewBag.directory1 = "MultiShop";
            ViewBag.directory2 = "Ödeme Ekranı";
            ViewBag.directory3 = "Kartla Ödeme";
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompletePayment()
        {
            var basket = await _basketService.GetBasket();
            basket.BasketItems ??= new List<MultiShop.DtoLayer.BasketDtos.BasketItemDto>();

            if (basket.BasketItems.Count == 0)
            {
                TempData["BasketError"] = "Sepetinizde ürün olmadığı için ödeme yapılamadı.";
                return RedirectToAction("Index", "ShoppingCart");
            }

            UserDetailViewModel user = null;
            try
            {
                user = await _userService.GetUserInfo();
            }
            catch
            {
            }

            if (user == null || string.IsNullOrWhiteSpace(user.Email))
            {
                TempData["BasketError"] = "Kullanıcı bilgisi alınamadığı için bilgilendirme e-postası gönderilemedi. Ödeme yine de tamamlanmış olabilir.";
                return RedirectToAction("Success", new { total = basket.TotalPrice + (basket.TotalPrice * TaxRate) });
            }

            var subtotal = basket.TotalPrice;
            var tax = subtotal * TaxRate;
            var totalWithTax = subtotal + tax;
            var fullName = $"{user.Name} {user.Surname}".Trim();
            if (string.IsNullOrWhiteSpace(fullName)) fullName = user.Username ?? "Değerli müşterimiz";

            var html = BuildOrderConfirmationHtml(fullName, basket.BasketItems, subtotal, tax, totalWithTax);

            try
            {
                await _mailService.SendAsync(new MailRequest
                {
                    ReceiverMail = user.Email,
                    Subject = "MultiShop - Sipariş Alındı",
                    MessageContent = html
                }, isHtml: true);

                try
                {
                    foreach (var item in basket.BasketItems.ToList())
                    {
                        await _basketService.RemoveBasketItem(item.ProductId);
                    }
                }
                catch
                {
                }

                return RedirectToAction("Success", new { total = totalWithTax });
            }
            catch
            {
                TempData["BasketError"] = "Ödeme tamamlandı fakat bilgilendirme e-postası gönderilemedi.";
                return RedirectToAction("Success", new { total = totalWithTax });
            }
        }

        [Authorize]
        public IActionResult Success(decimal total)
        {
            ViewBag.directory1 = "MultiShop";
            ViewBag.directory2 = "Ödeme";
            ViewBag.directory3 = "Sipariş Tamamlandı";
            ViewBag.Total = total.ToString("N2", TrCulture);
            return View();
        }

        private static string BuildOrderConfirmationHtml(
            string fullName,
            IEnumerable<MultiShop.DtoLayer.BasketDtos.BasketItemDto> items,
            decimal subtotal,
            decimal tax,
            decimal total)
        {
            var sb = new StringBuilder();
            sb.Append("<div style=\"font-family:Arial,Helvetica,sans-serif;color:#333;max-width:640px;margin:auto;\">");
            sb.Append("<h2 style=\"color:#f5b301;margin-bottom:0;\">MultiShop</h2>");
            sb.Append("<p style=\"margin-top:4px;\">Sipariş Onayı</p><hr/>");
            sb.AppendFormat("<p>Merhaba <b>{0}</b>,</p>", System.Net.WebUtility.HtmlEncode(fullName));
            sb.Append("<p>MultiShop mağazamızdan yaptığınız alışveriş için teşekkür ederiz. Sipariş özetiniz aşağıdadır:</p>");
            sb.Append("<table cellpadding=\"8\" cellspacing=\"0\" style=\"width:100%;border-collapse:collapse;border:1px solid #eee;\">");
            sb.Append("<thead><tr style=\"background:#f7f7f7;\"><th align=\"left\">Ürün</th><th align=\"center\">Adet</th><th align=\"right\">Birim Fiyat</th><th align=\"right\">Tutar</th></tr></thead><tbody>");
            foreach (var i in items)
            {
                var line = i.Price * i.Quantity;
                sb.Append("<tr style=\"border-top:1px solid #eee;\">");
                sb.AppendFormat("<td>{0}</td>", System.Net.WebUtility.HtmlEncode(i.ProductName ?? string.Empty));
                sb.AppendFormat("<td align=\"center\">{0}</td>", i.Quantity);
                sb.AppendFormat("<td align=\"right\">{0} ₺</td>", i.Price.ToString("N2", TrCulture));
                sb.AppendFormat("<td align=\"right\">{0} ₺</td>", line.ToString("N2", TrCulture));
                sb.Append("</tr>");
            }
            sb.Append("</tbody></table>");
            sb.Append("<table cellpadding=\"6\" cellspacing=\"0\" style=\"width:100%;margin-top:12px;\">");
            sb.AppendFormat("<tr><td>Ara Toplam</td><td align=\"right\">{0} ₺</td></tr>", subtotal.ToString("N2", TrCulture));
            sb.AppendFormat("<tr><td>KDV (%10)</td><td align=\"right\">{0} ₺</td></tr>", tax.ToString("N2", TrCulture));
            sb.AppendFormat("<tr style=\"font-size:16px;font-weight:bold;border-top:2px solid #333;\"><td>Genel Toplam</td><td align=\"right\">{0} ₺</td></tr>", total.ToString("N2", TrCulture));
            sb.Append("</table>");
            sb.Append("<p style=\"margin-top:18px;\">Siparişiniz hazırlanmaya başlandı. Kargoya verildiğinde sizi tekrar bilgilendireceğiz.</p>");
            sb.Append("<p style=\"color:#888;font-size:12px;\">Bu e-posta MultiShop tarafından otomatik olarak gönderilmiştir.</p>");
            sb.Append("</div>");
            return sb.ToString();
        }
    }
}
