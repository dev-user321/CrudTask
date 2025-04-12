using CrudSettingTask.Data;
using CrudSettingTask.Helper;
using CrudSettingTask.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudSettingTask.Areas.AdminPanel.Services
{
    public class EmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pagination<Employee>> GetPaginatedEmployeesAsync(int page, int take)
        {
            var employees = await _context.Employees
                .Where(e => !e.IsDelete)
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();

            int totalCount = await _context.Employees.Where(e => !e.IsDelete).CountAsync();
            int totalPages = (int)Math.Ceiling((decimal)totalCount / take);

            return new Pagination<Employee>(employees, totalPages, page);
        }

        public async Task<bool> ToggleEmployeeStatusAsync(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null) return false;

            employee.IsActive = !employee.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null) return false;

            employee.IsDelete = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task CreateEmployeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> UpdateEmployeeAsync(int id, Employee updatedEmployee)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null) return false;

            employee.FullName = updatedEmployee.FullName;
            employee.Age = updatedEmployee.Age;
            employee.Position = updatedEmployee.Position;
            employee.IsActive = updatedEmployee.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
