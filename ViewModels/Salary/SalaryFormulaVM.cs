using HRM.Models;
using System.ComponentModel.DataAnnotations;

namespace HRM.ViewModels.Salary
{
    public class SalaryFormulaVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Thành phần lương")]
        public int SalaryComponentId { get; set; }
        
        [Display(Name = "Tên thành phần lương")]
        public string? SalaryComponentName { get; set; }
        
        public bool IsFixed { get; set; }

        [Required]
        [Display(Name = "Công thức")]
        public string FormulaExpression { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }
    }
}
