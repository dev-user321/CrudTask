using CrudSettingTask.Areas.AdminPanel.Services;
using CrudSettingTask.Data;
using CrudSettingTask.Helper;
using CrudSettingTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            var pagination = await _employeeService.GetPaginatedEmployeesAsync(page, take);
            return View(pagination);
        }

        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null) return BadRequest();

            var result = await _employeeService.ToggleEmployeeStatusAsync(id.Value);
            if (!result) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var result = await _employeeService.SoftDeleteEmployeeAsync(id.Value);
            if (!result) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (employee == null) return BadRequest();

            await _employeeService.CreateEmployeeAsync(employee);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee == null) return NotFound();

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Employee employee)
        {
            if (id == null) return BadRequest();

            var result = await _employeeService.UpdateEmployeeAsync(id.Value, employee);
            if (!result) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }

}
