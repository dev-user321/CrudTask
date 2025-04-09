using CrudSettingTask.Data;
using CrudSettingTask.Helper;
using CrudSettingTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context; 
        }
        //public async Task<IActionResult> Index()
        //{
        //    var categories = await _context.Categories.Where(m=>!m.IsDelete).ToListAsync();
        //    return View(categories);
        //}

        public async Task<IActionResult> Index(int page = 1,int take = 5)
        {
            var categories = await _context.Categories
                .Where(m=>!m.IsDelete)
                .Skip((page*take) - take)
                .Take(take)
                .ToListAsync();

            //int totalCount = await GetTotalCategories();
            int totalPageCount = await RoundPageCount(take);
            Pagination<Category> pagination = new Pagination<Category>(categories,totalPageCount,page);
            return View(pagination);
        }

        private async Task<int> GetTotalCategories()
        {
            return await _context.Categories.Where(m=>!m.IsDelete).CountAsync();    
        }
        private async Task<int> RoundPageCount(int take)
        {
            int totalCount = await GetTotalCategories();
            return (int)Math.Ceiling((decimal)totalCount / take);

        }
    }
}

