using Expense_Tracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Get SQL Server password from configuration
var sqlPassword = builder.Configuration["SQLServer:Password"];

// Register Syncfusion License
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBPh8sVXJzS0d+WFlPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9nSH9RdURkWXtacHBSQGY=");

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure connection string for the database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

if (!string.IsNullOrEmpty(sqlPassword))
{
    connectionString = connectionString.Replace("{your_password}", sqlPassword);
}

// Configure DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add Identity services with custom ApplicationUser
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Adjust based on your requirements
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddRazorPages();

var app = builder.Build();

// Seed roles and admin user during application startup with logging for debugging
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await RoleSeeder.SeedRolesAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding roles and admin user.");
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Ensure authentication middleware is added
app.UseAuthorization();

// Map routes for controllers with default settings
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Razor Pages if using them
app.MapRazorPages();

app.Run(); // Run the application