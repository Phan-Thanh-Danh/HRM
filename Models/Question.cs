using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuizId { get; set; }
        [ForeignKey("QuizId")]
        public virtual Quiz? Quiz { get; set; }

        [Required]
        public string QuestionText { get; set; } = string.Empty;

        // Simple options stored as JSON or delimited string for now
        // Format: "Option A|Option B|Option C|Option D"
        [Required]
        public string Options { get; set; } = string.Empty; 

        [Required]
        public int CorrectOptionIndex { get; set; } // 0, 1, 2, 3
    }
}
