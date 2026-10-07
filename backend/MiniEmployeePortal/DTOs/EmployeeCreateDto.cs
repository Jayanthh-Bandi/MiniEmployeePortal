using MiniEmployeePortal.Models;
using System.ComponentModel.DataAnnotations;

namespace MiniEmployeePortal.DTOs
{
    public class EmployeeCreateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        
        public string Name { get; set; } = string.Empty;
        [Range(0.01, double.MaxValue)]
        public decimal Salary { get; set; }

        [Required]
        public int DepartmentId {  get; set; }

    }
}
