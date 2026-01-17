using DrinkShop.Data;
using DrinkShop.Models;
using DrinkShopShop.PersianTranslationError;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.Configuration.AddJsonFile("appsettings.json");

services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();


services.AddDbContext<DrinkShopDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB_CONNECTION_STRING"));
});


services.AddIdentity<User, IdentityRole>(option =>
    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10))
    .AddEntityFrameworkStores<DrinkShopDbContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<PersianIdentityErrorDescriber>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
Database_Initializer.Seed(app);
app.Run();
