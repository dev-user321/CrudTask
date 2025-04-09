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
        private readonly AppDbContext _context;
        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1 , int take = 5)
        {
            var employees = await _context.Employees.Where(m => m.IsDelete == false)
                .Skip((page*take) - take)
                .Take(take)
                .ToListAsync();
            int totalPageCount = await RoundPageCount(take);

            Pagination<Employee> pagination = new Pagination<Employee>(employees, totalPageCount, page);
            return View(pagination);
        }
        private async Task<int> GetAllEmployeeCount()
        {
            return await _context.Employees.Where(m=> m.IsDelete == false).CountAsync();    
        }
        private async Task<int> RoundPageCount(int take)
        {
            int totalCount = await GetAllEmployeeCount();
            return (int)Math.Ceiling((decimal)totalCount / take);

        }
        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null) return BadRequest();

            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null) return BadRequest();

            if(employee.IsActive)
            {
                employee.IsActive = false;
            }else
            {
                employee.IsActive = true;
            }
            await _context.SaveChangesAsync();  
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null) return BadRequest();

            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null) return BadRequest();
            employee.IsDelete = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
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

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");   
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Employee employee)
        {
            if (id == null) return BadRequest();

            var existedEmployee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);
            if (existedEmployee == null) return NotFound();


            existedEmployee.FullName = employee.FullName;
            existedEmployee.Age = employee.Age;
            existedEmployee.Position = employee.Position;
            existedEmployee.IsActive  = employee.IsActive;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }

}
