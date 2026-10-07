using MiniEmployeePortal.Models;

namespace MiniEmployeePortal.DTOs
{
    public class EmployeeResponseDto
    {
        public int EmployeeId {  get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }=string.Empty;
    }
}
