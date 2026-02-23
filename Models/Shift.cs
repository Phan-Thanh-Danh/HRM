using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public class Shift
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty; // Ca Sáng, Ca Chiều

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public bool IsOvernight { get; set; } = false; // Ca qua đêm

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
