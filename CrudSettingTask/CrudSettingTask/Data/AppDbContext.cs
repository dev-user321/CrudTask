using CrudSettingTask.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options) { }
   
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Social> Socials { get; set; }  
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Slider> Sliders { get; set; }  
    }
}
