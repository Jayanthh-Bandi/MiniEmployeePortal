using MiniEmployeePortal.Models;

namespace MiniEmployeePortal.Services
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllDepartmentsAsync();
        Task<Department?> GetDepartmentByIdAsync(int DepartmentId);

        Task<Department> AddDepartmentAsync(Department department);

        Task<Department> UpdateDepartmentAsync(Department department);
        Task<Department> DeleteDepartmentAsync(int DepartmentId);
    }
}
