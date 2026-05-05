using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CommentDtos;
using MultiShop.WebUI.Services.CommentServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/Comment")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.v0 = "Yorum işlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Yorumlar";
            ViewBag.v3 = "Yorum Listesi";

            try
            {
                var values = await _commentService.GetAllCommentAsync();
                return View(values ?? new List<ResultCommentDto>());
            }
            catch (Exception)
            {
                TempData["ServiceError"] = "Yorum servisi şu anda erişilemiyor.";
            }

            return View(new List<ResultCommentDto>());
        }

        [Route("DeleteComment/{id}")]
        public async Task<IActionResult> DeleteComment(string id)
        {
            try
            {
                await _commentService.DeleteCommentAsync(id);
            }
            catch (Exception)
            {
                TempData["ServiceError"] = "Yorum silinemedi, servis erişilemiyor.";
            }
            return RedirectToAction("Index", "Comment", new { area = "Admin" });
        }

        [Route("ApproveComment/{id}")]
        public async Task<IActionResult> ApproveComment(string id)
        {
            try
            {
                await _commentService.ApproveCommentAsync(id);
                TempData["SuccessMessage"] = "Yorum başarıyla onaylandı ve yayınlandı.";
            }
            catch (Exception)
            {
                TempData["ServiceError"] = "Yorum onaylanamadı, servis erişilemiyor.";
            }
            return RedirectToAction("Index", "Comment", new { area = "Admin" });
        }

        [Route("UpdateComment/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateComment(string id)
        {
            ViewBag.v0 = "Yorum işlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Yorumlar";
            ViewBag.v3 = "Yorum Listesi";

            try
            {
                var values = await _commentService.GetByIdCommentAsync(id);
                return View(values);
            }
            catch (Exception)
            {
                TempData["ServiceError"] = "Yorum bilgisi alınamadı, servis erişilemiyor.";
            }
            return RedirectToAction("Index", "Comment", new { area = "Admin" });
        }

        [Route("UpdateComment/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateComment(UpdateCommentDto _updateCommentDto)
        {
            _updateCommentDto.Status = true;
            try
            {
                await _commentService.UpdateCommentAsync(_updateCommentDto);
            }
            catch (Exception)
            {
                TempData["ServiceError"] = "Yorum güncellenemedi, servis erişilemiyor.";
            }
            return RedirectToAction("Index", "Comment", new { area = "Admin" });
        }
    }
}
