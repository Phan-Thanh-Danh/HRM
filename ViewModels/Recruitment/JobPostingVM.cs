using HRM.Models;
using System.ComponentModel.DataAnnotations;

namespace HRM.ViewModels.Recruitment
{
    public class JobPostingVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        [Display(Name = "Chức danh tuyển dụng")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Mô tả công việc")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Phòng ban")]
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }

        [Required]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Hạn nộp hồ sơ")]
        public DateTime Deadline { get; set; } = DateTime.Today.AddDays(30);

        [Display(Name = "Lương tối thiểu")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal MinSalary { get; set; }

        [Display(Name = "Lương tối đa")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal MaxSalary { get; set; }

        [Display(Name = "Địa điểm")]
        public string? Location { get; set; }

        public JobStatus Status { get; set; } = JobStatus.Draft;
        
        public int ApplicationCount { get; set; }
    }
}
