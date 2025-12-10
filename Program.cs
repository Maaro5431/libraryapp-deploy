using LibraryApp.Data;
using LibraryApp.Hubs;                    // NEW
using LibraryApp.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/libraryapp-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// Database + Identity (same as before)
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("LibraryDb"));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 4;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AppDbContext>();

// NEW: In-memory caching (PA1101, PA1103)
builder.Services.AddMemoryCache();

// NEW: Response caching middleware
builder.Services.AddResponseCaching();

// NEW: SignalR real-time hub (PA1105, PA1106, IAC1102, IAC1103)
builder.Services.AddSignalR();

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddHttpClient();   

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// NEW: Response caching (PA1101)
app.UseResponseCaching();

app.MapControllers();  // THIS LINE ENABLES API CONTROLLERS


// Map SignalR hub
app.MapHub<BookHub>("/bookHub");


// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// === SEED ADMIN USER & ROLE (runs once on startup) ===
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string roleName = "Admin";
    if (!await roleManager.RoleExistsAsync(roleName))
    {
        await roleManager.CreateAsync(new IdentityRole(roleName));
    }

    string adminUserName = "admin";
    string adminPassword = "Pass123";

    var adminUser = await userManager.FindByNameAsync(adminUserName);
    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = adminUserName,
            Email = "admin@library.com",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, roleName);
        }
    }
}

app.Run();