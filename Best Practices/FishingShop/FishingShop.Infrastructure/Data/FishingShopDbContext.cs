using FishingShop.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace FishingShop.Infrastructure.Data
{
    public class FishingShopDbContext:DbContext
    {
        public FishingShopDbContext(DbContextOptions<FishingShopDbContext> options):base(options)
        {
            
        }
        public DbSet<FishingRod> FishingRods { get; set; } = null!;
        
    }
}
