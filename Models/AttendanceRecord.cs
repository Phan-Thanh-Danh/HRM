using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public enum AttendanceStatus
    {
        Present,
        Late,
        EarlyLeave,
        Absent
    }

    public class AttendanceRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        [Required]
        public DateTime Date { get; set; } // Ngày chấm công

        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        public string? CheckInLocation { get; set; } // GPS Coordinates
        public string? CheckOutLocation { get; set; }

        public string? CheckInImage { get; set; } // Path to FaceID image
        public string? CheckOutImage { get; set; }

        public AttendanceStatus Status { get; set; } = AttendanceStatus.Absent;
        
        public string? Note { get; set; } // Lý do đi muộn/về sớm

        public double WorkingHours { get; set; } // Số giờ làm việc thực tế
    }
}
