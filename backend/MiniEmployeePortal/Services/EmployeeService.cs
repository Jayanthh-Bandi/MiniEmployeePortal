using MiniEmployeePortal.DTOs;
using MiniEmployeePortal.Models;
using MiniEmployeePortal.Repositories;

namespace MiniEmployeePortal.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        // CREATE
        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            if (employee.Salary <= 0)
            {
                throw new ArgumentException(
                    "Salary must be greater than zero.");
            }

            // Check department using EXISTS.
            // We don't need to load/track the Department entity.
            var departmentExists =
                await _employeeRepository.DepartmentExistsAsync(
                    employee.DepartmentId);

            if (!departmentExists)
            {
                throw new KeyNotFoundException(
                    "Department does not exist.");
            }

            // IMPORTANT:
            // GetEmployeeByEmailAsync returns Task<Employee?>,
            // so we MUST await it.
            var existingEmployee =
                await _employeeRepository.GetEmployeeByEmailAsync(
                    employee.Email);

            if (existingEmployee != null)
            {
                throw new InvalidOperationException(
                    "Email already exists.");
            }

            return await _employeeRepository.AddEmployeeAsync(employee);
        }


        // DELETE
        public async Task<Employee> DeleteEmployeeAsync(int id)
        {
            var emp =
                await _employeeRepository.GetEmployeeByIdAsync(id);

            if (emp == null)
            {
                throw new KeyNotFoundException(
                    "Employee ID not found.");
            }

            await _employeeRepository.DeleteEmployeeAsync(emp);

            return emp;
        }


        // GET ALL
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllEmployeesAsync();
        }


        // GET BY ID
        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetEmployeeByIdAsync(id);
        }


        // UPDATE
        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            if (employee.Salary <= 0)
            {
                throw new ArgumentException(
                    "Salary must be greater than zero.");
            }

            var existingEmployee =
                await _employeeRepository.GetEmployeeByIdAsync(
                    employee.EmployeeId);

            if (existingEmployee == null)
            {
                throw new KeyNotFoundException(
                    $"Employee with ID {employee.EmployeeId} was not found.");
            }

            var emailExists =
                await _employeeRepository
                    .EmailExistsForOtherEmployeeAsync(
                        employee.Email,
                        employee.EmployeeId);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    "Another employee already uses this email.");
            }

            var departmentExists =
                await _employeeRepository.DepartmentExistsAsync(
                    employee.DepartmentId);

            if (!departmentExists)
            {
                throw new KeyNotFoundException(
                    "Department does not exist.");
            }

            existingEmployee.Name = employee.Name;
            existingEmployee.Email = employee.Email;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.DepartmentId = employee.DepartmentId;

            await _employeeRepository.UpdateEmployeeAsync(
                existingEmployee);

            // Reload so DepartmentName is also current
            var updatedEmployee =
                await _employeeRepository.GetEmployeeByIdAsync(
                    existingEmployee.EmployeeId);

            return updatedEmployee!;
        }


        // GET EMPLOYEE BY USER ID
        public async Task<EmployeeResponseDto?> GetEmployeeByUserIdAsync(
            int userId)
        {
            var employee =
                await _employeeRepository.GetByUserIdAsync(userId);

            if (employee == null)
            {
                throw new KeyNotFoundException(
                    "Employee with the given UserId does not exist.");
            }

            return new EmployeeResponseDto
            {
                EmployeeId = employee.EmployeeId,
                Name = employee.Name,
                Email = employee.Email,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId,
                DepartmentName =
                    employee.Department?.DepartmentName
                    ?? string.Empty
            };
        }
    }
}