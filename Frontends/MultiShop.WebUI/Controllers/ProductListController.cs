using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CommentDtos;
using MultiShop.WebUI.Services.CommentServices;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Controllers
{
    public class ProductListController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;

        public ProductListController(ICommentService commentService, IUserService userService)
        {
            _commentService = commentService;
            _userService = userService;
        }

        public IActionResult Index(string id)
        {
            ViewBag.directory1 = "Ana Sayfa";
            ViewBag.directory2 = "Ürünler";
            ViewBag.directory3 = "Ürün Listesi";
            ViewBag.i = id;
            return View();
        }

        public IActionResult ProductDetail(string id)
        {
            ViewBag.directory1 = "Ana Sayfa";
            ViewBag.directory2 = "Ürün Listesi";
            ViewBag.directory3 = "Ürün Detayları";
            ViewBag.x = id;
            return View();
        }

        [HttpGet]
        public PartialViewResult AddComment()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CreateCommentDto createCommentDto)
        {
            // Kullanıcı bilgilerini Identity Server'dan otomatik al
            var user = await _userService.GetUserInfo();
            createCommentDto.NameSurname = $"{user.Name} {user.Surname}".Trim();
            createCommentDto.Email = user.Email;
            createCommentDto.ImageUrl = "test";
            createCommentDto.CreatedDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            createCommentDto.Status = false;
            // ProductId formdan gelen hidden input değeriyle gelir, sabit değer KALDIRILDI

            var productId = createCommentDto.ProductId;

            try
            {
                await _commentService.CreateCommentAsync(createCommentDto);
            }
            catch (Exception)
            {
                TempData["CommentError"] = "Yorum servisi şu anda erişilemiyor. Lütfen daha sonra tekrar deneyin.";
            }

            return RedirectToAction("ProductDetail", "ProductList", new { id = productId });
        }
    }
}
