using HRM.Services.Time;
using HRM.ViewModels.Time;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HRM.Models;

namespace HRM.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AttendanceController(IAttendanceService attendanceService, UserManager<ApplicationUser> userManager)
        {
            _attendanceService = attendanceService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Show Check-in/Check-out buttons and today's status
            var user = await _userManager.GetUserAsync(User);
            if (user?.EmployeeId == null) return RedirectToAction("AccessDenied", "Account");

            ViewBag.HasCheckedIn = await _attendanceService.HasCheckedInTodayAsync(user.EmployeeId.Value);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckIn(AttendanceCheckInVM model)
        {
            var user = await _userManager.GetUserAsync(User);
             if (user?.EmployeeId == null) return RedirectToAction("AccessDenied", "Account");

            await _attendanceService.CheckInAsync(user.EmployeeId.Value, model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(AttendanceCheckInVM model)
        {
             var user = await _userManager.GetUserAsync(User);
             if (user?.EmployeeId == null) return RedirectToAction("AccessDenied", "Account");

            await _attendanceService.CheckOutAsync(user.EmployeeId.Value, model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> History(int? month, int? year)
        {
            var user = await _userManager.GetUserAsync(User);
             if (user?.EmployeeId == null) return RedirectToAction("AccessDenied", "Account");

            var m = month ?? DateTime.Now.Month;
            var y = year ?? DateTime.Now.Year;

            var history = await _attendanceService.GetHistoryAsync(user.EmployeeId.Value, m, y);
            return View(history);
        }

        public async Task<IActionResult> RegisterFace()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.EmployeeId == null) return RedirectToAction("AccessDenied", "Account");
            
            var descriptor = await _attendanceService.GetFaceDescriptorAsync(user.EmployeeId.Value);
            ViewBag.IsRegistered = !string.IsNullOrEmpty(descriptor);
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveFaceDescriptor([FromBody] string descriptor)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.EmployeeId == null) return BadRequest("User or Employee not found");

            var result = await _attendanceService.RegisterFaceAsync(user.EmployeeId.Value, descriptor);
            if (result) return Ok();
            return BadRequest("Failed to save face descriptor");
        }

        public async Task<IActionResult> FaceCheckIn()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.EmployeeId == null) return RedirectToAction("AccessDenied", "Account");

            var descriptor = await _attendanceService.GetFaceDescriptorAsync(user.EmployeeId.Value);
            if (string.IsNullOrEmpty(descriptor))
            {
                return RedirectToAction(nameof(RegisterFace));
            }

            ViewBag.FaceDescriptor = descriptor;
            ViewBag.HasCheckedIn = await _attendanceService.HasCheckedInTodayAsync(user.EmployeeId.Value);
            return View();
        }
    }
}
