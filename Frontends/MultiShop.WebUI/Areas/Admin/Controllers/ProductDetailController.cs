using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductDetailDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductDetailServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/ProductDetail")]
    public class ProductDetailController : Controller
    {
        private readonly IProductDetailService _productDetailService;

        public ProductDetailController(IProductDetailService productDetailService)
        {
            _productDetailService = productDetailService;
        }

        [Route("UpdateProductDetail/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateProductDetail(string id)
        {
            ViewBag.v0 = "Ürün işlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Açıklama ve Bilgi Güncelleme Sayfası";

            var existing = await _productDetailService.GetByProductIdProductDetailAsync(id);
            if (existing != null)
            {
                var model = new UpdateProductDetailDto
                {
                    ProductDetailId = existing.ProductDetailId,
                    ProductId = string.IsNullOrWhiteSpace(existing.ProductId) ? id : existing.ProductId,
                    ProductDescription = existing.ProductDescription,
                    ProductInfo = existing.ProductInfo
                };
                return View(model);
            }

            return View(new UpdateProductDetailDto { ProductId = id });
        }

        [Route("UpdateProductDetail/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateProductDetail(string id, UpdateProductDetailDto _updateProductDetailDto)
        {
            if (string.IsNullOrWhiteSpace(_updateProductDetailDto.ProductId))
            {
                _updateProductDetailDto.ProductId = id;
            }

            if (string.IsNullOrWhiteSpace(_updateProductDetailDto.ProductDetailId))
            {
                var createDto = new CreateProductDetailDto
                {
                    ProductId = _updateProductDetailDto.ProductId,
                    ProductDescription = _updateProductDetailDto.ProductDescription,
                    ProductInfo = _updateProductDetailDto.ProductInfo
                };
                await _productDetailService.CreateProductDetailAsync(createDto);
            }
            else
            {
                await _productDetailService.UpdateProductDetailAsync(_updateProductDetailDto);
            }

            return RedirectToAction("ProductListWithCategory", "Product", new { area = "Admin" });
        }
    }
}
