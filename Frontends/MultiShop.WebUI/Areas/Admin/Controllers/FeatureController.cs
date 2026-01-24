using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.FeatureDtos;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/Feature")]
    public class FeatureController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FeatureController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.v0 = "Ana Sayfa Öne Çıkan Alan işlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Öne Çıkan Alanlar";
            ViewBag.v3 = "Öne Çıkan Alan Listesi";

            var client = _httpClientFactory.CreateClient(); //İsteği atacak istemciyi(client) oluştur..
            var responseMeassage = await client.GetAsync("https://localhost:7070/api/Features");//get isteği atarız ve bir nesne (paket) döner.
                                                                                                  //bu paketin içersinde--> status, header ve content alanları var
            if (responseMeassage.IsSuccessStatusCode)
            {
                string jsonData = await responseMeassage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultFeatureDto>>(jsonData);
                return View(values);
            }

            return View();
        }

        [HttpGet]
        [Route("CreateFeature")]
        public async Task<IActionResult> CreateFeature()
        {
            ViewBag.v0 = "Ana Sayfa Öne Çıkan Alan işlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Öne Çıkan Alanlar";
            ViewBag.v3 = "Öne Çıkan Alan Listesi";
            return View();
        }
        [HttpPost]
        [Route("CreateFeature")]
        public async Task<IActionResult> CreateFeature(CreateFeatureDto _createFeatureDto)
        {
            var client = _httpClientFactory.CreateClient();
            string jsonData = JsonConvert.SerializeObject(_createFeatureDto);

            //http client, string kabul etmez. Yani senin elindeki JSON string’i doğrudan gönderilebilecek bir şey değildir.
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7070/api/Features", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Feature", new { area = "Admin" });
            }
            return View();
        }
        [Route("DeleteFeature/{id}")]
        public async Task<IActionResult> DeleteFeature(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync("https://localhost:7070/api/Features?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Feature", new { area = "Admin" });
            }
            return View();
        }

        [Route("UpdateFeature/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateFeature(string id)
        {
            ViewBag.v0 = "Ana Sayfa Öne Çıkan Alan işlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Öne Çıkan Alanlar";
            ViewBag.v3 = "Öne Çıkan Alan Listesi";
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7070/api/Features/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateFeatureDto>(jsonData);
                return View(values);
            }
            return View();
        }

        [Route("UpdateFeature/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateFeature(UpdateFeatureDto _updateFeatureDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(_updateFeatureDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PutAsync("https://localhost:7070/api/Features/", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Feature", new { area = "Admin" });
            }
            return View();
        }
    }
}
