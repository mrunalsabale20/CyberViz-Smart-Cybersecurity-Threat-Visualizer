using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartCyberViz.Data;
using SmartCyberViz.Models;
using SmartCyberViz.Repositories;
using SmartCyberViz.Repositories.Interfaces;
using SmartCyberViz.Services;
using SmartCyberViz.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ── Database ──────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Identity ──────────────────────────────────────────────────────────────────
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ── Auth Cookie ───────────────────────────────────────────────────────────────
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// ── Repositories ──────────────────────────────────────────────────────────────
builder.Services.AddScoped<IThreatLogRepository, ThreatLogRepository>();
builder.Services.AddScoped<IIPReportRepository, IPReportRepository>();
builder.Services.AddScoped<IPhishingCheckRepository, PhishingCheckRepository>();
builder.Services.AddScoped<IPasswordCheckRepository, PasswordCheckRepository>();

// ── API Services ──────────────────────────────────────────────────────────────
builder.Services.AddHttpClient<IAbuseIPService, AbuseIPService>();
builder.Services.AddHttpClient<IVirusTotalService, VirusTotalService>();
builder.Services.AddHttpClient<IHaveIBeenPwnedService, HaveIBeenPwnedService>();
builder.Services.AddHttpClient<IIPStackService, IPStackService>();

// ── MVC ───────────────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ── Middleware Pipeline ───────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ── Seed Roles on Startup ─────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    foreach (var role in new[] { "Admin", "User" })
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

app.Run();