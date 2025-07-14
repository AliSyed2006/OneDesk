using System.ComponentModel.DataAnnotations;

namespace OneDesk.API.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string SKU { get; set; } = string.Empty;

        [StringLength(250)]
        public string Description { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [StringLength(100)]
        public string Supplier { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
        public int QuantityInStock { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Cost price must be ≥ 0")]
        public decimal CostPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Selling price must be ≥ 0")]
        public decimal SellingPrice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
