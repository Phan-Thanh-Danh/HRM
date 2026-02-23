using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public class Interview
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public virtual Application? Application { get; set; }

        [Required]
        public DateTime ScheduledTime { get; set; }

        public string? MeetingLink { get; set; } // Online meeting link
        public string? Location { get; set; }   // Physical location

        public int? InterviewerId { get; set; }
        [ForeignKey("InterviewerId")]
        public virtual Employee? Interviewer { get; set; }

        public string? Feedback { get; set; }
        public int? Score { get; set; } // 1-10

        public bool IsCompleted { get; set; } = false;
    }
}
