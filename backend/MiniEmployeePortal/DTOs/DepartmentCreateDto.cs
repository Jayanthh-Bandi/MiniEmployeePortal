using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace MiniEmployeePortal.DTOs
{
    public class DepartmentCreateDto
    {
        [Required]
        public string DepartmentName { get; set; }=string.Empty;

    }
}
