using CrudSettingTask.Areas.AdminPanel.Services;
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
        private readonly SliderDescriptionService _sliderDescriptionService;

        public SliderDescriptionController(SliderDescriptionService service)
        {
            _sliderDescriptionService = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var items = await _sliderDescriptionService.GetAllAsync();
            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            await _sliderDescriptionService.DeleteAsync(id.Value);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            var item = await _sliderDescriptionService.GetByIdAsync(id.Value);
            if (item == null) return NotFound();

            return View(item);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(SliderDescriptionVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _sliderDescriptionService.CreateAsync(vm);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var item = await _sliderDescriptionService.GetByIdAsync(id.Value);
            if (item == null) return NotFound();

            var vm = new SliderDescriptionVM
            {
                Title = item.Title,
                Description = item.Description
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, SliderDescriptionVM vm)
        {
            if (id == null) return BadRequest();

            await _sliderDescriptionService.UpdateAsync(id.Value, vm);
            return RedirectToAction("Index");
        }
    }
}
