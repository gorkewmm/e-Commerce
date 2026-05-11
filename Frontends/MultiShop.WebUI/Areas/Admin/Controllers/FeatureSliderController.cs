using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.DtoLayer.CatalogDtos.FeatureSliderDtos;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;
using MultiShop.WebUI.Services.CatalogServices.FeatureSliderServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/FeatureSlider")]
    public class FeatureSliderController : Controller
    {
        private readonly IFeatureSliderService _featureSliderService;
        private readonly ICategoryService _categoryService;

        public FeatureSliderController(IFeatureSliderService featureSliderService, ICategoryService categoryService)
        {
            _featureSliderService = featureSliderService;
            _categoryService = categoryService;
        }

        static List<SelectListItem> BuildCategorySelectList(IEnumerable<ResultCategoryDto> categories, string selectedCategoryId = null)
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem { Text = "— Kategori seçin (isteğe bağlı) —", Value = "", Selected = string.IsNullOrEmpty(selectedCategoryId) }
            };
            list.AddRange(categories.Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = x.CategoryId,
                Selected = !string.IsNullOrEmpty(selectedCategoryId) && x.CategoryId == selectedCategoryId
            }));
            return list;
        }

        void FeatureSliderViewBagList()
        {
            ViewBag.v0 = "Öne Çıkan Görsel İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Öne Çıkan Görseller";
            ViewBag.v3 = "Slider Öne Çıkan Görsel Listesi";
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            FeatureSliderViewBagList();
            var values = await _featureSliderService.GetAllFeatureSliderAsync();
            var categories = await _categoryService.GetAllCategoryAsync() ?? new List<ResultCategoryDto>();
            ViewBag.CategoryNameById = categories.ToDictionary(c => c.CategoryId, c => c.CategoryName);
            return View(values);
        }

        [HttpGet]
        [Route("CreateFeatureSlider")]
        public async Task<IActionResult> CreateFeatureSlider()
        {
            FeatureSliderViewBagList();
            var categories = await _categoryService.GetAllCategoryAsync() ?? new List<ResultCategoryDto>();
            ViewBag.CategoryValues = BuildCategorySelectList(categories);
            return View();
        }
        [HttpPost]
        [Route("CreateFeatureSlider")]
        public async Task<IActionResult> CreateFeatureSlider(CreateFeatureSliderDto _createFeatureSliderDto)
        {
            await _featureSliderService.CreateFeatureSliderAsync(_createFeatureSliderDto);
            return RedirectToAction("Index", "FeatureSlider", new { area = "Admin" });
        }

        [Route("DeleteFeatureSlider/{id}")]
        public async Task<IActionResult> DeleteFeatureSlider(string id)
        {
            await _featureSliderService.DeleteFeatureSliderAsync(id);
            return RedirectToAction("Index", "FeatureSlider", new { area = "Admin" });
        }

        [Route("UpdateFeatureSlider/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateFeatureSlider(string id)
        {
            FeatureSliderViewBagList();
            var values = await _featureSliderService.GetByIdFeatureSliderAsync(id);
            var categories = await _categoryService.GetAllCategoryAsync() ?? new List<ResultCategoryDto>();
            ViewBag.CategoryValues = BuildCategorySelectList(categories, values.CategoryId);
            return View(values);
        }

        [Route("UpdateFeatureSlider/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateFeatureSlider(UpdateFeatureSliderDto _updateFeatureSliderDto)
        {
            await _featureSliderService.UpdateFeatureSliderAsync(_updateFeatureSliderDto);
            return RedirectToAction("Index", "FeatureSlider", new { area = "Admin" });
        }
    }
}
