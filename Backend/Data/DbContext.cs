using Microsoft.EntityFrameworkCore;
using PhoneVault.Models;

namespace PhoneVault.Data
{
    public class PhoneVaultContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Shipping> Shipping { get; set; }
        public DbSet<AdminLog> AdminLogs { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<Image> ProductImages { get; set; }
        public DbSet<ImageBlob> ImageBlobs { get; set; }

        public PhoneVaultContext(DbContextOptions<PhoneVaultContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships and constraints here, if needed
            modelBuilder
                .Entity<User>()
                .Property(u => u.AccountType)
                .HasConversion<string>();
        }
    }
}