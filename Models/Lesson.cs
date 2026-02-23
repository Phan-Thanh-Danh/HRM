using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Content { get; set; } // HTML/Text content
        
        public string? VideoUrl { get; set; } // Youtube/Video link

        public int Order { get; set; } // Sort order

        // Optional: Quiz for this lesson
        public int? QuizId { get; set; }
        [ForeignKey("QuizId")]
        public virtual Quiz? Quiz { get; set; }
    }
}
