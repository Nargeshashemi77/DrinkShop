using DrinkShop.Models;
using Microsoft.EntityFrameworkCore;

namespace DrinkShop.Data
{
    public partial class DrinkShopDbContext
    {
        internal void AddModelCreatingConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasQueryFilter(p => p.IsDelete == false);
            modelBuilder.Entity<Buyer>().HasQueryFilter(b => b.IsDelete == false);
            modelBuilder.Entity<Group>().HasQueryFilter(g => g.IsDelete == false);
            modelBuilder.Entity<SubGroup>().HasQueryFilter(sg => sg.IsDelete == false);            
        }
    }
}
