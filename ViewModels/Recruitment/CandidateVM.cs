using HRM.Models;
using System.ComponentModel.DataAnnotations;

namespace HRM.ViewModels.Recruitment
{
    public class CandidateVM
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        [Display(Name = "CV Upload")]
        public IFormFile? CVFile { get; set; }
        
        public string? CVFilePath { get; set; }

        public string? Source { get; set; }
        
        // Context for Applying
        public int? JobPostingId { get; set; }
    }
}
