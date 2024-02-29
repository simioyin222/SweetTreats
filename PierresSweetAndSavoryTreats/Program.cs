using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using PierresSweetAndSavoryTreats.Models;

namespace Pierres
{
  class Program
  {
    static void Main(string[] args)
    {
      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      builder.Services.AddControllersWithViews();

      builder.Services.AddDbContext<ApplicationDbContext>(
                        dbContextOptions => dbContextOptions
                          .UseMySql(
                            builder.Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"]
                            )
                          )
                        );
      
      builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                  .AddEntityFrameworkStores<ApplicationDbContext>()
                  .AddDefaultTokenProviders();
      builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
    options.Cookie.IsEssential = true; // Make the cookie essential
    // Set the LoginPath, LogoutPath, AccessDeniedPath as needed
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/LogOff";
});

WebApplication app = builder.Build();

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      
      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
      );

      app.Run();
    }
  }
}