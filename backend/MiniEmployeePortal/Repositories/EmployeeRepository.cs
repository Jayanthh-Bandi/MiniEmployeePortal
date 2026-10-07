
using Microsoft.EntityFrameworkCore;
using MiniEmployeePortal.Data;
using MiniEmployeePortal.Models;

namespace MiniEmployeePortal.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee?> GetEmployeeByEmailAsync(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<bool> DepartmentExistsAsync(int DepartmentId)
        {
            return await _context.Departments.AnyAsync(d => d.DepartmentId == DepartmentId);
        }

        public async Task DeleteEmployeeAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }



        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.Include(e => e.Department).AsNoTracking().ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.EmployeeId == id);


        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return employee;
        }
        public async Task<Employee?> GetByUserIdAsync(int userId)
        {
            return await _context.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.UserId == userId);
        }

        public async Task<bool> EmailExistsForOtherEmployeeAsync(
    string email,
    int employeeId)
        {
            return await _context.Employees
                .AnyAsync(e =>
                    e.Email == email &&
                    e.EmployeeId != employeeId);
        }


    }
}
