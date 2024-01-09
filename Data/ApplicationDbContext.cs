using Dashboard.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cart>()
                .Property(c => c.Total)
                .HasColumnType("decimal(18, 2)"); // Example precision and scale
            modelBuilder.Entity<Order>()
              .Property(c => c.TotalAmount)
              .HasColumnType("decimal(18, 2)");

                modelBuilder.Entity<OrderDetail>()
              .Property(c => c.PricePerItem)
              .HasColumnType("decimal(18, 2)");
            // Configure mappings for other entities and properties here
        }
    }
}