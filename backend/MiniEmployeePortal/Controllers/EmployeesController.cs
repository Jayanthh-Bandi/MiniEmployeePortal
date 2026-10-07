using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniEmployeePortal.DTOs;
using MiniEmployeePortal.Models;
using MiniEmployeePortal.Services;
using System.Security.Claims;

namespace MiniEmployeePortal.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(
            IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyEmployeeProfile()
        {
            var userIdValue =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized();
            }

            var employee =
                await _employeeService.GetEmployeeByUserIdAsync(userId);

            if (employee == null)
            {
                return NotFound(new
                {
                    message =
                        "Employee profile not found for this user."
                });
            }

            return Ok(employee);
        }

        [HttpGet]
        public async Task<ActionResult<List<EmployeeResponseDto>>> GetAllAsync()
        {
            var employees =
                await _employeeService.GetAllEmployeesAsync();

            var response = employees
                .Select(e => new EmployeeResponseDto
                {
                    EmployeeId = e.EmployeeId,
                    Name = e.Name,
                    Email = e.Email,
                    Salary = e.Salary,
                    DepartmentId = e.DepartmentId,
                    DepartmentName =
                        e.Department?.DepartmentName
                        ?? string.Empty
                })
                .ToList();

            return Ok(response);
        }

        [HttpGet("{id:int}", Name = "GetEmployeeById")]
        public async Task<ActionResult<EmployeeResponseDto>> GetByIdAsync(int id)
        {
            var emp =
                await _employeeService.GetEmployeeByIdAsync(id);

            if (emp is null)
            {
                return NotFound(new
                {
                    message =
                        $"Employee with ID {id} was not found."
                });
            }

            var response = new EmployeeResponseDto
            {
                EmployeeId = emp.EmployeeId,
                Name = emp.Name,
                Email = emp.Email,
                Salary = emp.Salary,
                DepartmentId = emp.DepartmentId,
                DepartmentName =
                    emp.Department?.DepartmentName
                    ?? string.Empty
            };

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<EmployeeResponseDto>> AddEmployeeAsync(
            EmployeeCreateDto dto)
        {
            var employee = new Employee
            {
                Name = dto.Name,
                Email = dto.Email,
                Salary = dto.Salary,
                DepartmentId = dto.DepartmentId
            };

            var emp =
                await _employeeService.AddEmployeeAsync(employee);

            var response = new EmployeeResponseDto
            {
                EmployeeId = emp.EmployeeId,
                Name = emp.Name,
                Email = emp.Email,
                Salary = emp.Salary,
                DepartmentId = emp.DepartmentId,
                DepartmentName =
                    emp.Department?.DepartmentName
                    ?? string.Empty
            };

            return CreatedAtRoute(
                "GetEmployeeById",
                new { id = emp.EmployeeId },
                response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<EmployeeResponseDto>> UpdateEmployeeAsync(
    int id,
    EmployeeUpdateDto dto)
        {
            var employee = new Employee
            {
                EmployeeId = id,
                Name = dto.Name,
                Email = dto.Email,
                Salary = dto.Salary,
                DepartmentId = dto.DepartmentId
            };

            var emp =
                await _employeeService.UpdateEmployeeAsync(employee);

            var response = new EmployeeResponseDto
            {
                EmployeeId = emp.EmployeeId,
                Name = emp.Name,
                Email = emp.Email,
                Salary = emp.Salary,
                DepartmentId = emp.DepartmentId,
                DepartmentName =
                    emp.Department?.DepartmentName
                    ?? string.Empty
            };

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployeeAsync(int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);

            return NoContent();
        }
    }
}