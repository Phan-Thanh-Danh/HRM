using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public class OnboardingTask
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsRequired { get; set; } = true;
        
        public int Order { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
