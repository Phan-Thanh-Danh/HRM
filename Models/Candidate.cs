using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public class Candidate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? CVFilePath { get; set; } // Link to uploaded CV

        public string? Source { get; set; } // LinkedIn, Facebook, Referral...

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
