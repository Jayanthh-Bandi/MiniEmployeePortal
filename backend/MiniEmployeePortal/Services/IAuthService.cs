using MiniEmployeePortal.DTOs;
using MiniEmployeePortal.Models;

namespace MiniEmployeePortal.Services
{
    public interface IAuthService
    {
        Task<List<UserResponseDto>> GetAllUsersAsync();
        Task<User> RegisterAsync(string userName, string email, string password, decimal salary, int departmentId);
        Task<User?> LoginAsync(string email, string password);
        Task DeleteAsync(int userId, int currentUserId);
        Task UpdateUserRoleAsync(int userId,int currentUserId,string role);

    }
}
    