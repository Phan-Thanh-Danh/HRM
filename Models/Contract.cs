using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public enum ContractType
    {
        Probation,      // Thử việc
        DefiniteTerm,   // Có thời hạn (12, 24 tháng)
        IndefiniteTerm, // Không xác định thời hạn
        PartTime,
        Internship
    }

    public enum ContractStatus
    {
        Active,
        Expired,
        Terminated,
        Draft
    }

    public class Contract
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        [Required]
        [MaxLength(50)]
        public string ContractNumber { get; set; } = string.Empty;

        public ContractType ContractType { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseSalary { get; set; }

        public string? FilePath { get; set; } // Link to scanned PDF

        public ContractStatus Status { get; set; } = ContractStatus.Active;

        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
