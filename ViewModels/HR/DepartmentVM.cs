using System.ComponentModel.DataAnnotations;

namespace HRM.ViewModels.HR
{
    public class DepartmentVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên phòng ban là bắt buộc")]
        [Display(Name = "Tên phòng ban")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Phòng ban cha")]
        public int? ParentId { get; set; }
        public string? ParentName { get; set; }

        [Display(Name = "Trưởng phòng")]
        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; }

        public int EmployeeCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
