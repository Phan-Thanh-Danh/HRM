using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public class JobTitle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal BaseSalaryMin { get; set; }
        public decimal BaseSalaryMax { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
