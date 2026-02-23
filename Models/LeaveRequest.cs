using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public enum LeaveType
    {
        AnnualLeave, // Phép năm
        SickLeave,   // Nghỉ ốm
        UnpaidLeave, // Nghỉ không lương
        MaternityLeave, // Thai sản
        Wedding,     // Cưới hỏi
        Funeral      // Tang chế
    }

    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected,
        Cancelled
    }

    public class LeaveRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        [Required]
        public LeaveType Type { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public double TotalDays { get; set; }

        [Required]
        public string Reason { get; set; } = string.Empty;

        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

        public int? ApproverId { get; set; }
        [ForeignKey("ApproverId")]
        public virtual Employee? Approver { get; set; }

        public string? RejectionReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
