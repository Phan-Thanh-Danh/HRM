using HRM.Models;
using System.ComponentModel.DataAnnotations;

namespace HRM.ViewModels.HR
{
    public class ContractVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nhân viên là bắt buộc")]
        [Display(Name = "Nhân viên")]
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }

        [Required(ErrorMessage = "Số hợp đồng là bắt buộc")]
        [Display(Name = "Số hợp đồng")]
        public string ContractNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Loại hợp đồng là bắt buộc")]
        [Display(Name = "Loại hợp đồng")]
        public ContractType Type { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        [Display(Name = "Ngày hiệu lực")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Ngày hết hạn")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Lương cơ bản là bắt buộc")]
        [Display(Name = "Lương cơ bản")]
        [DataType(DataType.Currency)]
        public decimal BaseSalary { get; set; }

        [Display(Name = "File đính kèm")]
        public IFormFile? ContractFile { get; set; }
        public string? FilePath { get; set; }

        [Display(Name = "Trạng thái")]
        public ContractStatus Status { get; set; }

        public string? Note { get; set; }
    }
}
