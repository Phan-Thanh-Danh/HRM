using HRM.ViewModels.HR;

namespace HRM.Services.HR
{
    public interface IDepartmentService
    {
        Task<List<DepartmentVM>> GetAllAsync();
        Task<DepartmentVM?> GetByIdAsync(int id);
        Task CreateAsync(DepartmentVM departmentVM);
        Task UpdateAsync(DepartmentVM departmentVM);
        Task DeleteAsync(int id);
        Task<List<DepartmentVM>> GetOrgChartAsync(); // For tree view
    }
}
