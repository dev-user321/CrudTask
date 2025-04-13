using CrudSettingTask.Data;
using CrudSettingTask.Models;
using CrudSettingTask.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Areas.AdminPanel.Services
{
    public class BlogService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<List<Blog>> GetAllAsync()
        {
            return await _context.Blogs.Where(m => !m.IsDelete).ToListAsync();
        }

        public async Task<Blog> GetByIdAsync(int id)
        {
            return await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id && !m.IsDelete);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var blog = await GetByIdAsync(id);
            if (blog == null) return false;

            blog.IsDelete = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task CreateAsync(BlogCreateVM blogCreateVM)
        {
            string fileName = Guid.NewGuid().ToString() + blogCreateVM.Photo.FileName;
            string path = Path.Combine(_env.WebRootPath, "img", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await blogCreateVM.Photo.CopyToAsync(stream);
            }

            Blog blog = new Blog()
            {
                Title = blogCreateVM.Title,
                Description = blogCreateVM.Description,
                CreatedTime = DateTime.Now,
                Image = fileName
            };

            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(int id, BlogCreateVM blogCreateVM)
        {
            var oldBlog = await GetByIdAsync(id);
            if (oldBlog == null) return false;

            string oldPath = Path.Combine(_env.WebRootPath, "img", oldBlog.Image);
            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }

            string newFileName = Guid.NewGuid().ToString() + blogCreateVM.Photo.FileName;
            string newPath = Path.Combine(_env.WebRootPath, "img", newFileName);

            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await blogCreateVM.Photo.CopyToAsync(stream);
            }

            oldBlog.Title = blogCreateVM.Title;
            oldBlog.Description = blogCreateVM.Description;
            oldBlog.CreatedTime = DateTime.Now;
            oldBlog.Image = newFileName;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
