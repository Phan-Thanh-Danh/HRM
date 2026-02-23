using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Department? ParentDepartment { get; set; }

        public int? ManagerId { get; set; }
        // Navigation property defined after Employee class is created to avoid circular dependency issues in code definition order, 
        // but EF Core handles it fine.
        [ForeignKey("ManagerId")]
        public virtual Employee? Manager { get; set; }

        public virtual ICollection<Department> ChildDepartments { get; set; } = new List<Department>();
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
