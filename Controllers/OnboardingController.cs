using HRM.Services.HR;
using HRM.ViewModels.HR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers
{
    [Authorize]
    public class OnboardingController : Controller
    {
        private readonly IOnboardingService _onboardingService;

        public OnboardingController(IOnboardingService onboardingService)
        {
            _onboardingService = onboardingService;
        }

        // For HR to manage Tasks
        [Authorize(Roles = "SuperAdmin,HRManager,TalentAcquisition")]
        public async Task<IActionResult> ManageTasks()
        {
            var tasks = await _onboardingService.GetAllTasksAsync();
            return View(tasks);
        }

        [Authorize(Roles = "SuperAdmin,HRManager,TalentAcquisition")]
        [HttpPost]
        public async Task<IActionResult> CreateTask(OnboardingTaskVM model)
        {
            if (ModelState.IsValid)
            {
                await _onboardingService.CreateTaskAsync(model);
            }
            return RedirectToAction(nameof(ManageTasks));
        }

        // For Employee to view their progress
        public async Task<IActionResult> MyOnboarding()
        {
            // Get current user employee ID
            // Implementation detail: need to get EmployeeID from User Claims or User Manager
            // For now, placeholder or strictly typed if we implemented CurrentUser service
            // Assuming we pass ID or have helper
            return View(); 
            // Real implementation needs User.Identity.Name -> UserManager -> EmployeeId
        }
        
        // For Manager/HR to view specific employee progress
        public async Task<IActionResult> EmployeeProgress(int id)
        {
            var vm = await _onboardingService.GetEmployeeOnboardingStatusAsync(id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteTask(int employeeId, int taskId)
        {
            await _onboardingService.CompleteTaskAsync(employeeId, taskId);
            return RedirectToAction(nameof(EmployeeProgress), new { id = employeeId });
        }
    }
}
