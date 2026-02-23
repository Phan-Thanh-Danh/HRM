using HRM.Models;
using System.ComponentModel.DataAnnotations;

namespace HRM.ViewModels.Time
{
    public class AttendanceCheckInVM
    {
        [Display(Name = "Vị trí hiện tại")]
        public string? Latitude { get; set; }
        
        [Display(Name = "Vị trí hiện tại")]
        public string? Longitude { get; set; }
        
        // Base64 image or File upload
        public string? ImageData { get; set; }
        
        public string? Note { get; set; }
    }

    public class AttendanceLogVM
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        
        [Display(Name = "Ngày")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        [Display(Name = "Giờ vào")]
        [DataType(DataType.Time)]
        public DateTime? CheckInTime { get; set; }
        
        [Display(Name = "Giờ ra")]
        [DataType(DataType.Time)]
        public DateTime? CheckOutTime { get; set; }
        
        [Display(Name = "Trạng thái")]
        public AttendanceStatus Status { get; set; }
        
        [Display(Name = "Tổng giờ làm")]
        public double WorkingHours { get; set; }

        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }
    }
}
