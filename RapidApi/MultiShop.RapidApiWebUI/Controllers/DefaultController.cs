using Microsoft.AspNetCore.Mvc;
using MultiShop.RapidApiWebUI.Models;
using Newtonsoft.Json;

namespace MultiShop.RapidApiWebUI.Controllers
{
    public class DefaultController : Controller
    {
        public async Task<IActionResult> WeatherDetail()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://weatherapi-com.p.rapidapi.com/current.json?q=ankara"),
                Headers =
    {
        { "x-rapidapi-key", "4848a7b720msh8873c3d27af4f29p13666ejsnb86638b91187" },
        { "x-rapidapi-host", "weatherapi-com.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                // JSON'ı senin yazdığın modeldeki Rootobject sınıfına dönüştürüyoruz
                var weatherData = JsonConvert.DeserializeObject<WeatherViewModel.Rootobject>(body);

                // Sadece sıcaklık lazımsa ViewBag ile gönderebilirsin
                ViewBag.cityTemp = weatherData.current.temp_c;

                // Tüm modeli View'a göndermek en sağlıklı yoldur
                return View(weatherData);
            }

        }

        public async Task<IActionResult> Exchange()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://real-time-finance-data.p.rapidapi.com/currency-exchange-rate?from_symbol=USD&to_symbol=TRY&language=en"),
                Headers =
    {
        { "x-rapidapi-key", "4848a7b720msh8873c3d27af4f29p13666ejsnb86638b91187" },
        { "x-rapidapi-host", "real-time-finance-data.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                var exchangeData = JsonConvert.DeserializeObject<ExchangeViewModel.Rootobject>(body);

                // Sadece sıcaklık lazımsa ViewBag ile gönderebilirsin
                ViewBag.exchangeRateUsd = exchangeData.data.exchange_rate;
                ViewBag.previous_closeUsd = exchangeData.data.previous_close;
            }


            var client2 = new HttpClient();
            var request2 = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://real-time-finance-data.p.rapidapi.com/currency-exchange-rate?from_symbol=EUR&to_symbol=TRY&language=en"),
                Headers =
    {
        { "x-rapidapi-key", "4848a7b720msh8873c3d27af4f29p13666ejsnb86638b91187" },
        { "x-rapidapi-host", "real-time-finance-data.p.rapidapi.com" },
    },
            };
            using (var response = await client2.SendAsync(request2))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                var exchangeData = JsonConvert.DeserializeObject<ExchangeViewModel.Rootobject>(body);

                // Sadece sıcaklık lazımsa ViewBag ile gönderebilirsin
                ViewBag.exchangeRateEur = exchangeData.data.exchange_rate;
                ViewBag.previous_closeEur = exchangeData.data.previous_close;
                return View();
            }
        }
    }
}
