using Microsoft.AspNetCore.Mvc;
using OneDesk.API.Data;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace OneDesk.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryExportController : ControllerBase
    {
        private readonly AppDbContext _context;
        public InventoryExportController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/InventoryExport/download
        [HttpGet("download")]
        public async Task<IActionResult> DownloadCsv()
        {
            var items = await _context.InventoryItems.ToListAsync();

            var sb = new StringBuilder();
            // Header row
            sb.AppendLine("Id,Name,SKU,Description,Category,Supplier,QuantityInStock,CostPrice,SellingPrice,CreatedAt");

            foreach (var i in items)
            {
                // Escape commas in fields if needed
                string Escape(string s) => $"\"{s.Replace("\"", "\"\"")}\"";
                sb.AppendLine(string.Join(',',
                    i.Id,
                    Escape(i.Name),
                    Escape(i.SKU),
                    Escape(i.Description),
                    Escape(i.Category),
                    Escape(i.Supplier),
                    i.QuantityInStock,
                    i.CostPrice,
                    i.SellingPrice,
                    i.CreatedAt.ToString("o")
                ));
            }

            var csvBytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(csvBytes, "text/csv", "onedesk_inventory.csv");
        }
    }
}
