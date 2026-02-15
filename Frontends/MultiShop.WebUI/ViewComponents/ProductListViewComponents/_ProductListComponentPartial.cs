using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using Newtonsoft.Json;

namespace MultiShop.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListComponentPartial : ViewComponent
    {
        private readonly IProductService _productService;
        public _ProductListComponentPartial(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var values = await _productService.GetProductsWithCategoryByCategoryIdAsync(id);
            return View(values);

            //var client = _httpClientFactory.CreateClient(); //İsteği atacak istemciyi(client) oluştur..
            //var responseMeassage = await client.GetAsync("https://localhost:7070/api/Products/ProductListWithCategoryByCategoryId?id="+id);//get isteği atarız ve bir nesne (paket) döner.
            //                                                                                      //bu paketin içersinde--> status, header ve content alanları var
            //if (responseMeassage.IsSuccessStatusCode)
            //{
            //    string jsonData = await responseMeassage.Content.ReadAsStringAsync();
            //    var values = JsonConvert.DeserializeObject<List<ResultProductWithCategoryDto>>(jsonData);
            //    return View(values);
            //}

            //return View();
        }
    }
}
