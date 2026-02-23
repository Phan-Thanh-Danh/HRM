using HRM.ViewModels.HR;

namespace HRM.Services.HR
{
    public interface IEmployeeService
    {
        Task<List<EmployeeVM>> GetAllAsync();
        Task<EmployeeVM?> GetByIdAsync(int id);
        Task CreateAsync(EmployeeVM employeeVM);
        Task UpdateAsync(EmployeeVM employeeVM);
        Task DeleteAsync(int id);

      }
}
