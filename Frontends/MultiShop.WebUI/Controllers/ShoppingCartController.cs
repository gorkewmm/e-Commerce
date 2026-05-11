using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.BasketDtos;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;

namespace MultiShop.WebUI.Controllers
{
    public class ShoppingCartController : Controller
    {
        private const decimal TaxRate = 0.10m;
        private static readonly CultureInfo TrCulture = CultureInfo.GetCultureInfo("tr-TR");

        private readonly IProductService _productService;
        private readonly IBasketService _basketService;

        public ShoppingCartController(IProductService productService, IBasketService basketService)
        {
            _productService = productService;
            _basketService = basketService;
        }

        public async Task<IActionResult> Index(string code, int discountRate, decimal totalNewPriceWithDiscount)
        {
            ViewBag.directory1 = "Ana Sayfa";
            ViewBag.directory2 = "Ürünler";
            ViewBag.directory3 = "Sepetim";

            var basket = await _basketService.GetBasket() ?? new BasketTotalDto { BasketItems = new List<BasketItemDto>() };
            basket.BasketItems ??= new List<BasketItemDto>();

            var summary = ComputeSummary(basket, code, discountRate, totalNewPriceWithDiscount);
            ApplyToViewBag(summary, code);

            return View();
        }

        public async Task<IActionResult> AddBasketItem(string id, string returnUrl = null)
        {
            try
            {
                var values = await _productService.GetByIdProductAsync(id);
                if (values == null)
                {
                    TempData["BasketError"] = "Ürün bulunamadığı için sepete eklenemedi.";
                }
                else
                {
                    var items = new BasketItemDto
                    {
                        ProductId = values.ProductId,
                        ProductName = values.ProductName,
                        Price = values.ProductPrice,
                        Quantity = 1,
                        ProductImageUrl = values.ProductImageUrl
                    };
                    await _basketService.AddBasketItem(items);
                    TempData["BasketSuccess"] = $"\"{values.ProductName}\" sepete eklendi.";
                }
            }
            catch
            {
                TempData["BasketError"] = "Ürün sepete eklenemedi. Lütfen tekrar deneyin.";
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveBasketItem(string id, string returnUrl = null)
        {
            await _basketService.RemoveBasketItem(id);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddBasketItemJson(string id, int quantity = 1)
        {
            if (quantity <= 0) quantity = 1;
            if (quantity > 99) quantity = 99;

            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Sepete ürün eklemek için giriş yapmanız gerekiyor.", requiresLogin = true });
            }

            try
            {
                var values = await _productService.GetByIdProductAsync(id);
                if (values == null)
                {
                    return Json(new { success = false, message = "Ürün bulunamadı." });
                }

                var item = new BasketItemDto
                {
                    ProductId = values.ProductId,
                    ProductName = values.ProductName,
                    Price = values.ProductPrice,
                    Quantity = quantity,
                    ProductImageUrl = values.ProductImageUrl
                };
                await _basketService.AddBasketItem(item);

                var basket = await _basketService.GetBasket();
                var summary = ComputeSummary(basket, null, 0, 0);

                var message = quantity > 1
                    ? $"\"{values.ProductName}\" sepete {quantity} adet eklendi."
                    : $"\"{values.ProductName}\" sepete eklendi.";

                return Json(new
                {
                    success = true,
                    message,
                    basketCount = summary.BasketCount,
                    summary = ToSummaryDto(summary)
                });
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized
                                                 || ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return Json(new { success = false, message = "Oturumunuzun süresi dolmuş olabilir. Lütfen tekrar giriş yapın.", requiresLogin = true });
            }
            catch
            {
                return Json(new { success = false, message = "Ürün sepete eklenemedi." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBasketItemQuantityJson(string id, int delta)
        {
            try
            {
                var (removed, newQuantity) = await _basketService.UpdateBasketItemQuantity(id, delta);

                var basket = await _basketService.GetBasket();
                var summary = ComputeSummary(basket, null, 0, 0);

                var item = basket.BasketItems.FirstOrDefault(x => x.ProductId == id);
                var lineTotal = item == null ? 0m : item.Price * item.Quantity;

                return Json(new
                {
                    success = true,
                    removed,
                    productId = id,
                    newQuantity,
                    lineTotal = lineTotal.ToString("N2", TrCulture),
                    basketCount = summary.BasketCount,
                    summary = ToSummaryDto(summary)
                });
            }
            catch
            {
                return Json(new { success = false, message = "Miktar güncellenemedi." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveBasketItemJson(string id)
        {
            try
            {
                await _basketService.RemoveBasketItem(id);

                var basket = await _basketService.GetBasket();
                var summary = ComputeSummary(basket, null, 0, 0);

                return Json(new
                {
                    success = true,
                    productId = id,
                    basketCount = summary.BasketCount,
                    summary = ToSummaryDto(summary)
                });
            }
            catch
            {
                return Json(new { success = false, message = "Ürün sepetten kaldırılamadı." });
            }
        }

        private static SummaryResult ComputeSummary(BasketTotalDto basket, string code, int discountRate, decimal totalNewPriceWithDiscount)
        {
            basket ??= new BasketTotalDto { BasketItems = new List<BasketItemDto>() };
            basket.BasketItems ??= new List<BasketItemDto>();

            var subtotal = basket.TotalPrice;
            var tax = subtotal * TaxRate;
            var totalWithTax = subtotal + tax;

            if (string.IsNullOrWhiteSpace(code) || discountRate <= 0)
            {
                discountRate = 0;
                totalNewPriceWithDiscount = totalWithTax;
            }

            var discountAmount = totalWithTax - totalNewPriceWithDiscount;
            if (discountAmount < 0) discountAmount = 0;

            return new SummaryResult
            {
                BasketCount = basket.BasketItems.Count,
                Subtotal = subtotal,
                Tax = tax,
                TotalWithTax = totalWithTax,
                DiscountRate = discountRate,
                DiscountAmount = discountAmount,
                FinalTotal = totalNewPriceWithDiscount
            };
        }

        private void ApplyToViewBag(SummaryResult summary, string code)
        {
            ViewBag.code = code;
            ViewBag.discountRate = summary.DiscountRate;
            ViewBag.subtotal = summary.Subtotal.ToString("N2", TrCulture);
            ViewBag.total = summary.Subtotal.ToString("N2", TrCulture);
            ViewBag.tax = summary.Tax.ToString("N2", TrCulture);
            ViewBag.totalPriceWithTax = summary.TotalWithTax.ToString("N2", TrCulture);
            ViewBag.discountAmount = summary.DiscountAmount.ToString("N2", TrCulture);
            ViewBag.totalNewPriceWithDiscount = summary.FinalTotal.ToString("N2", TrCulture);
        }

        private static object ToSummaryDto(SummaryResult s) => new
        {
            subtotal = s.Subtotal.ToString("N2", TrCulture),
            tax = s.Tax.ToString("N2", TrCulture),
            totalPriceWithTax = s.TotalWithTax.ToString("N2", TrCulture),
            discountRate = s.DiscountRate,
            discountAmount = s.DiscountAmount.ToString("N2", TrCulture),
            totalNewPriceWithDiscount = s.FinalTotal.ToString("N2", TrCulture),
            basketCount = s.BasketCount
        };

        private class SummaryResult
        {
            public int BasketCount { get; set; }
            public decimal Subtotal { get; set; }
            public decimal Tax { get; set; }
            public decimal TotalWithTax { get; set; }
            public int DiscountRate { get; set; }
            public decimal DiscountAmount { get; set; }
            public decimal FinalTotal { get; set; }
        }
    }
}
