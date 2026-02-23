using HRM.Services.HR;
using HRM.ViewModels.HR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers
{
    [Authorize(Roles = "SuperAdmin,HRManager,CBSpecialist")]
    public class ContractController : Controller
    {
        private readonly IContractService _contractService;
        private readonly IEmployeeService _employeeService;

        public ContractController(IContractService contractService, IEmployeeService employeeService)
        {
            _contractService = contractService;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var contracts = await _contractService.GetAllAsync();
            return View(contracts);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? employeeId)
        {
            ViewBag.Employees = await _employeeService.GetAllAsync();
            var model = new ContractVM();
            if (employeeId.HasValue)
            {
                model.EmployeeId = employeeId.Value;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContractVM model)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (model.ContractFile != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ContractFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/contracts", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ContractFile.CopyToAsync(stream);
                    }
                    model.FilePath = "/uploads/contracts/" + fileName;
                }

                await _contractService.CreateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Employees = await _employeeService.GetAllAsync();
            return View(model);
        }

        // Edit, Delete, Details...
    }
}
