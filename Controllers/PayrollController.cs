using HRM.Services.Salary;
using HRM.ViewModels.Salary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers
{
    [Authorize(Roles = "SuperAdmin,HRManager,CBSpecialist")]
    public class PayrollController : Controller
    {
        private readonly IPayrollService _payrollService;

        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        public async Task<IActionResult> Index()
        {
            var periods = await _payrollService.GetPeriodsAsync();
            return View(periods);
        }

        public IActionResult CreatePeriod()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePeriod(PayrollPeriodVM model)
        {
            if (ModelState.IsValid)
            {
                await _payrollService.CreatePeriodAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Run(int id)
        {
            await _payrollService.CalculatePayrollAsync(id);
            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Details(int id)
        {
            var records = await _payrollService.GetPayrollRecordsAsync(id);
            ViewBag.PeriodId = id;
            return View(records);
        }

        [HttpPost]
        public async Task<IActionResult> Lock(int id)
        {
            await _payrollService.ClosePeriodAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Simple formula management
        public async Task<IActionResult> Formulas()
        {
            var formulas = await _payrollService.GetFormulasAsync();
            return View(formulas);
        }

        public async Task<IActionResult> CreateFormula()
        {
            ViewBag.Components = await _payrollService.GetComponentsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFormula(SalaryFormulaVM model)
        {
            if (ModelState.IsValid)
            {
                await _payrollService.CreateFormulaAsync(model);
                return RedirectToAction(nameof(Formulas));
            }
            ViewBag.Components = await _payrollService.GetComponentsAsync();
            return View(model);
        }
    }
}
