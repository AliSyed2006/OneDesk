using Microsoft.AspNetCore.Mvc;
using OneDesk.API.Data;
using OneDesk.API.Models;
using Microsoft.EntityFrameworkCore;

namespace OneDesk.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/inventory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetItems()
        {
            return await _context.InventoryItems.ToListAsync();
        }

        // GET: api/inventory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryItem>> GetItem(int id)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item == null)
                return NotFound();

            return item;
        }

        // POST: api/inventory
        [HttpPost]
        public async Task<ActionResult<InventoryItem>> AddItem(InventoryItem item)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            _context.InventoryItems.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }

        // PUT: api/inventory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, InventoryItem item)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            if (id != item.Id)
                return BadRequest();

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.InventoryItems.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/inventory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.InventoryItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
