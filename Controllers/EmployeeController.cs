using HRM.Services.HR;
using HRM.ViewModels.HR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllAsync();
            return View(employees);
        }

        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [Authorize(Roles = "SuperAdmin,HRManager")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _departmentService.GetAllAsync();
            return View();
        }

        [Authorize(Roles = "SuperAdmin,HRManager")]
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeVM model)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload if AvatarFile is present
                if (model.AvatarFile != null)
                {
                    // Simple file upload logic (should be a service)
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.AvatarFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/avatars", fileName);
                    
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.AvatarFile.CopyToAsync(stream);
                    }
                    model.AvatarUrl = "/uploads/avatars/" + fileName;
                }

                await _employeeService.CreateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = await _departmentService.GetAllAsync();
            return View(model);
        }

        // Edit and Delete actions would be similar...
    }
}
