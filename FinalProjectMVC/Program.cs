using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Data;
using FinalProjectMVC.Models;
using FinalProjectMVC.RepositoryPattern;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(
    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

builder.Services.AddScoped<IRepository<Admin>, AdminRepoService>();
builder.Services.AddScoped<IRepository<Brand>, BrandRepoService>();
builder.Services.AddScoped<IRepository<CartItem>, CartItemsRepoService>();
builder.Services.AddScoped<IRepository<Category>, CategoryRepoService>();
builder.Services.AddScoped<IRepository<Customer>, CustomerRepoService>();
builder.Services.AddScoped<IRepository<OrderItem>, OrderItemRepoService>();
builder.Services.AddScoped<IRepository<Product>, ProductRepoService>();
builder.Services.AddScoped<IRepository<Report>, ReportRepoService>();
builder.Services.AddScoped<IRepository<Review>, ReviewRepoService>();
builder.Services.AddScoped<IRepository<SellerProduct>, SellerProductRepoService>();
builder.Services.AddScoped<IRepository<Seller>, SellerRepoService>();
builder.Services.AddScoped<IRepository<SubCategory>, SubCategoryRepoService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();



app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
