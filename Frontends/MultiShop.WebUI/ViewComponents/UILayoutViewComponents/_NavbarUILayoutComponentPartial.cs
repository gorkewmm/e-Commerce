using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;
using MultiShop.WebUI.Services.FavoriteServices;

namespace MultiShop.WebUI.ViewComponents.UILayoutViewComponents
{
    public class _NavbarUILayoutComponentPartial : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IFavoriteService _favoriteService;
        private readonly IBasketService _basketService;

        public _NavbarUILayoutComponentPartial(
            ICategoryService categoryService,
            IFavoriteService favoriteService,
            IBasketService basketService)
        {
            _categoryService = categoryService;
            _favoriteService = favoriteService;
            _basketService = basketService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _categoryService.GetAllCategoryAsync() ?? new List<ResultCategoryDto>();

            ViewBag.FavoriteCount = _favoriteService.Count();

            int basketCount = 0;
            try
            {
                if (User?.Identity != null && User.Identity.IsAuthenticated)
                {
                    var basket = await _basketService.GetBasket();
                    basketCount = basket?.BasketItems?.Count ?? 0;
                }
            }
            catch
            {
            }
            ViewBag.BasketCount = basketCount;

            return View(values);
        }
    }
}
