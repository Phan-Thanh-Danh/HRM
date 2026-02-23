using AutoMapper;
using HRM.Data;
using HRM.Models;
using HRM.ViewModels.HR;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.HR
{
    public class DepartmentService : IDepartmentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DepartmentService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(DepartmentVM departmentVM)
        {
            var department = _mapper.Map<Department>(departmentVM);
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                department.IsDeleted = true; // Soft delete
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<DepartmentVM>> GetAllAsync()
        {
            var departments = await _context.Departments
                .Include(d => d.ParentDepartment)
                .Include(d => d.Manager)
                .Where(d => !d.IsDeleted) // Filter soft deleted
                .ToListAsync();
            return _mapper.Map<List<DepartmentVM>>(departments);
        }

        public async Task<DepartmentVM?> GetByIdAsync(int id)
        {
            var department = await _context.Departments
                .Include(d => d.ParentDepartment)
                .Include(d => d.Manager)
                .FirstOrDefaultAsync(d => d.Id == id);
            return _mapper.Map<DepartmentVM>(department);
        }

        public async Task<List<DepartmentVM>> GetOrgChartAsync()
        {
            // For Org Chart, we might need a specific structure, but List<DepartmentVM> with ParentId is enough to build a tree on client side
            return await GetAllAsync();
        }

        public async Task UpdateAsync(DepartmentVM departmentVM)
        {
            var department = await _context.Departments.FindAsync(departmentVM.Id);
            if (department != null)
            {
                _mapper.Map(departmentVM, department);
                await _context.SaveChangesAsync();
            }
        }
    }
}
