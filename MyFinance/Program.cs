using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyFinance.Managers;

namespace MyFinance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            #region SQL Server Configuration

            builder.Services.AddDbContext<Context.MyFinanceDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            #endregion SQL Server Configuration

            builder.Services.AddScoped<ServiceManager>();
            builder.Services.AddScoped<TransactionManager>();
            builder.Services.AddScoped<UserManager>();

            #region Authentication Configuration

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(config =>
                {
                    config.LoginPath = "/Accounts/Login";
                    config.Cookie.Name = "UserLoginCookie";
                    config.AccessDeniedPath = "/Accounts/AccessDenied";
                    config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                });

            #endregion Authentication Configuration

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Accounts}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}