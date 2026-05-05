using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductImageDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductImageServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/ProductImage")]
    public class ProductImageController : Controller
    {
        private readonly IProductImageService _productImageService;

        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [Route("ProductImageDetail/{id}")]
        [HttpGet]
        public async Task<IActionResult> ProductImageDetail(string id)
        {
            ViewBag.v0 = "Ürün Görsek işlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Görsel İşlemleri";

            var existing = await _productImageService.GetByProductIdProductImageAsync(id);
            if (existing != null)
            {
                var model = new UpdateProductImageDto
                {
                    ProductImageID = existing.ProductImageID,
                    ProductId = string.IsNullOrWhiteSpace(existing.ProductId) ? id : existing.ProductId,
                    Image1 = existing.Image1,
                    Image2 = existing.Image2,
                    Image3 = existing.Image3,
                    Image4 = existing.Image4
                };
                return View(model);
            }

            return View(new UpdateProductImageDto { ProductId = id });
        }

        [Route("ProductImageDetail/{id}")]
        [HttpPost]
        public async Task<IActionResult> ProductImageDetail(string id, UpdateProductImageDto _updateProductImageDto)
        {
            if (string.IsNullOrWhiteSpace(_updateProductImageDto.ProductId))
            {
                _updateProductImageDto.ProductId = id;
            }

            if (string.IsNullOrWhiteSpace(_updateProductImageDto.ProductImageID))
            {
                var createDto = new CreateProductImageDto
                {
                    ProductId = _updateProductImageDto.ProductId,
                    Image1 = _updateProductImageDto.Image1,
                    Image2 = _updateProductImageDto.Image2,
                    Image3 = _updateProductImageDto.Image3,
                    Image4 = _updateProductImageDto.Image4
                };
                await _productImageService.CreateProductImageAsync(createDto);
            }
            else
            {
                await _productImageService.UpdateProductImageAsync(_updateProductImageDto);
            }

            return RedirectToAction("ProductListWithCategory", "Product", new { area = "Admin" });
        }
    }
}
