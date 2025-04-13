using CrudSettingTask.Areas.AdminPanel.Services;
using CrudSettingTask.Data;

namespace CrudSettingTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddSqlServer<AppDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));


            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<EmployeeService>();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<SliderService>();
            builder.Services.AddScoped<SliderDescriptionService>();
            builder.Services.AddScoped<SocialService>();
            builder.Services.AddScoped<BlogService>();

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
