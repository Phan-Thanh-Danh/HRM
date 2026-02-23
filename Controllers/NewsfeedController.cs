using HRM.Services.Social;
using HRM.ViewModels.Social;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HRM.Models;

namespace HRM.Controllers
{
    [Authorize]
    public class NewsfeedController : Controller
    {
        private readonly INewsfeedService _newsfeedService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NewsfeedController(INewsfeedService newsfeedService, UserManager<ApplicationUser> userManager)
        {
            _newsfeedService = newsfeedService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _newsfeedService.GetFeedAsync();
            return View(posts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostVM model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.EmployeeId != null && !string.IsNullOrWhiteSpace(model.Content))
            {
                await _newsfeedService.CreatePostAsync(model, user.EmployeeId.Value);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int postId, string content)
        {
             var user = await _userManager.GetUserAsync(User);
             if (user?.EmployeeId != null && !string.IsNullOrWhiteSpace(content))
             {
                 await _newsfeedService.AddCommentAsync(postId, content, user.EmployeeId.Value);
             }
             return RedirectToAction(nameof(Index));
        }
    }
}
