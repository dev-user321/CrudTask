using CrudSettingTask.Data;
using CrudSettingTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context; 
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var sliders = await _context.Sliders.Where(m => m.IsDelete == false).ToListAsync();
            return View(sliders);
        }

        [HttpGet]   
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (slider == null) return NotFound();
            string fileName = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;
            string path = Path.Combine(_env.WebRootPath, "img", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await slider.Photo.CopyToAsync(stream);
            }

            slider.Image = fileName;
            await _context.Sliders.AddAsync(slider);    
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);
            if (slider == null) return NotFound();

            
            string oldImagePath = Path.Combine(_env.WebRootPath, "img", slider.Image);
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if(id == null) return BadRequest();
            var slide = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            return View(slide);
        }

        [HttpGet] 
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            return View(slider);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id,Slider slider)
        {
            if (id == null) return BadRequest();
            if (slider.Photo == null)
            {
                return RedirectToAction("Index");
            }
            
            var updatedSlider = await _context.Sliders.FirstOrDefaultAsync(m=>m.Id == id);
            
            string oldImagePath = Path.Combine(_env.WebRootPath, "img", updatedSlider.Image);
            
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            string fileName = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;

            string path = Path.Combine(_env.WebRootPath, "img", fileName);
            
            using(FileStream stream = new FileStream(path,FileMode.Create))
            {
                await slider.Photo.CopyToAsync(stream); 
            };

            updatedSlider.Image = fileName;

            await _context.SaveChangesAsync();  
            
            return RedirectToAction("Index");  
        }
    }
}
