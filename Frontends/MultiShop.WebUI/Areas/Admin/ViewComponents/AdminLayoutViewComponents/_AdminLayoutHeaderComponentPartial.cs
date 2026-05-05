using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CommentServices;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.MessageServices;

namespace MultiShop.WebUI.Areas.Admin.ViewComponents.AdminLayoutViewComponents
{
    public class _AdminLayoutHeaderComponentPartial : ViewComponent
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;

        public _AdminLayoutHeaderComponentPartial(IMessageService messageService, IUserService userService, ICommentService commentService)
        {
            _messageService = messageService;
            _userService = userService;
            _commentService = commentService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Kullanıcı bilgisini al
            var user = await _userService.GetUserInfo();

            // Mesaj sayısı ve yorum sayısını PARALEL olarak çek
            var messageCountTask = Task.Run(async () =>
            {
                try { return await _messageService.GetTotalMessageCountByReceiverId(user.Id); }
                catch { return 0; }
            });

            var commentCountTask = Task.Run(async () =>
            {
                try { return await _commentService.GetTotalCommentCount(); }
                catch { return 0; }
            });

            await Task.WhenAll(messageCountTask, commentCountTask);

            ViewBag.messageCount = messageCountTask.Result;
            ViewBag.totalCommentCount = commentCountTask.Result;

            return View();
        }
    }
}
