using System.ComponentModel.DataAnnotations;

namespace HRM.ViewModels.Time
{
    public class ShiftVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên ca làm việc là bắt buộc")]
        [Display(Name = "Tên ca")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giờ bắt đầu là bắt buộc")]
        [DataType(DataType.Time)]
        [Display(Name = "Giờ bắt đầu")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Giờ kết thúc là bắt buộc")]
        [DataType(DataType.Time)]
        [Display(Name = "Giờ kết thúc")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "Ca qua đêm")]
        public bool IsOvernight { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
