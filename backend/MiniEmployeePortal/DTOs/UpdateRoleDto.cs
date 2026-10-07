using System.ComponentModel.DataAnnotations;

namespace MiniEmployeePortal.DTOs
{
    public class UpdateRoleDto
    {
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
