using AutoMapper;
using HRM.Data;
using HRM.Models;
using HRM.ViewModels.Time;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Time
{
    public interface IShiftService
    {
        Task<List<ShiftVM>> GetAllAsync();
        Task<ShiftVM?> GetByIdAsync(int id);
        Task CreateAsync(ShiftVM shiftVM);
        Task UpdateAsync(ShiftVM shiftVM);
        Task DeleteAsync(int id);
    }

    public class ShiftService : IShiftService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ShiftService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(ShiftVM shiftVM)
        {
            var shift = _mapper.Map<Shift>(shiftVM);
            _context.Shifts.Add(shift);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var shift = await _context.Shifts.FindAsync(id);
            if (shift != null)
            {
                shift.IsActive = false; // Soft delete
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ShiftVM>> GetAllAsync()
        {
            var shifts = await _context.Shifts.Where(s => s.IsActive).ToListAsync();
            return _mapper.Map<List<ShiftVM>>(shifts);
        }

        public async Task<ShiftVM?> GetByIdAsync(int id)
        {
            var shift = await _context.Shifts.FindAsync(id);
            return _mapper.Map<ShiftVM>(shift);
        }

        public async Task UpdateAsync(ShiftVM shiftVM)
        {
            var shift = await _context.Shifts.FindAsync(shiftVM.Id);
            if (shift != null)
            {
                _mapper.Map(shiftVM, shift);
                await _context.SaveChangesAsync();
            }
        }
    }
}
