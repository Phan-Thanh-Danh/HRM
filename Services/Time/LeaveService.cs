using AutoMapper;
using HRM.Data;
using HRM.Models;
using HRM.ViewModels.Time;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Time
{
    public interface ILeaveService
    {
        Task<List<LeaveRequestVM>> GetMyRequestsAsync(int employeeId);
        Task<List<LeaveRequestVM>> GetPendingRequestsAsync(int managerId); // Requests for manager to approve
        Task CreateRequestAsync(LeaveRequestVM requestVM);
        Task ApproveRequestAsync(int id, int approverId);
        Task RejectRequestAsync(int id, int approverId, string reason);
    }

    public class LeaveService : ILeaveService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public LeaveService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task ApproveRequestAsync(int id, int approverId)
        {
            var request = await _context.LeaveRequests.FindAsync(id);
            if (request != null && request.Status == LeaveStatus.Pending)
            {
                request.Status = LeaveStatus.Approved;
                request.ApproverId = approverId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateRequestAsync(LeaveRequestVM requestVM)
        {
            var request = _mapper.Map<LeaveRequest>(requestVM);
            request.TotalDays = (request.ToDate - request.FromDate).TotalDays + 1; // Inclusive logic
            _context.LeaveRequests.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LeaveRequestVM>> GetMyRequestsAsync(int employeeId)
        {
             var requests = await _context.LeaveRequests
                .Include(r => r.Approver)
                .Where(r => r.EmployeeId == employeeId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return _mapper.Map<List<LeaveRequestVM>>(requests);
        }

        public async Task<List<LeaveRequestVM>> GetPendingRequestsAsync(int managerId)
        {
            // Logic: Find employees managed by this manager, then find their pending requests
            // For simple Department Manager role:
            // var subordinateIds = _context.Employees.Where(e => e.ManagerId == managerId).Select(e => e.Id);
            // This query can be complex. 

            var requests = await _context.LeaveRequests
                .Include(r => r.Employee)
                .Where(r => r.Status == LeaveStatus.Pending && r.Employee != null && r.Employee.ManagerId == managerId)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
            return _mapper.Map<List<LeaveRequestVM>>(requests);
        }

        public async Task RejectRequestAsync(int id, int approverId, string reason)
        {
            var request = await _context.LeaveRequests.FindAsync(id);
            if (request != null && request.Status == LeaveStatus.Pending)
            {
                request.Status = LeaveStatus.Rejected;
                request.ApproverId = approverId;
                request.RejectionReason = reason;
                await _context.SaveChangesAsync();
            }
        }
    }
}
