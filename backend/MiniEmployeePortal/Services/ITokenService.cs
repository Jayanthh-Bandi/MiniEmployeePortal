using MiniEmployeePortal.Models;

namespace MiniEmployeePortal.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
