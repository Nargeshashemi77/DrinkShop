using DrinkShop.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DrinkShop.DbContext
{
    public partial class DrinkShopContext : IdentityDbContext<User, IdentityRole, string>
    {
        public DrinkShopContext(DbContextOptions<DrinkShopContext> options) : base(options)
        {

        }
        public DbSet<Product> products { get; set; }
        public DbSet<Group> groups { get; set; }

        public DbSet<Buyer> buyers { get; set; }
        public DbSet<SubGroup> subGroups { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Favorite> favorites { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            AddModelCreatingConfiguration(modelBuilder);

            #region AddNameAndFamilyToIdentityUserTabel
            modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserRole<string>>();
            modelBuilder.Ignore<IdentityUserClaim<string>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityUser<string>>();
            modelBuilder.Ignore<User>();

            #endregion

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
            .HasOne(b => b.buyer)
            .WithOne(i => i.user)
            .HasForeignKey<Buyer>(b => b.userId);
            modelBuilder.Entity<Favorite>()
                .HasKey(f => new { f.buyerId, f.productId });
        }
    }
}
