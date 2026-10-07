using MiniEmployeePortal.DTOs;
using MiniEmployeePortal.Models;

namespace MiniEmployeePortal.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
       
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
        Task<Employee> DeleteEmployeeAsync(int id);
        Task<EmployeeResponseDto?> GetEmployeeByUserIdAsync(int userId);
    }
}
