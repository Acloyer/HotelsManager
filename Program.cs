using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using HotelsManager.Models.Users;
using HotelsManager.Models.Hotels;
using HotelsManager.Models.Orders;
using HotelsManager.Models.Repository;
using HotelsManager.Models.HotelsRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
// Dupper

/*string connectionString = "Server=.\\SQLEXPRESS;Initial Catalog=HotelsDB;Integrated Security=True";*/
string connectionString = @"Data Source=LAPTOP-8U7UGFTE; Initial Catalog = HotelsDB; Integrated Security = SSPI; TrustServerCertificate = True;";
builder.Services.AddTransient<IUserRepository, UserRepository>(provider => new UserRepository(connectionString));
builder.Services.AddTransient<IOrderRepository, OrderRepository>(provider => new OrderRepository(connectionString));
builder.Services.AddTransient<IHotelRepository, HotelRepository>(provider => new HotelRepository(connectionString));
builder.Services.AddControllersWithViews();
//


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Identity/Login";
        options.ReturnUrlParameter = "returnUrl";

        options.AccessDeniedPath = "/Identity/Forbidden";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrators", (policyBuilder) =>
    {
        policyBuilder.RequireClaim(ClaimTypes.Role, "admin");
    });
});


var app = builder.Build();

app.UseExceptionHandler("/Home/Error");
app.UseHsts();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Loading}/{id?}");

app.Run();