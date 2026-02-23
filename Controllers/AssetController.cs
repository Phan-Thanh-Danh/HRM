using HRM.Services.Admin;
using HRM.Services.HR;
using HRM.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers
{
    [Authorize(Roles = "SuperAdmin,HRManager")]
    public class AssetController : Controller
    {
        private readonly IAssetService _assetService;
        private readonly IEmployeeService _employeeService;

        public AssetController(IAssetService assetService, IEmployeeService employeeService)
        {
            _assetService = assetService;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var assets = await _assetService.GetAllAssetsAsync();
            return View(assets);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AssetVM model)
        {
            if (ModelState.IsValid)
            {
                await _assetService.CreateAssetAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Assign(int id)
        {
            ViewBag.AssetId = id;
            var employees = await _employeeService.GetAllAsync();
            ViewBag.Employees = employees;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Assign(int assetId, int employeeId, string? notes)
        {
            await _assetService.AssignAssetAsync(assetId, employeeId, notes);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Return(int id)
        {
            await _assetService.ReturnAssetAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
