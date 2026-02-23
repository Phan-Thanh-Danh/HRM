using AutoMapper;
using HRM.Data;
using HRM.Models;
using HRM.ViewModels.HR;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.HR
{
    public interface IOnboardingService
    {
        Task<List<OnboardingTaskVM>> GetAllTasksAsync();
        Task CreateTaskAsync(OnboardingTaskVM taskVM);
        Task UpdateTaskAsync(OnboardingTaskVM taskVM);

        Task<EmployeeOnboardingVM> GetEmployeeOnboardingStatusAsync(int employeeId);
        Task AssignOnboardingTasksAsync(int employeeId);
        Task CompleteTaskAsync(int employeeId, int taskId);
    }

    public class OnboardingService : IOnboardingService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OnboardingService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AssignOnboardingTasksAsync(int employeeId)
        {
            var tasks = await _context.OnboardingTasks.Where(t => t.IsActive).ToListAsync();
            var existing = await _context
                .EmployeeOnboardings.Where(eo => eo.EmployeeId == employeeId)
                .Select(eo => eo.OnboardingTaskId)
                .ToListAsync();

            foreach (var task in tasks)
            {
                if (!existing.Contains(task.Id))
                {
                    _context.EmployeeOnboardings.Add(
                        new EmployeeOnboarding
                        {
                            EmployeeId = employeeId,
                            OnboardingTaskId = task.Id,
                            IsCompleted = false,
                        }
                    );
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task CompleteTaskAsync(int employeeId, int taskId)
        {
            var eo = await _context.EmployeeOnboardings.FirstOrDefaultAsync(x =>
                x.EmployeeId == employeeId && x.OnboardingTaskId == taskId
            );

            if (eo != null)
            {
                eo.IsCompleted = true;
                eo.CompletedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateTaskAsync(OnboardingTaskVM taskVM)
        {
            var task = _mapper.Map<OnboardingTask>(taskVM);
            _context.OnboardingTasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OnboardingTaskVM>> GetAllTasksAsync()
        {
            var tasks = await _context.OnboardingTasks.OrderBy(t => t.Order).ToListAsync();
            return _mapper.Map<List<OnboardingTaskVM>>(tasks);
        }

        public async Task<EmployeeOnboardingVM> GetEmployeeOnboardingStatusAsync(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                return new EmployeeOnboardingVM();

            // Ensure tasks are assigned
            await AssignOnboardingTasksAsync(employeeId);

            var onboardingEntries = await _context
                .EmployeeOnboardings.Include(eo => eo.OnboardingTask)
                .Where(eo => eo.EmployeeId == employeeId)
                .OrderBy(eo => eo.OnboardingTask != null ? eo.OnboardingTask.Order : 0)
                .ToListAsync();

            var vm = new EmployeeOnboardingVM
            {
                EmployeeId = employee.Id,
                EmployeeName = employee.FullName,
                Tasks = onboardingEntries
                    .Select(eo => new OnboardingTaskStatusVM
                    {
                        TaskId = eo.OnboardingTaskId,
                        Title = eo.OnboardingTask?.Title ?? "",
                        Description = eo.OnboardingTask?.Description,
                        IsRequired = eo.OnboardingTask?.IsRequired ?? false,
                        IsCompleted = eo.IsCompleted,
                        CompletedDate = eo.CompletedDate,
                        Note = eo.Note,
                    })
                    .ToList(),
            };

            return vm;
        }

        public async Task UpdateTaskAsync(OnboardingTaskVM taskVM)
        {
            var task = await _context.OnboardingTasks.FindAsync(taskVM.Id);
            if (task != null)
            {
                _mapper.Map(taskVM, task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
