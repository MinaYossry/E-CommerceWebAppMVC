using FinalProjectMVC.Areas.AdminPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.Models
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions options) : base (options)
        {
            
        }

        public virtual DbSet<Product> Products { get;  set; }
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
    }
}
