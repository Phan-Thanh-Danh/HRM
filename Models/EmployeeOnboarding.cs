using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public class EmployeeOnboarding
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        [Required]
        public int OnboardingTaskId { get; set; }
        [ForeignKey("OnboardingTaskId")]
        public virtual OnboardingTask? OnboardingTask { get; set; }

        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedDate { get; set; }

        public string? Note { get; set; }
    }
}
