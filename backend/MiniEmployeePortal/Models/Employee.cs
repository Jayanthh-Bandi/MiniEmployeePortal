using System.ComponentModel.DataAnnotations;

namespace MiniEmployeePortal.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        // Foreign Key
        public int DepartmentId { get; set; }

        // Navigation Property
        public Department Department { get; set; } = null!;

        public int? UserId { get; set;  }
        public User? user { get; set; }  
    }
}