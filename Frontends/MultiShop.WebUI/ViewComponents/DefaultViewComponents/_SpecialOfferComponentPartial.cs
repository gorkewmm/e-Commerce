using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.SpecialOfferDtos;
using Newtonsoft.Json;

namespace MultiShop.WebUI.ViewComponents.DefaultViewComponents
{
    public class _SpecialOfferComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _SpecialOfferComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient(); //İsteği atacak istemciyi(client) oluştur..
            var responseMeassage = await client.GetAsync("https://localhost:7070/api/SpecialOffers");//get isteği atarız ve bir nesne (paket) döner.
                                                                                                     //bu paketin içersinde--> status, header ve content alanları var
            if (responseMeassage.IsSuccessStatusCode)
            {
                string jsonData = await responseMeassage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultSpecialOfferDto>>(jsonData);
                return View(values);
            }

            return View();
        }
    }
}
