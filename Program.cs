using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Cloud Run Port
builder.WebHost.UseUrls("http://0.0.0.0:8080");

//
// ===================== SERVICES =====================
//

// MVC + Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Database (SQLite)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=/tmp/HospitalDB.db"));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Email Sender
builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();

//
// ===================== ROTATIVA =====================
// Disabled for Cloud Run testing
//

// var env = app.Services.GetRequiredService<IWebHostEnvironment>();
// RotativaConfiguration.Setup(env.WebRootPath, "Rotativa");
// Rotativa.AspNetCore.RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");

//
// ===================== SEED DATA =====================
//

using (var scope = app.Services.CreateScope())
{
    try
    {
        var services = scope.ServiceProvider;

        var db = services.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = { "Admin", "Doctor", "Patient" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

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
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
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