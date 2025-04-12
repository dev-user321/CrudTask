using CrudSettingTask.Data;
using CrudSettingTask.Helper;
using CrudSettingTask.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Areas.AdminPanel.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pagination<Category>> GetPaginatedCategoriesAsync(int page, int take)
        {
            var categories = await _context.Categories
                .Where(c => !c.IsDelete)
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();

            int totalCount = await _context.Categories.Where(c => !c.IsDelete).CountAsync();
            int totalPages = (int)Math.Ceiling((decimal)totalCount / take);

            return new Pagination<Category>(categories, totalPages, page);
        }
    }
}
