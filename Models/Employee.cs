using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public enum EmployeeStatus
    {
        Active,
        Probation,
        MaternityLeave,
        Terminated,
        Resigned,
    }

    public enum Gender
    {
        Male,
        Female,
        Other,
    }

    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        [Required]
        [MaxLength(20)]
        public string CitizenId { get; set; } = string.Empty; // CCCD

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? AvatarUrl { get; set; }

        // Job Details
        public int? DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }

        public int? JobTitleId { get; set; }

        [ForeignKey("JobTitleId")]
        public virtual JobTitle? JobTitle { get; set; }

        public int? ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        public virtual Employee? Manager { get; set; }

        public DateTime JoinDate { get; set; }
        public EmployeeStatus Status { get; set; } = EmployeeStatus.Probation;

        // Identity Link
        public string? ApplicationUserId { get; set; }

        // Navigation to ApplicationUser - Note: ApplicationUser already has EmployeeId

        // Relationships
        public virtual ICollection<Employee> Subordinates { get; set; } = new List<Employee>();
        public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public string? FaceDescriptor { get; set; } // JSON serialized face template
    }
}
