using System.ComponentModel.DataAnnotations;

namespace MiniEmployeePortal.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        [Required]
        public string DepartmentName { get; set; }=string.Empty;
        public ICollection<Employee> Employees { get; set; } = new List<Employee> { };
    }
}
