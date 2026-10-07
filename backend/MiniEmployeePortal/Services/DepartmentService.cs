using MiniEmployeePortal.Models;
using MiniEmployeePortal.Repositories;

namespace MiniEmployeePortal.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepo;

        public DepartmentService(IDepartmentRepository departmentrepo)
        {
            _departmentRepo = departmentrepo;

        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            return await _departmentRepo.GetAllDepartmentsAsync();
        }
        public async Task<Department?> GetDepartmentByIdAsync(int departmentId)
        {
            return await _departmentRepo.GetDepartmentByIdAsync(departmentId);
        }
        public async Task<Department> AddDepartmentAsync(Department department)
        {
            var deptExist = await _departmentRepo.DepartmentExistsByNameAsync(department.DepartmentName);
            if (deptExist)
            {
                throw new InvalidOperationException("Department already exist");
            }
            return await _departmentRepo.AddDepartmentAsync(department);
        }
        public async Task<Department> UpdateDepartmentAsync(Department department)
        {

            var existingDepartment =
                await _departmentRepo.GetDepartmentByIdAsync(department.DepartmentId);

            if (existingDepartment is null)
            {
                throw new KeyNotFoundException("Department Id Not Found");
            }
            existingDepartment.DepartmentName = department.DepartmentName;
            return await _departmentRepo.UpdateDepartmentAsync(department);
        }

        public async Task<Department> DeleteDepartmentAsync(int DepartmentId)
        {
            var dept = await _departmentRepo.GetDepartmentByIdAsync(DepartmentId);
            if (dept is null)
            {
                throw new KeyNotFoundException("Department Id Not Found");
            }
            await _departmentRepo.DeleteDepartmentAsync(dept);
            return dept;
        }

        
    }
}
