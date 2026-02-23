using HRM.Models;
using System.ComponentModel.DataAnnotations;

namespace HRM.ViewModels.HR
{
    public class EmployeeVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Giới tính là bắt buộc")]
        [Display(Name = "Giới tính")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "CCCD là bắt buộc")]
        [Display(Name = "Số CCCD")]
        public string CitizenId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; // Used for Account creation too

        [Phone]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public IFormFile? AvatarFile { get; set; }
        public string? AvatarUrl { get; set; }

        [Display(Name = "Phòng ban")]
        public int? DepartmentId { get; set; }

        [Display(Name = "Chức danh")]
        public int? JobTitleId { get; set; }

        [Display(Name = "Quản lý trực tiếp")]
        public int? ManagerId { get; set; }

        [Display(Name = "Ngày gia nhập")]
        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; } = DateTime.Now;

        [Display(Name = "Trạng thái")]
        public EmployeeStatus Status { get; set; }
    }
}
