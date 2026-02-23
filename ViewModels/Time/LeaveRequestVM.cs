using HRM.Models;
using System.ComponentModel.DataAnnotations;

namespace HRM.ViewModels.Time
{
    public class LeaveRequestVM
    {
        public int Id { get; set; }
        
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }

        [Required(ErrorMessage = "Loại nghỉ phép là bắt buộc")]
        [Display(Name = "Loại nghỉ")]
        public LeaveType Type { get; set; }

        [Required(ErrorMessage = "Từ ngày là bắt buộc")]
        [DataType(DataType.Date)]
        [Display(Name = "Từ ngày")]
        public DateTime FromDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Đến ngày là bắt buộc")]
        [DataType(DataType.Date)]
        [Display(Name = "Đến ngày")]
        public DateTime ToDate { get; set; } = DateTime.Today;

        [Display(Name = "Tổng số ngày")]
        public double TotalDays { get; set; }

        [Required(ErrorMessage = "Lý do là bắt buộc")]
        [Display(Name = "Lý do")]
        public string Reason { get; set; } = string.Empty;

        [Display(Name = "Trạng thái")]
        public LeaveStatus Status { get; set; }

        [Display(Name = "Người duyệt")]
        public string? ApproverName { get; set; }
        
        [Display(Name = "Lý do từ chối")]
        public string? RejectionReason { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}
