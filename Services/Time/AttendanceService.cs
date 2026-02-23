using AutoMapper;
using HRM.Data;
using HRM.Models;
using HRM.ViewModels.Time;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Time
{
    public interface IAttendanceService
    {
        Task<List<AttendanceLogVM>> GetHistoryAsync(int employeeId, int month, int year);
        Task<bool> CheckInAsync(int employeeId, AttendanceCheckInVM checkInVM);
        Task<bool> CheckOutAsync(int employeeId, AttendanceCheckInVM checkOutVM);
        Task<bool> HasCheckedInTodayAsync(int employeeId);
        Task<bool> RegisterFaceAsync(int employeeId, string descriptor);
        Task<string?> GetFaceDescriptorAsync(int employeeId);
    }

    public class AttendanceService : IAttendanceService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AttendanceService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckInAsync(int employeeId, AttendanceCheckInVM checkInVM)
        {
            var today = DateTime.Today;
            var exists = await _context.AttendanceRecords
                .AnyAsync(a => a.EmployeeId == employeeId && a.Date == today);
            
            if (exists) return false;

            var record = new AttendanceRecord
            {
                EmployeeId = employeeId,
                Date = today,
                CheckInTime = DateTime.Now,
                CheckInLocation = $"{checkInVM.Latitude}, {checkInVM.Longitude}",
                CheckInImage = checkInVM.ImageData, // Would save image to file in Controller
                Status = AttendanceStatus.Present // Simplification, need logic to check Late
            };

            // Calculate Status based on Shift (logic needed: find assigned shift for today)
            // For now, default to Present

            _context.AttendanceRecords.Add(record);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckOutAsync(int employeeId, AttendanceCheckInVM checkOutVM)
        {
            var today = DateTime.Today;
            var record = await _context.AttendanceRecords
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date == today);
            
            if (record == null) return false;

            record.CheckOutTime = DateTime.Now;
            record.CheckOutLocation = $"{checkOutVM.Latitude}, {checkOutVM.Longitude}";
            record.CheckOutImage = checkOutVM.ImageData; // Would save image to file in Controller
            
            // Calculate working hours
            if (record.CheckInTime.HasValue)
            {
                record.WorkingHours = (record.CheckOutTime.Value - record.CheckInTime.Value).TotalHours;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<AttendanceLogVM>> GetHistoryAsync(int employeeId, int month, int year)
        {
            var records = await _context.AttendanceRecords
                .Where(a => a.EmployeeId == employeeId && a.Date.Month == month && a.Date.Year == year)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
            return _mapper.Map<List<AttendanceLogVM>>(records);
        }

        public async Task<bool> HasCheckedInTodayAsync(int employeeId)
        {
             return await _context.AttendanceRecords
                .AnyAsync(a => a.EmployeeId == employeeId && a.Date == DateTime.Today);
        }

        public async Task<bool> RegisterFaceAsync(int employeeId, string descriptor)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null) return false;

            employee.FaceDescriptor = descriptor;
            employee.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string?> GetFaceDescriptorAsync(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            return employee?.FaceDescriptor;
        }
    }
}
