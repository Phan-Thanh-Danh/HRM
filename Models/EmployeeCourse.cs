using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public enum CourseStatus
    {
        NotStarted,
        InProgress,
        Completed
    }

    public class EmployeeCourse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        [Required]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; }

        public CourseStatus Status { get; set; } = CourseStatus.NotStarted;

        public double Progress { get; set; } // 0 - 100%

        public int? QuizScore { get; set; } // Final score if applicable

        public DateTime EnrolledDate { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedDate { get; set; }
    }
}
