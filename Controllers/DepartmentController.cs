using HRM.Services.HR;
using HRM.ViewModels.HR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers
{
    [Authorize(Roles = "SuperAdmin,HRManager")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _departmentService.GetAllAsync();
            return View(departments);
        }

        public async Task<IActionResult> OrgChart()
        {
            var departments = await _departmentService.GetOrgChartAsync();
            return View(departments);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Populate Dropdowns if needed (Managers, Parent Departments) in ViewBag or ViewModel
            ViewBag.ParentDepartments = await _departmentService.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentVM model)
        {
            if (ModelState.IsValid)
            {
                await _departmentService.CreateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ParentDepartments = await _departmentService.GetAllAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null) return NotFound();
            
            ViewBag.ParentDepartments = await _departmentService.GetAllAsync();
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentVM model)
        {
            if (ModelState.IsValid)
            {
                await _departmentService.UpdateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ParentDepartments = await _departmentService.GetAllAsync();
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _departmentService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
