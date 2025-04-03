using CrudSettingTask.Data;
using CrudSettingTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SocialController : Controller
    {
        private readonly AppDbContext _context;
        public SocialController(AppDbContext context)
        {
            _context = context; 
        }
        public async Task<IActionResult> Index()
        {
            var socials = await _context.Socials.Where(m=>m.IsDelete == false).ToListAsync();
            return View(socials);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var social = await _context.Socials.FirstOrDefaultAsync(m => m.Id == id);
            social.IsDelete = true;
            await _context.SaveChangesAsync();  
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Social social)
        {

            if(social == null) return BadRequest();

            var existedSocial = await _context.Socials
                .FirstOrDefaultAsync(
                m => m.Name.Trim().ToLower() == social.Name.Trim().ToLower() && 
                m.Url.Trim().ToLower() == social.Url.Trim().ToLower());      
            if(existedSocial != null)
            {
                ViewBag.Message = "Bu Sosial Sebeke Movcuddur";
                return View();
            }
            await _context.Socials.AddAsync(social);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var social = await _context.Socials.FirstOrDefaultAsync(m => m.Id == id);
            if (social == null) return NotFound();
            return View(social);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var social = await _context.Socials.FindAsync(id);
            if (social == null) return NotFound();

            return View(social);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Social social)
        {
            if (id == null) return BadRequest();

            var existedSocial = await _context.Socials.FirstOrDefaultAsync(m => m.Id == id);
            if (existedSocial == null) return NotFound();


            existedSocial.Name = social.Name;
            existedSocial.Url = social.Url;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
