using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//
// ===================== SERVICES =====================
//

// MVC + Razor Pages (Identity needs Razor Pages)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Database (SQLite)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=HospitalDB.db"));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Email sender (required for Identity UI)
builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();

var env = app.Services.GetRequiredService<IWebHostEnvironment>();

RotativaConfiguration.Setup(env.WebRootPath, "Rotativa");
Rotativa.AspNetCore.RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");

//
// ===================== SEED DATA =====================
//

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Role Manager
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "Doctor", "Patient" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // User Manager
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    // Create Admin User
    var adminEmail = "admin@hospital.com";
    var adminPassword = "Admin@123";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(adminUser, adminPassword);
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

//
// ===================== MIDDLEWARE =====================
//

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//
// ===================== ROUTES =====================
//

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();