using MiniEmployeePortal.Models;

namespace MiniEmployeePortal.Repositories
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllDepartmentsAsync();
        Task<Department?> GetDepartmentByIdAsync(int DepartmentId);

        Task<Department> AddDepartmentAsync(Department department);

        Task<Department> UpdateDepartmentAsync(Department department);
        Task DeleteDepartmentAsync(Department department);
        Task<bool> DepartmentExistsByNameAsync(string departmentName);
    }
}
