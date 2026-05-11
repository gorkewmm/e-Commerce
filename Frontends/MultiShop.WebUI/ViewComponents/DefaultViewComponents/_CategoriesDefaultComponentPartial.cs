using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;

namespace MultiShop.WebUI.ViewComponents.DefaultViewComponents
{
    public class _CategoriesDefaultComponentPartial : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public _CategoriesDefaultComponentPartial(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetAllCategoryAsync();
            var products = await _productService.GetAllProductAsync();

            var productCountByCategoryId = products?
                .Where(p => !string.IsNullOrEmpty(p.CategoryId))
                .GroupBy(p => p.CategoryId)
                .ToDictionary(g => g.Key, g => g.Count())
                ?? new Dictionary<string, int>();

            ViewBag.ProductCountByCategoryId = productCountByCategoryId;
            return View(categories);
        }
    }
}
