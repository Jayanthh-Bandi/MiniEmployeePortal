using MiniEmployeePortal.Models;

namespace MiniEmployeePortal.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee> AddEmployeeAsync(Employee employee);

        Task<bool> EmailExistsForOtherEmployeeAsync(
    string email,
    int employeeId);

        Task<Employee?> GetEmployeeByEmailAsync(string email);
        Task<bool> DepartmentExistsAsync(int departmentId);

        Task<Employee> UpdateEmployeeAsync(Employee employee);

        Task DeleteEmployeeAsync(Employee employee);
        Task<Employee?> GetByUserIdAsync(int UserId);

    }
}
