using Microsoft.AspNetCore.Identity;
using MiniEmployeePortal.Data;
using MiniEmployeePortal.DTOs;
using MiniEmployeePortal.Models;
using MiniEmployeePortal.Repositories;

namespace MiniEmployeePortal.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AppDbContext _context;
        public AuthService(IUserRepository userRepository, IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository, IPasswordHasher<User> passwordHasher, AppDbContext context)
        {
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _passwordHasher = passwordHasher;
            _context = context;
        }

        public async Task<User> RegisterAsync(string userName, string email, string password, decimal Salary, int departmentId)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }

            var departmentExists = await _departmentRepository.GetDepartmentByIdAsync(departmentId);
            if (departmentExists == null)
            {
                throw new Exception("Invalid department selected.");
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = new User
                {
                    UserName = userName,
                    Email = email,
                    Role="Employee"

                };
                user.PasswordHash = _passwordHasher.HashPassword(user, password);
                await _userRepository.AddUserAsync(user);

                var employee = new Employee
                {
                    Name = user.UserName,
                    UserId = user.UserId,
                    Email = user.Email,
                    Salary = Salary,
                    DepartmentId = departmentId
                };
                await _employeeRepository.AddEmployeeAsync(employee);
                await transaction.CommitAsync();
                return user;

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return null;
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Success)
            {
                return user;
            }
            return null;

        }
        public async Task DeleteAsync(int userId, int currentUserId)
        {
            if (userId == currentUserId)
            {
                throw new Exception("You cannot delete your own account.");
            }
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            await _userRepository.DeleteUserAsync(user);

        }
        public async Task UpdateUserRoleAsync(
    int userId,
    int currentUserId,
    string role)
        {
            if (userId == currentUserId)
            {
                throw new InvalidOperationException(
                    "You cannot change your own role.");
            }

            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (!string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(role, "Employee", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "Role must be either Admin or Employee.");
            }

            user.Role = role;

            await _userRepository.UpdateUserAsync(user);
        }
        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return users.Select(user => new UserResponseDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            }).ToList();
        }
    }
}
