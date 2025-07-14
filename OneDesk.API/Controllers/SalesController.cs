using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneDesk.API.Data;
using OneDesk.API.Models;

namespace OneDesk.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SalesController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/sales
        [HttpPost]
        public async Task<IActionResult> RecordSale([FromBody] SaleRecord sale)
        {
            var item = await _context.InventoryItems.FindAsync(sale.InventoryItemId);
            if (item == null)
                return NotFound($"Item with ID {sale.InventoryItemId} not found.");

            if (sale.QuantitySold <= 0)
                return BadRequest("QuantitySold must be greater than zero.");

            if (item.QuantityInStock < sale.QuantitySold)
                return BadRequest("Not enough stock available.");

            // Save the sale
            sale.SoldAt = DateTime.UtcNow;
            sale.SellingPriceAtTime = sale.SellingPriceAtTime == 0 ? item.SellingPrice : sale.SellingPriceAtTime;
            _context.SaleRecords.Add(sale);

            // Update inventory
            item.QuantityInStock -= sale.QuantitySold;

            await _context.SaveChangesAsync();

            return Ok(sale);
        }

        // GET: api/sales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleRecord>>> GetSales()
        {
            return await _context.SaleRecords
                .Include(s => s.InventoryItem)
                .OrderByDescending(s => s.SoldAt)
                .ToListAsync();
        }
    }
}
