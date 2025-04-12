using CrudSettingTask.Areas.AdminPanel.Services;
using CrudSettingTask.Data;
using CrudSettingTask.Models;
using CrudSettingTask.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SliderController : Controller
    {
        private readonly SliderService _sliderService;

        public SliderController(SliderService sliderService)
        {
            _sliderService = sliderService;
        }

        public async Task<IActionResult> Index() => View(await _sliderService.GetAllAsync());

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateVM slider)
        {
            await _sliderService.CreateAsync(slider);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            await _sliderService.DeleteAsync(id.Value);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var slider = await _sliderService.GetByIdAsync(id.Value);
            return View(slider);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            var slider = await _sliderService.GetByIdAsync(id.Value);
            return View(slider);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, IFormFile photo)
        {
            if (id == null || photo == null) return RedirectToAction("Index");
            await _sliderService.UpdateAsync(id.Value, photo);
            return RedirectToAction("Index");
        }

    }
}
