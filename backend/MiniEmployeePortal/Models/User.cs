using System.ComponentModel.DataAnnotations;

namespace MiniEmployeePortal.Models
{
    public class User
    {
        
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public Employee? employee { get; set; }

    }
}
