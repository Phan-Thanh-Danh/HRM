using System.ComponentModel.DataAnnotations;

namespace HRM.ViewModels.HR
{
    public class OnboardingTaskVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        [Display(Name = "Nhiệm vụ")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Bắt buộc")]
        public bool IsRequired { get; set; }
        
        [Display(Name = "Thứ tự")]
        public int Order { get; set; }

        public bool IsActive { get; set; }
    }
}
