using HRM.Models;
using System.ComponentModel.DataAnnotations;

namespace HRM.ViewModels.Admin
{
    public class AssetVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên tài sản")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Mã tài sản")]
        public string Code { get; set; } = string.Empty;

        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày mua")]
        public DateTime PurchaseDate { get; set; }

        [Display(Name = "Giá mua")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal PurchasePrice { get; set; }

        public AssetStatus Status { get; set; }
        
        public string? CurrentHolderName { get; set; }
        public int? CurrentHolderId { get; set; }
    }
}
