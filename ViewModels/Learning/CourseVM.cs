using HRM.Models;
using System.ComponentModel.DataAnnotations;

namespace HRM.ViewModels.Learning
{
    public class CourseVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên khóa học")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Thumbnail URL")]
        public string? ThumbnailUrl { get; set; }

        public bool IsPublished { get; set; }
        
        public int LessonsCount { get; set; }
        
        // For Employee View
        public CourseStatus EnrollmentStatus { get; set; } = CourseStatus.NotStarted;
        public double Progress { get; set; }
    }
}
