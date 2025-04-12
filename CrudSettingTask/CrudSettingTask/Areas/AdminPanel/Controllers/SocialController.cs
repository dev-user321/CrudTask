using CrudSettingTask.Areas.AdminPanel.Services;
using CrudSettingTask.Data;
using CrudSettingTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SocialController : Controller
    {
        private readonly SocialService _socialService;

        public SocialController(SocialService socialService)
        {
            _socialService = socialService;
        }

        public async Task<IActionResult> Index()
        {
            var socials = await _socialService.GetAllAsync();
            return View(socials);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            await _socialService.DeleteAsync(id.Value);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Social social)
        {
            if (!ModelState.IsValid) return View(social);

            bool exists = await _socialService.ExistsAsync(social.Name, social.Url);
            if (exists)
            {
                ViewBag.Message = "Bu Sosial Şəbəkə artıq mövcuddur";
                return View();
            }

            await _socialService.CreateAsync(social);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var social = await _socialService.GetByIdAsync(id.Value);
            if (social == null) return NotFound();

            return View(social);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var social = await _socialService.GetByIdAsync(id.Value);
            if (social == null) return NotFound();

            return View(social);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Social social)
        {
            if (id == null || !ModelState.IsValid) return View(social);

            await _socialService.UpdateAsync(id.Value, social);
            return RedirectToAction(nameof(Index));
        }

    }
}
