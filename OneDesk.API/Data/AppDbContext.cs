using Microsoft.EntityFrameworkCore;
using OneDesk.API.Models;

namespace OneDesk.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) {}

        public DbSet<SaleRecord> SaleRecords { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
    }
}
