using AutoMapper;
using HRM.Data;
using HRM.Models;
using HRM.ViewModels.HR;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.HR
{
    public interface IContractService
    {
        Task<List<ContractVM>> GetAllAsync();
        Task<List<ContractVM>> GetByEmployeeIdAsync(int employeeId);
        Task<ContractVM?> GetByIdAsync(int id);
        Task CreateAsync(ContractVM contractVM);
        Task UpdateAsync(ContractVM contractVM);
        Task DeleteAsync(int id);
        Task<List<ContractVM>> GetExpiringContractsAsync(int daysInAdvance);
    }

    public class ContractService : IContractService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ContractService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(ContractVM contractVM)
        {
            var contract = _mapper.Map<Contract>(contractVM);
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract != null)
            {
                contract.Status = ContractStatus.Terminated;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ContractVM>> GetAllAsync()
        {
            var contracts = await _context.Contracts.Include(c => c.Employee).ToListAsync();
            return _mapper.Map<List<ContractVM>>(contracts);
        }

        public async Task<List<ContractVM>> GetByEmployeeIdAsync(int employeeId)
        {
            var contracts = await _context.Contracts
                .Where(c => c.EmployeeId == employeeId)
                .Include(c => c.Employee)
                .ToListAsync();
            return _mapper.Map<List<ContractVM>>(contracts);
        }

        public async Task<ContractVM?> GetByIdAsync(int id)
        {
            var contract = await _context.Contracts.Include(c => c.Employee).FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<ContractVM>(contract);
        }

        public async Task<List<ContractVM>> GetExpiringContractsAsync(int daysInAdvance)
        {
            var targetDate = DateTime.Today.AddDays(daysInAdvance);
            var today = DateTime.Today;
            
            var contracts = await _context.Contracts
                .Include(c => c.Employee)
                .Where(c => c.EndDate.HasValue 
                            && c.EndDate.Value <= targetDate 
                            && c.EndDate.Value >= today
                            && c.Status == ContractStatus.Active)
                .ToListAsync();
            return _mapper.Map<List<ContractVM>>(contracts);
        }

        public async Task UpdateAsync(ContractVM contractVM)
        {
            var contract = await _context.Contracts.FindAsync(contractVM.Id);
            if (contract != null)
            {
                _mapper.Map(contractVM, contract);
                await _context.SaveChangesAsync();
            }
        }
    }
}
