using HappyProgramming_SWP391_GROUP1.Controllers;
using HappyProgramming_SWP391_GROUP1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    option =>
    {
        option.LoginPath = "/Login/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);

    });
builder.Services.AddSignalR();
builder.Services.AddSession(x =>
{
    x.IdleTimeout = TimeSpan.FromMinutes(10);
});
builder.Services.AddHttpContextAccessor();
var app = builder.Build();
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "signup",    // đặt tên route
    defaults: new { controller = "SignUp", action = "Index" },
    pattern: "signup");
app.MapHub<ChatHub>("/chatHub");
app.Run();