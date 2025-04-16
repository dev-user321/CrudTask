using CrudSettingTask.Areas.AdminPanel.Services;
using CrudSettingTask.Data;
using CrudSettingTask.Models;
using CrudSettingTask.Services;
using CrudSettingTask.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace CrudSettingTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(opt => {
                opt.IOTimeout = TimeSpan.FromSeconds(5);
            });

            builder.Services.AddSqlServer<AppDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));
            builder.Services.AddIdentity<AppUser,IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<EmployeeService>();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<SliderService>();
            builder.Services.AddScoped<SliderDescriptionService>();
            builder.Services.AddScoped<SocialService>();
            builder.Services.AddScoped<BlogService>();
            builder.Services.AddScoped<IFileService,FileService>();
            builder.Services.AddScoped<IEmailService,EmailService>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false; 

                options.SignIn.RequireConfirmedEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 5; 
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMilliseconds(5); 
                options.Lockout.AllowedForNewUsers = true; 


                options.User.RequireUniqueEmail = true; 
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
