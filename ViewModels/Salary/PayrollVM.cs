using HRM.Models;
using System.ComponentModel.DataAnnotations;

namespace HRM.ViewModels.Salary
{
    public class PayrollPeriodVM
    {
        public int Id { get; set; }
        
        [Display(Name = "Tên kỳ lương")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Tháng")]
        public int Month { get; set; }
        
        [Display(Name = "Năm")]
        public int Year { get; set; }

        [Display(Name = "Từ ngày")]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }

        [Display(Name = "Đến ngày")]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

        [Display(Name = "Trạng thái")]
        public PayrollStatus Status { get; set; }
    }

    public class PayrollRecordVM
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [Display(Name = "Nhân viên")]
        public string EmployeeName { get; set; } = string.Empty;
        
        [Display(Name = "Lương cơ bản")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal BasicSalary { get; set; }

        [Display(Name = "Công thực tế")]
        public double RealWorkDays { get; set; }

        [Display(Name = "Tổng thu nhập")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal TotalEarnings { get; set; }

        [Display(Name = "Tổng khấu trừ")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal TotalDeductions { get; set; }
        
        [Display(Name = "Thuế TNCN")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal TaxAmount { get; set; }

        [Display(Name = "Bảo hiểm")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal InsuranceAmount { get; set; }

        [Display(Name = "Thực lĩnh")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal NetSalary { get; set; }
    }
}
