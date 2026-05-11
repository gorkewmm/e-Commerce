using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.IdentityDtos.RegisterDtos;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegisterController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateRegisterDto createRegisterDto)
        {
            if (string.IsNullOrWhiteSpace(createRegisterDto.Username) ||
                string.IsNullOrWhiteSpace(createRegisterDto.Password))
            {
                ViewBag.RegisterError = "Kullanıcı adı ve şifre zorunludur.";
                return View(createRegisterDto);
            }

            if (createRegisterDto.Password != createRegisterDto.ConfirmPassword)
            {
                ViewBag.RegisterError = "Şifreler eşleşmiyor.";
                return View(createRegisterDto);
            }

            var client = _httpClientFactory.CreateClient();
            string jsonData = JsonConvert.SerializeObject(createRegisterDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("http://localhost:5001/api/Registers", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["RegisterSuccess"] = "Kayıt başarılı. Şimdi giriş yapabilirsiniz.";
                return RedirectToAction("Index", "Login");
            }

            var responseBody = await responseMessage.Content.ReadAsStringAsync();
            ViewBag.RegisterError = ParseErrorMessage(responseBody)
                ?? "Kayıt başarısız oldu. Lütfen şifrenin uzunluğunu, büyük/küçük harf, rakam ve özel karakter içerip içermediğini kontrol edin.";

            return View(createRegisterDto);
        }

        private static string ParseErrorMessage(string body)
        {
            if (string.IsNullOrWhiteSpace(body)) return null;

            try
            {
                dynamic obj = JsonConvert.DeserializeObject(body);
                if (obj == null) return null;

                string message = obj.message;
                var errorsList = new List<string>();

                if (obj.errors != null)
                {
                    foreach (var err in obj.errors)
                    {
                        errorsList.Add((string)err);
                    }
                }

                if (errorsList.Count > 0)
                {
                    return $"{message} ({string.Join(" | ", errorsList)})";
                }
                return message;
            }
            catch
            {
                return null;
            }
        }
    }
}
