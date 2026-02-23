using HRM.Services.Learning;
using HRM.Services.HR;
using HRM.ViewModels.Learning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HRM.Models;

namespace HRM.Controllers
{
    [Authorize]
    public class LearningController : Controller
    {
        private readonly ILearningService _learningService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LearningController(ILearningService learningService, UserManager<ApplicationUser> userManager)
        {
            _learningService = learningService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.EmployeeId != null)
            {
                var myCourses = await _learningService.GetMyCoursesAsync(user.EmployeeId.Value);
                ViewBag.AllCourses = await _learningService.GetCoursesAsync(); // For browsing
                return View(myCourses);
            }
            return RedirectToAction("AccessDenied", "Account");
        }

        public async Task<IActionResult> CourseDetails(int id)
        {
            var course = await _learningService.GetCourseByIdAsync(id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.EmployeeId != null)
            {
                await _learningService.EnrollAsync(user.EmployeeId.Value, id);
                return RedirectToAction(nameof(Learn), new { id });
            }
            return RedirectToAction("AccessDenied", "Account");
        }

        public async Task<IActionResult> Learn(int id)
        {
             var course = await _learningService.GetCourseByIdAsync(id);
             if (course == null) return NotFound();
             
             ViewBag.Lessons = await _learningService.GetLessonsByCourseIdAsync(id);
             return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteLesson(int courseId, int lessonId) // Simplified progress
        {
             var user = await _userManager.GetUserAsync(User);
             if (user?.EmployeeId != null)
             {
                 // Mock progress update logic
                 await _learningService.UpdateProgressAsync(user.EmployeeId.Value, courseId, 100); // Mark course as done for now easiest way
             }
             return RedirectToAction(nameof(Index));
        }

        // Admin Management
        [Authorize(Roles = "SuperAdmin,HRManager")]
        public async Task<IActionResult> CreateCourse()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,HRManager")]
        public async Task<IActionResult> CreateCourse(CourseVM model)
        {
            if (ModelState.IsValid)
            {
                await _learningService.CreateCourseAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
