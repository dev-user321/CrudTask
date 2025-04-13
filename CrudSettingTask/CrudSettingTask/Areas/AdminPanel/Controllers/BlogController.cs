using CrudSettingTask.Areas.AdminPanel.Services;
using CrudSettingTask.Data;
using CrudSettingTask.Models;
using CrudSettingTask.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class BlogController : Controller
    {
        private readonly BlogService _blogService;

        public BlogController(BlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _blogService.GetAllAsync();
            return View(blogs);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var blog = await _blogService.GetByIdAsync(id.Value);
            if (blog == null) return NotFound();
            return View(blog);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogCreateVM blogCreateVM)
        {
            
            await _blogService.CreateAsync(blogCreateVM);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            var blog = await _blogService.GetByIdAsync(id.Value);
            if (blog == null) return NotFound();

            BlogCreateVM vm = new BlogCreateVM()
            {
                Title = blog.Title,
                Description = blog.Description,
                Image = blog.Image
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, BlogCreateVM blogCreateVM)
        {
            if (id == null) return BadRequest();

            var updated = await _blogService.UpdateAsync(id.Value, blogCreateVM);
            if (!updated) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var deleted = await _blogService.SoftDeleteAsync(id.Value);
            if (!deleted) return NotFound();

            return RedirectToAction(nameof(Index));
        }

    }
}
