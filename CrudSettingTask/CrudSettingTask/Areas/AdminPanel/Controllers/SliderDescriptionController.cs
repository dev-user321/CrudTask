using CrudSettingTask.Data;
using CrudSettingTask.Models;
using CrudSettingTask.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SliderDescriptionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderDescriptionController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _env = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sliderDescription = await _context.SliderDescriptions.Where(m => m.IsDelete == false).ToListAsync();
            return View(sliderDescription);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var sliderDescription = await _context.SliderDescriptions.FirstOrDefaultAsync(m => m.Id == id);
            if (sliderDescription == null) return NotFound();

            string oldPath = Path.Combine(_env.WebRootPath, "img", sliderDescription.Image);
            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }
            _context.SliderDescriptions.Remove(sliderDescription);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var sliderDescription = await _context.SliderDescriptions.FirstOrDefaultAsync(m => m.Id == id);
            if (sliderDescription == null) return NotFound();
            return View(sliderDescription);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SliderDescriptionVM sliderDescription)
        {
            if(sliderDescription == null) return BadRequest();  
            string fileName = Guid.NewGuid().ToString() + "_" + sliderDescription.Photo.FileName;
            string path = Path.Combine(_env.WebRootPath, "img", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await sliderDescription.Photo.CopyToAsync(stream);
            }
            SliderDescription newSliderDescription = new SliderDescription()
            {
                Title = sliderDescription.Title,    
                Description = sliderDescription.Description,    
                Image = fileName
            };
            await _context.SliderDescriptions.AddAsync(newSliderDescription); 
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            var sliderDescription = await _context.SliderDescriptions.FirstOrDefaultAsync(m => m.Id == id);

            if (sliderDescription == null) return NotFound();

            SliderDescriptionVM sliderDescriptionVM = new SliderDescriptionVM()
            {
                Title = sliderDescription.Title,
                Description = sliderDescription.Description,
                
            };
            return View(sliderDescriptionVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, SliderDescriptionVM sliderDescriptionVM)
        {
            if (id == null) return BadRequest();
            var sliderDescription = await _context.SliderDescriptions.FirstOrDefaultAsync(m => m.Id == id);
            if (sliderDescription == null) return NotFound();
            string oldPath = Path.Combine(_env.WebRootPath, "img", sliderDescription.Image);
            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }
            string fileName = Guid.NewGuid().ToString() + sliderDescriptionVM.Photo.FileName;
            string newPath = Path.Combine(_env.WebRootPath, "img", fileName);
            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await sliderDescriptionVM.Photo.CopyToAsync(stream);
            }
            sliderDescription.Title = sliderDescriptionVM.Title;
            sliderDescription.Description = sliderDescriptionVM.Description;
            sliderDescription.Image = fileName;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
