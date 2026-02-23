using System.ComponentModel.DataAnnotations;

namespace HRM.ViewModels.Learning
{
    public class LessonVM
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        
        [Required]
        [Display(Name = "Tiêu đề bài học")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Nội dung")]
        public string? Content { get; set; }
        
        [Display(Name = "Video URL")]
        public string? VideoUrl { get; set; }
        
        public int Order { get; set; }
        
        public int? QuizId { get; set; }
    }
}
