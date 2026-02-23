using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public enum AssetStatus
    {
        Available,
        InUse,
        Broken,
        Lost,
        Maintenance
    }

    public class Asset
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty; // e.g., LP-001

        public string? Description { get; set; }

        public DateTime PurchaseDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal PurchasePrice { get; set; }

        public AssetStatus Status { get; set; } = AssetStatus.Available;

        public int? CurrentHolderId { get; set; } // Employee currently holding this
    }
}
