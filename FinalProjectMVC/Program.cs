using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Data;
using FinalProjectMVC.Models;
using FinalProjectMVC.RepositoryPattern;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using FinalProjectMVC.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Main DB
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Identity Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseLazyLoadingProxies().UseSqlServer(connectionString));

#endregion


#region Identity customization
// Adjusted after adding new Identity class

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

/* ApplicationUser class => We created it, it ineherites from IdentityUser
 *  Note: Also adjusted in _LoginPartial.cshtml
 *  
   IdentityRole is the default class.*/

/*builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders()
    .AddUserManager<UserManager<ApplicationUser>>(); // Note included in video but needed.*/

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

// Service for manging profile picture.
builder.Services.AddScoped<IFileService, FileService>();   



#endregion



builder.Services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(
    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));



//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
// Store Context

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


#region Services using => Repository pattern scopes 

builder.Services.AddScoped<IRepository<Admin>, AdminRepoService>();
builder.Services.AddScoped<IRepository<Order>, OrderRepoService>();
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

#endregion





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
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}/{SellerId?}"
    );

app.MapControllerRoute(
      name: "DeleteReview",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}/{reportId?}"
    );

app.MapControllerRoute(
    name: "SellerProduct",
    pattern: "{controller=Home}/{action=Index}/{id?}/{SellerId?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();



#region Roles

// Must be added after the `build`, so that all the builds would be available for it
using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedRolesAndAdminAsync(scope.ServiceProvider);
}

#endregion



app.Run();
