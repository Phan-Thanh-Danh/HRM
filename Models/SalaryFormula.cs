using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Models
{
    public class SalaryFormula
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SalaryComponentId { get; set; }
        [ForeignKey("SalaryComponentId")]
        public virtual SalaryComponent? SalaryComponent { get; set; }

        [Required]
        public string FormulaExpression { get; set; } = string.Empty; 
        // Example: "[BasicSalary] / 26 * [RealWorkDays]" or "500000"

        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
}
