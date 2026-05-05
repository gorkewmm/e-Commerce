using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.DiscountServices;

namespace MultiShop.WebUI.Controllers
{
    public class DiscountController : Controller
    {
        private const decimal TaxRate = 0.10m;

        private readonly IDiscountService _discountService;
        private readonly IBasketService _basketService;

        public DiscountController(IDiscountService discountService, IBasketService basketService)
        {
            _discountService = discountService;
            _basketService = basketService;
        }

        [HttpGet]
        public PartialViewResult ConfirmDiscountCoupon()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDiscountCoupon(string code)
        {
            var basket = await _basketService.GetBasket();
            var subtotal = basket?.TotalPrice ?? 0m;
            var totalWithTax = subtotal + (subtotal * TaxRate);

            int discountRate = 0;

            if (!string.IsNullOrWhiteSpace(code))
            {
                try
                {
                    discountRate = await _discountService.GetDiscountCouponCountRate(code);
                }
                catch
                {
                    discountRate = 0;
                }
            }

            var totalNewPriceWithDiscount = discountRate > 0
                ? totalWithTax - (totalWithTax * discountRate / 100m)
                : totalWithTax;

            return RedirectToAction("Index", "ShoppingCart", new
            {
                code,
                discountRate,
                totalNewPriceWithDiscount
            });
        }
    }
}
