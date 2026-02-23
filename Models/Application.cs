using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public enum ApplicationStatus
    {
        New,
        Screening,
        Interview,
        Offer,
        Hired,
        Rejected
    }

    public class Application
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int JobPostingId { get; set; }
        [ForeignKey("JobPostingId")]
        public virtual JobPosting? JobPosting { get; set; }

        [Required]
        public int CandidateId { get; set; }
        [ForeignKey("CandidateId")]
        public virtual Candidate? Candidate { get; set; }

        public ApplicationStatus Status { get; set; } = ApplicationStatus.New;

        public DateTime AppliedDate { get; set; } = DateTime.UtcNow;
        
        public virtual ICollection<Interview> Interviews { get; set; } = new List<Interview>();
    }
}
