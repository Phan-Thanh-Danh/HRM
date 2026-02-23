using HRM.Services.Time;
using HRM.ViewModels.Time;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers
{
    [Authorize(Roles = "SuperAdmin,HRManager")]
    public class ShiftController : Controller
    {
        private readonly IShiftService _shiftService;

        public ShiftController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        public async Task<IActionResult> Index()
        {
            var shifts = await _shiftService.GetAllAsync();
            return View(shifts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ShiftVM model)
        {
            if (ModelState.IsValid)
            {
                await _shiftService.CreateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var shift = await _shiftService.GetByIdAsync(id);
            if (shift == null) return NotFound();
            return View(shift);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ShiftVM model)
        {
            if (ModelState.IsValid)
            {
                await _shiftService.UpdateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _shiftService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
