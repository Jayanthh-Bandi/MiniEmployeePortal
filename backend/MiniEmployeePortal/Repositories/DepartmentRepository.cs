using Microsoft.EntityFrameworkCore;
using MiniEmployeePortal.Data;
using MiniEmployeePortal.Models;

namespace MiniEmployeePortal.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;
        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments.AsNoTracking().ToListAsync();
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
        }
        public async Task<Department> AddDepartmentAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }
        public async Task<Department> UpdateDepartmentAsync(Department department)
        {
            
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task DeleteDepartmentAsync(Department department)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

        }
        public async Task<bool> DepartmentExistsByNameAsync(string departmentName)
        {
            return await _context.Departments
                .AnyAsync(d => d.DepartmentName == departmentName);
        }
    }
}
