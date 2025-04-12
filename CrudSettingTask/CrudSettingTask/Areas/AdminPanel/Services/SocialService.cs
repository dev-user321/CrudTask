using CrudSettingTask.Data;
using CrudSettingTask.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Areas.AdminPanel.Services
{
    public class SocialService
    {
        private readonly AppDbContext _context;

        public SocialService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Social>> GetAllAsync()
        {
            return await _context.Socials.Where(m => !m.IsDelete).ToListAsync();
        }

        public async Task<Social?> GetByIdAsync(int id)
        {
            return await _context.Socials.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> ExistsAsync(string name, string url)
        {
            return await _context.Socials.AnyAsync(
                m => m.Name.Trim().ToLower() == name.Trim().ToLower() &&
                     m.Url.Trim().ToLower() == url.Trim().ToLower());
        }

        public async Task CreateAsync(Social social)
        {
            await _context.Socials.AddAsync(social);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var social = await _context.Socials.FirstOrDefaultAsync(m => m.Id == id);
            if (social == null) return;

            social.IsDelete = true;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, Social updated)
        {
            var social = await _context.Socials.FirstOrDefaultAsync(m => m.Id == id);
            if (social == null) return;

            social.Name = updated.Name;
            social.Url = updated.Url;
            await _context.SaveChangesAsync();
        }
    }
}
