using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace OneDesk.API.Models
{
    public class SaleRecord
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("InventoryItem")]
        public int InventoryItemId { get; set; }

        public int QuantitySold { get; set; }

        public decimal SellingPriceAtTime { get; set; }

        public DateTime SoldAt { get; set; } = DateTime.UtcNow;

        // Navigation Property (optional but useful)
        [JsonIgnore]
        [ValidateNever]
        public InventoryItem InventoryItem { get; set; }
    }
}
