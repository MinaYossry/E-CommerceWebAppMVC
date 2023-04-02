using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.Areas.Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SellerProduct>()
                .HasIndex("SellerId", "ProductId")
                .IsUnique();

            builder.Entity<Product>()
                .HasIndex(p => p.SerialNumber)
                .IsUnique();

            builder.Entity<OrderItem>()
                .HasOne(p => p.SellerProduct)
                .WithMany(s => s.OrderItems)
                .HasForeignKey(p => p.SellerProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CartItem>()
                .HasOne(p => p.SellerProduct)
                .WithMany(s => s.CartItems)
                .HasForeignKey(p => p.SellerProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Report>()
                .HasOne(p => p.Review)
                .WithMany(s => s.Reports)
                .HasForeignKey(p => p.ReviewId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Report>()
                .Property(r => r.CreatedDate)
                .HasDefaultValueSql("getdate()");

            builder.Entity<Review>()
                .Property(r => r.CreatedDate)
                .HasDefaultValueSql("getdate()");

            builder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                 .HasOne(r => r.Seller)
                 .WithMany()
                 .HasForeignKey(r => r.SellerId)
                 .OnDelete(DeleteBehavior.Restrict);
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Seller> Sellers { get; set; }
        public virtual DbSet<SellerProduct> SellerProducts { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }

        public virtual DbSet<Admin> Admins { get; set; }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        //  public DbSet<FinalProjectMVC.Areas.SellerPanel.ViewModel.SellerOrderItemViewModel> SellerOrderItemViewModel { get; set; } = default!;
    }
}