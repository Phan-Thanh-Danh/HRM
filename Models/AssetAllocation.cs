using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public class AssetAllocation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AssetId { get; set; }
        [ForeignKey("AssetId")]
        public virtual Asset? Asset { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ReturnedDate { get; set; }

        public string? Notes { get; set; }
    }
}
