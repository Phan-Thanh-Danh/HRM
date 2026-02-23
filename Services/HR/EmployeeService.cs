using AutoMapper;
using HRM.Data;
using HRM.Models;
using HRM.ViewModels.HR;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.HR
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EmployeeService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(EmployeeVM employeeVM)
        {
            var employee = _mapper.Map<Employee>(employeeVM);

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                employee.Status = EmployeeStatus.Terminated; // Logic soft delete or status change
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<EmployeeVM>> GetAllAsync()
        {
            var employees = await _context
                .Employees.Include(e => e.Department)
                .Include(e => e.JobTitle)
                .ToListAsync();
            return _mapper.Map<List<EmployeeVM>>(employees);
        }



        public async Task<EmployeeVM?> GetByIdAsync(int id)
        {
            var employee = await _context
                .Employees.Include(e => e.Department)
                .Include(e => e.JobTitle)
                .Include(e => e.Manager)
                .FirstOrDefaultAsync(e => e.Id == id);
            return _mapper.Map<EmployeeVM>(employee);
        }

        public async Task UpdateAsync(EmployeeVM employeeVM)
        {
            var employee = await _context.Employees.FindAsync(employeeVM.Id);
            if (employee != null)
            {
                _mapper.Map(employeeVM, employee);
                employee.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
