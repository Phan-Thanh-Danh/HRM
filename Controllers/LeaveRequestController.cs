using HRM.Services.Time;
using HRM.ViewModels.Time;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HRM.Models;

namespace HRM.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveService _leaveService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LeaveRequestController(ILeaveService leaveService, UserManager<ApplicationUser> userManager)
        {
            _leaveService = leaveService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.EmployeeId == null) return RedirectToAction("AccessDenied", "Account");

            var requests = await _leaveService.GetMyRequestsAsync(user.EmployeeId.Value);
            return View(requests);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(LeaveRequestVM model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.EmployeeId == null) return RedirectToAction("AccessDenied", "Account");

            if (ModelState.IsValid)
            {
                model.EmployeeId = user.EmployeeId.Value;
                await _leaveService.CreateRequestAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "SuperAdmin,HRManager,DepartmentManager")]
        public async Task<IActionResult> Manage()
        {
             var user = await _userManager.GetUserAsync(User);
            if (user?.EmployeeId == null) return RedirectToAction("AccessDenied", "Account");

            var pendingRequests = await _leaveService.GetPendingRequestsAsync(user.EmployeeId.Value);
            return View(pendingRequests);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,HRManager,DepartmentManager")]
        public async Task<IActionResult> Approve(int id)
        {
            var user = await _userManager.GetUserAsync(User);
             if (user?.EmployeeId == null) return RedirectToAction("AccessDenied", "Account");

            await _leaveService.ApproveRequestAsync(id, user.EmployeeId.Value);
            return RedirectToAction(nameof(Manage));
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,HRManager,DepartmentManager")]
        public async Task<IActionResult> Reject(int id, string reason)
        {
             var user = await _userManager.GetUserAsync(User);
             if (user?.EmployeeId == null) return RedirectToAction("AccessDenied", "Account");

            await _leaveService.RejectRequestAsync(id, user.EmployeeId.Value, reason);
            return RedirectToAction(nameof(Manage));
        }
    }
}
