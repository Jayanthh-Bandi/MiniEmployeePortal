using System.ComponentModel.DataAnnotations;

namespace MiniEmployeePortal.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = string.Empty;

        [Range(1, double.MaxValue)]
        public decimal Salary { get; set; }


        [Range(1,int.MaxValue)]
        public int DepartmentId {  get; set; }
    }
}
