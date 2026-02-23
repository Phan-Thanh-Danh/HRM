using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public class PayrollRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PayrollPeriodId { get; set; }
        [ForeignKey("PayrollPeriodId")]
        public virtual PayrollPeriod? PayrollPeriod { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        // Snapshot data at time of calculation
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasicSalary { get; set; }

        public double StandardWorkDays { get; set; } // Ngày công chuẩn (vd: 26)
        public double RealWorkDays { get; set; }     // Thực tế đi làm

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalEarnings { get; set; }   // Tổng thu nhập

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDeductions { get; set; } // Tổng khấu trừ

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }       // Thuế TNCN

        [Column(TypeName = "decimal(18,2)")]
        public decimal InsuranceAmount { get; set; } // Bảo hiểm

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetSalary { get; set; }       // Thực lĩnh

        public string? Note { get; set; }
        
        public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
    }
}
