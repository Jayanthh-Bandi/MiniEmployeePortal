using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniEmployeePortal.DTOs;
using MiniEmployeePortal.Services;
using System.Security.Claims;

namespace MiniEmployeePortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ITokenService tokenService;
        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            this.authService = authService;
            this.tokenService = tokenService;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterDto(RegisterDto dto)
        {
            var user = await authService.RegisterAsync(dto.UserName, dto.Email, dto.Password, dto.Salary, dto.DepartmentId);

            var response = new AuthResponseDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };
            return Created("", response);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var role = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new
            {
                userId,
                userName,
                email,
                role
            });
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(LoginDto dto)
        {
            var user = await authService.LoginAsync(dto.Email, dto.Password);
            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "invalid Email or password"
                });
            }
            var token = tokenService.GenerateToken(user);
            var response = new AuthResponseDto
            {
                Token = token,
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };
            return Ok(response);

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("users/{userId:int}")]
        public async Task<IActionResult> DeleteUserAsync(int userId)
        {
            var currentUserIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(currentUserIdValue, out var currentUserId))
            {
                return Unauthorized();
            }
            await authService.DeleteAsync(userId, currentUserId);
            return NoContent();

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("users/{userId:int}/role")]
        public async Task<IActionResult> UpdateUserRole(
    int userId,
    UpdateRoleDto dto)
        {
            var currentUserIdValue =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(currentUserIdValue, out var currentUserId))
            {
                return Unauthorized();
            }

            await authService.UpdateUserRoleAsync(
                userId,
                currentUserId,
                dto.Role);

            return Ok(new
            {
                message = "User role updated successfully.",
                userId,
                role = dto.Role
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await authService.GetAllUsersAsync();

            return Ok(users);
        }

    }
}
