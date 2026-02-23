using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Foreign keys to link with Employee profile if needed
        public int? EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
