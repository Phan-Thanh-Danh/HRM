using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public enum PayrollStatus
    {
        Open,       // Đang tính toán
        Locked,     // Đã chốt, không được sửa
        Paid        // Đã thanh toán
    }

    public class PayrollPeriod
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // Eg: "Payroll Tháng 10/2026"

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public PayrollStatus Status { get; set; } = PayrollStatus.Open;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
