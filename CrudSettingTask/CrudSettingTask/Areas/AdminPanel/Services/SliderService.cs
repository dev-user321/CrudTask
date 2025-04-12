using CrudSettingTask.Data;
using CrudSettingTask.Models;
using CrudSettingTask.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Areas.AdminPanel.Services
{
    public class SliderService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<List<Slider>> GetAllAsync()
        {
            return await _context.Sliders.Where(x => !x.IsDelete).ToListAsync();
        }

        public async Task<Slider> GetByIdAsync(int id)
        {
            return await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateAsync(SliderCreateVM vm)
        {
            foreach (var photo in vm.Photos)
            {
                var fileName = await UploadFileAsync(photo, "img");
                var slider = new Slider { Image = fileName };
                await _context.Sliders.AddAsync(slider);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (slider == null) return;

            string fullPath = Path.Combine(_env.WebRootPath, "img", slider.Image);
            DeleteFile(fullPath);

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, IFormFile photo)
        {
            var slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (slider == null) return;

            string oldPath = Path.Combine(_env.WebRootPath, "img", slider.Image);
            DeleteFile(oldPath);

            string newFileName = await UploadFileAsync(photo, "img");
            slider.Image = newFileName;

            await _context.SaveChangesAsync();
        }


        private async Task<string> UploadFileAsync(IFormFile file, string folderPath)
        {
            string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string fullPath = Path.Combine(_env.WebRootPath, folderPath, fileName);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        private void DeleteFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
