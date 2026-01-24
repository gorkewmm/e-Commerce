using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.OfferDiscountDtos;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/OfferDiscount")]
    public class OfferDiscountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OfferDiscountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.v0 = "İndirim Teklif işlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "İndirim Teklifleri";
            ViewBag.v3 = "İndirim Teklif Listesi";

            var client = _httpClientFactory.CreateClient(); //İsteği atacak istemciyi(client) oluştur..
            var responseMeassage = await client.GetAsync("https://localhost:7070/api/OfferDiscounts");//get isteği atarız ve bir nesne (paket) döner.
                                                                                                  //bu paketin içersinde--> status, header ve content alanları var
            if (responseMeassage.IsSuccessStatusCode)
            {
                string jsonData = await responseMeassage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultOfferDiscountDto>>(jsonData);
                return View(values);
            }

            return View();
        }

        [HttpGet]
        [Route("CreateOfferDiscount")]
        public async Task<IActionResult> CreateOfferDiscount()
        {
            ViewBag.v0 = "İndirim Teklif işlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "İndirim Teklifleri";
            ViewBag.v3 = "İndirim Teklif Listesi";
            return View();
        }
        [HttpPost]
        [Route("CreateOfferDiscount")]
        public async Task<IActionResult> CreateOfferDiscount(CreateOfferDiscountDto _createOfferDiscountDto)
        {
            var client = _httpClientFactory.CreateClient();
            string jsonData = JsonConvert.SerializeObject(_createOfferDiscountDto);

            //http client, string kabul etmez. Yani senin elindeki JSON string’i doğrudan gönderilebilecek bir şey değildir.
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7070/api/OfferDiscounts", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "OfferDiscount", new { area = "Admin" });
            }
            return View();
        }
        [Route("DeleteOfferDiscount/{id}")]
        public async Task<IActionResult> DeleteOfferDiscount(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync("https://localhost:7070/api/OfferDiscounts?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "OfferDiscount", new { area = "Admin" });
            }
            return View();
        }

        [Route("UpdateOfferDiscount/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateOfferDiscount(string id)
        {
            ViewBag.v0 = "İndirim Teklif işlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "İndirim Teklifleri";
            ViewBag.v3 = "İndirim Teklif Listesi";
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7070/api/OfferDiscounts/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateOfferDiscountDto>(jsonData);
                return View(values);
            }
            return View();
        }

        [Route("UpdateOfferDiscount/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateOfferDiscount(UpdateOfferDiscountDto _updateOfferDiscountDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(_updateOfferDiscountDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PutAsync("https://localhost:7070/api/OfferDiscounts/", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "OfferDiscount", new { area = "Admin" });
            }
            return View();
        }
    }
}
