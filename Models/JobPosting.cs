using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public enum JobStatus
    {
        Draft,
        Published,
        Closed
    }

    public class JobPosting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }

        [Required]
        public int Quantity { get; set; } // Số lượng cần tuyển

        public DateTime Deadline { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MinSalary { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MaxSalary { get; set; }
        
        public string? Location { get; set; }

        public JobStatus Status { get; set; } = JobStatus.Draft;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
