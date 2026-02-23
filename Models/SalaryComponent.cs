using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public enum SalaryComponentType
    {
        Earning,    // Thu nhập (Lương cơ bản, Phụ cấp...)
        Deduction   // Khấu trừ (Bảo hiểm, Thuế, Phạt...)
    }

    public class SalaryComponent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public SalaryComponentType Type { get; set; }

        public bool IsFixed { get; set; } = true; // True: Số tiền cố định, False: Tính theo công thức/biến đổi

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
