using CrudSettingTask.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ProductImageController : Controller
    {
        private readonly AppDbContext _context;
        public ProductImageController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var productImages = await _context.ProductImages.Where(m => !m.IsDelete).ToListAsync();
            return View(productImages);
        }
    }
}
