using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.FavoriteServices;

namespace MultiShop.WebUI.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IProductService _productService;

        public FavoriteController(IFavoriteService favoriteService, IProductService productService)
        {
            _favoriteService = favoriteService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.directory1 = "Ana Sayfa";
            ViewBag.directory2 = "Hesabım";
            ViewBag.directory3 = "Favorilerim";

            var ids = _favoriteService.GetFavoriteIds();
            if (ids.Count == 0)
            {
                return View(new List<ResultProductDto>());
            }

            var allProducts = await _productService.GetAllProductAsync() ?? new List<ResultProductDto>();
            var favorites = allProducts.Where(p => ids.Contains(p.ProductId)).ToList();
            return View(favorites);
        }

        public IActionResult AddFavorite(string id, string returnUrl = null)
        {
            _favoriteService.AddFavorite(id);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFavorite(string id, string returnUrl = null)
        {
            _favoriteService.RemoveFavorite(id);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddFavoriteJson(string id)
        {
            try
            {
                _favoriteService.AddFavorite(id);
                return Json(new
                {
                    success = true,
                    message = "Ürün favorilere eklendi.",
                    favoriteCount = _favoriteService.Count()
                });
            }
            catch
            {
                return Json(new { success = false, message = "Favorilere eklenemedi." });
            }
        }

        [HttpPost]
        public IActionResult RemoveFavoriteJson(string id)
        {
            try
            {
                _favoriteService.RemoveFavorite(id);
                return Json(new
                {
                    success = true,
                    productId = id,
                    favoriteCount = _favoriteService.Count()
                });
            }
            catch
            {
                return Json(new { success = false, message = "Favorilerden çıkarılamadı." });
            }
        }
    }
}
