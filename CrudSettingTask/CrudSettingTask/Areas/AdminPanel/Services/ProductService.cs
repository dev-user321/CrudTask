using CrudSettingTask.Data;
using CrudSettingTask.Models;
using CrudSettingTask.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Areas.AdminPanel.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                .Where(m => !m.IsDelete)
                .Include(m => m.Category)
                .Include(m => m.Images)
                .ToListAsync();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.Where(m => !m.IsDelete).ToListAsync();
        }

        public async Task CreateAsync(ProductCreateVM vm)
        {
            List<ProductImage> images = new List<ProductImage>();
            foreach (var photo in vm.Photos)
            {
                string fileName = Guid.NewGuid().ToString() + photo.FileName;
                string path = Path.Combine(_env.WebRootPath, "img", fileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                images.Add(new ProductImage { Image = fileName });
            }

            Product product = new Product
            {
                Title = vm.Title,
                Description = vm.Description,
                Price = vm.Price,
                CategoryId = vm.CategoryId,
                CreatedTime = DateTime.Now,
                Images = images
            };

            await _context.ProductImages.AddRangeAsync(images);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(m => m.Category)
                .Include(m => m.Images)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(int id, ProductCreateVM vm)
        {
            var product = await GetByIdAsync(id);
            if (product == null) return;

            foreach (var image in product.Images)
            {
                string oldPath = Path.Combine(_env.WebRootPath, "img", image.Image);
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            List<ProductImage> newImages = new List<ProductImage>();
            foreach (var photo in vm.Photos)
            {
                string fileName = Guid.NewGuid().ToString() + photo.FileName;
                string path = Path.Combine(_env.WebRootPath, "img", fileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                newImages.Add(new ProductImage { Image = fileName });
            }

            product.Title = vm.Title;
            product.Description = vm.Description;
            product.Price = vm.Price;
            product.CategoryId = vm.CategoryId;
            product.CreatedTime = DateTime.Now;
            product.Images = newImages;

            await _context.SaveChangesAsync();
        }
    }
}
