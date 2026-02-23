using AutoMapper;
using HRM.Data;
using HRM.Models;
using HRM.ViewModels.Salary;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Salary
{
    public interface IPayrollService
    {
        // Components & Formulas
        Task<List<SalaryComponent>> GetComponentsAsync();
        Task<List<SalaryFormulaVM>> GetFormulasAsync();
        Task CreateFormulaAsync(SalaryFormulaVM formulaVM);

        // Payroll Period
        Task<List<PayrollPeriodVM>> GetPeriodsAsync();
        Task CreatePeriodAsync(PayrollPeriodVM periodVM);
        Task ClosePeriodAsync(int id);

        // Calculation
        Task CalculatePayrollAsync(int periodId);
        Task<List<PayrollRecordVM>> GetPayrollRecordsAsync(int periodId);
        Task<PayrollRecordVM?> GetEmployeePayrollAsync(int periodId, int employeeId);
    }

    public class PayrollService : IPayrollService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PayrollService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CalculatePayrollAsync(int periodId)
        {
            var period = await _context.PayrollPeriods.FindAsync(periodId);
            if (period == null || period.Status == PayrollStatus.Locked)
                return; // Don't recalc if locked

            // 1. Get all eligible employees
            var employees = await _context
                .Employees.Include(e => e.Contracts)
                .Include(e => e.JobTitle)
                .Where(e =>
                    e.Status == EmployeeStatus.Active || e.Status == EmployeeStatus.Probation
                )
                .ToListAsync();

            // 2. Clear existing records for this period (to allow re-run)
            var existingRecords = _context.PayrollRecords.Where(pr =>
                pr.PayrollPeriodId == periodId
            );
            _context.PayrollRecords.RemoveRange(existingRecords);
            await _context.SaveChangesAsync();

            // 3. Calculate for each employee
            var records = new List<PayrollRecord>();

            foreach (var emp in employees)
            {
                // Basic data
                decimal baseSalary = 0;
                var activeContract = emp.Contracts.FirstOrDefault(c =>
                    c.Status == ContractStatus.Active
                );
                if (activeContract != null)
                {
                    baseSalary = activeContract.BaseSalary;
                }
                else if (emp.JobTitle != null)
                {
                    baseSalary = emp.JobTitle.BaseSalaryMin; // Fallback
                }

                // Work days from Attendance (Mock logic for now, or call AttendanceService)
                // In real app, inject IAttendanceService and get real days
                double standardWorkDays = 26.0;
                double realWorkDays = 26.0; // Assume full attendance for now or random

                // Simple Formula: Salary = Base / 26 * Real
                decimal grossIncome =
                    (baseSalary / (decimal)standardWorkDays) * (decimal)realWorkDays;

                // Allowances (Hardcoded for now, should come from Formulas)
                decimal allowances = 0;

                decimal totalEarnings = grossIncome + allowances;

                // Deductions (Tax & Insurance - Mock 10.5% insurance, Tax progressive)
                decimal insurance = totalEarnings * 0.105m;
                decimal taxableIncome = totalEarnings - insurance - 11000000; // 11tr deduction
                decimal tax = 0;
                if (taxableIncome > 0)
                {
                    tax = taxableIncome * 0.05m; // Simple 5% for demo
                }

                decimal totalDeductions = insurance + tax;
                decimal netSalary = totalEarnings - totalDeductions;

                records.Add(
                    new PayrollRecord
                    {
                        PayrollPeriodId = periodId,
                        EmployeeId = emp.Id,
                        BasicSalary = baseSalary,
                        StandardWorkDays = standardWorkDays,
                        RealWorkDays = realWorkDays,
                        TotalEarnings = totalEarnings,
                        TotalDeductions = totalDeductions,
                        TaxAmount = tax,
                        InsuranceAmount = insurance,
                        NetSalary = netSalary,
                        Note = "Auto-calculated",
                    }
                );
            }

            _context.PayrollRecords.AddRange(records);
            await _context.SaveChangesAsync();
        }

        public async Task ClosePeriodAsync(int id)
        {
            var period = await _context.PayrollPeriods.FindAsync(id);
            if (period != null)
            {
                period.Status = PayrollStatus.Locked;
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateFormulaAsync(SalaryFormulaVM formulaVM)
        {
            var formula = _mapper.Map<SalaryFormula>(formulaVM);
            _context.SalaryFormulas.Add(formula);
            await _context.SaveChangesAsync();
        }

        public async Task CreatePeriodAsync(PayrollPeriodVM periodVM)
        {
            var period = _mapper.Map<PayrollPeriod>(periodVM);
            _context.PayrollPeriods.Add(period);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SalaryComponent>> GetComponentsAsync()
        {
            return await _context.SalaryComponents.ToListAsync();
        }

        public async Task<PayrollRecordVM?> GetEmployeePayrollAsync(int periodId, int employeeId)
        {
            var record = await _context
                .PayrollRecords.Include(pr => pr.Employee)
                .FirstOrDefaultAsync(pr =>
                    pr.PayrollPeriodId == periodId && pr.EmployeeId == employeeId
                );
            return _mapper.Map<PayrollRecordVM>(record);
        }

        public async Task<List<SalaryFormulaVM>> GetFormulasAsync()
        {
            var formulas = await _context
                .SalaryFormulas.Include(f => f.SalaryComponent)
                .ToListAsync();
            return _mapper.Map<List<SalaryFormulaVM>>(formulas);
        }

        public async Task<List<PayrollRecordVM>> GetPayrollRecordsAsync(int periodId)
        {
            var records = await _context
                .PayrollRecords.Include(pr => pr.Employee)
                .Where(pr => pr.PayrollPeriodId == periodId)
                .ToListAsync();
            return _mapper.Map<List<PayrollRecordVM>>(records);
        }

        public async Task<List<PayrollPeriodVM>> GetPeriodsAsync()
        {
            var periods = await _context
                .PayrollPeriods.OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .ToListAsync();
            return _mapper.Map<List<PayrollPeriodVM>>(periods);
        }
    }
}
