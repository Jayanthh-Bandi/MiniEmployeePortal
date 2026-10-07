using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniEmployeePortal.DTOs;
using MiniEmployeePortal.Models;
using MiniEmployeePortal.Services;

namespace MiniEmployeePortal.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<DepartmentResponseDto>>> GetAllDepartmentsAsync()
        {
            var dept = await _departmentService.GetAllDepartmentsAsync();
            var response = dept.Select(dept => new DepartmentResponseDto
            {
                Id=dept.DepartmentId,
                DepartmentName=dept.DepartmentName,
            }).ToList();
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DepartmentResponseDto>> GetDepartmentById(int id)
        {
            var dept = await _departmentService.GetDepartmentByIdAsync(id);
            if (dept == null)
            {
                return NotFound("Department Id not found");
            }
            var response = new DepartmentResponseDto
            {
                Id=dept.DepartmentId,
                DepartmentName = dept.DepartmentName,
            };
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
[HttpPost]
        public async Task<ActionResult<DepartmentResponseDto>> PostDepartment(DepartmentCreateDto dto)
        {
            var department = new Department 
            { DepartmentName=dto.DepartmentName };

            var dept = await _departmentService.AddDepartmentAsync(department);

            var response = new DepartmentResponseDto    
            {
                Id=dept.DepartmentId,
                DepartmentName = dept.DepartmentName
            };
           
            return CreatedAtAction(nameof(GetDepartmentById), new { id = dept.DepartmentId }, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<DepartmentResponseDto>> UpdateDepartmentAsync(int id, DepartmentUpdateDto dto)
        {
            var department = new Department
            {
                DepartmentId = id,
                DepartmentName = dto.DepartmentName,
            };
            var dept = await _departmentService.UpdateDepartmentAsync(department);
            if (dept == null)
            {
                return NotFound("Department Id not found");
            }
            var response = new DepartmentResponseDto
            {
                Id = id,
                DepartmentName = dept.DepartmentName,
            };

            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
            var dept = await _departmentService.DeleteDepartmentAsync(id);
            if (dept == null)
            {
                return NotFound("Department Id not found");
            }
            return NoContent();
        }

    }
}
