using DrinkShop.DbContext;
using DrinkShop.Models.Entities;
using DrinkShopShop.PersianTranslationError;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.Configuration.AddJsonFile("appsettings.json");

services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

services.AddDbContext<DrinkShopContext>(options =>
{
    options.UseSqlServer(connectionString);
});


services.AddIdentity<User, IdentityRole>(option =>
    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10))
    .AddEntityFrameworkStores<DrinkShopContext>()
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
DbInitializer.Seed(app);
app.Run();
