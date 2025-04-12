using CrudSettingTask.Data;
using CrudSettingTask.Models;
using CrudSettingTask.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Areas.AdminPanel.Services
{
    public class SliderDescriptionService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderDescriptionService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<List<SliderDescription>> GetAllAsync()
        {
            return await _context.SliderDescriptions.Where(x => !x.IsDelete).ToListAsync();
        }

        public async Task<SliderDescription> GetByIdAsync(int id)
        {
            return await _context.SliderDescriptions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(SliderDescriptionVM vm)
        {
            var fileName = await UploadFileAsync(vm.Photo, "img");

            var entity = new SliderDescription
            {
                Title = vm.Title,
                Description = vm.Description,
                Image = fileName
            };

            await _context.SliderDescriptions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return;

            string fullPath = Path.Combine(_env.WebRootPath, "img", entity.Image);
            DeleteFile(fullPath);

            _context.SliderDescriptions.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, SliderDescriptionVM vm)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return;

            string oldPath = Path.Combine(_env.WebRootPath, "img", entity.Image);
            DeleteFile(oldPath);

            string newFileName = await UploadFileAsync(vm.Photo, "img");

            entity.Title = vm.Title;
            entity.Description = vm.Description;
            entity.Image = newFileName;

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
