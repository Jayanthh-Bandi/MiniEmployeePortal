using MiniEmployeePortal.Models;

namespace MiniEmployeePortal.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(User user);
        Task AddUserAsync(User user);
        Task DeleteUserAsync(User user);

    }
}
