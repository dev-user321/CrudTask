using CrudSettingTask.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public FooterViewComponent(AppDbContext context)
        {
            _context = context; 
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var socialMedia = await _context.Socials.Where(m => m.IsDelete == false).OrderByDescending(m=>m.Id).Take(2).ToListAsync(); 
            return View(socialMedia);
        }
    }
}
