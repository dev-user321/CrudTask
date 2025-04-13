using CrudSettingTask.Data;
using CrudSettingTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class BlogHeaderController : Controller
    {
        private readonly AppDbContext _context;
        public BlogHeaderController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var blogHeaders = await _context.BlogHeaders.Where(m => !m.IsDelete).ToListAsync();
            return View(blogHeaders);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var blogHeader  = await _context.BlogHeaders.Where(m=>!m.IsDelete).FirstOrDefaultAsync(m=>m.Id == id);
            if (blogHeader == null) return NotFound();

            blogHeader.IsDelete = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var blogHeader = await _context.BlogHeaders.Where(m=>!m.IsDelete).FirstOrDefaultAsync(m=>m.Id==id);
            if (blogHeader == null) return NotFound();

            return View(blogHeader);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]  
        public async Task<IActionResult> Create(BlogHeader blogHeader)
        {
            if (blogHeader == null) return BadRequest();
            await _context.BlogHeaders.AddAsync(blogHeader);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            var blogHeader = await _context.BlogHeaders.Where(m => !m.IsDelete).FirstOrDefaultAsync(m => m.Id == id);
            if (blogHeader == null) return NotFound();  
            return View(blogHeader);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, BlogHeader blogHeader)
        {
            if (id == null) return BadRequest();
            var oldBlogHeader = await _context.BlogHeaders.FirstOrDefaultAsync(m => m.Id == id);
            if (oldBlogHeader == null) return NotFound();
            oldBlogHeader.Title = blogHeader.Title;
            oldBlogHeader.Description = blogHeader.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");   
        }
    }
}
