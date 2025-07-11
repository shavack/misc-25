using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using TodoListApi.Application.Dtos;
using TodoListApi.Domain;

namespace TodoListApi.Application.Services;

public interface IAuthService
{
    string GenerateJwtToken(string username, int userId);
    bool ValidateUserCredentials(string username, string password);
    Task<User> RegisterUserAsync(RegisterRequest request);
    Task<List<User>> GetAllUsersAsync();
    Task<bool> ChangePasswordAsync(string username, string newPassword);
    Task<User> GetUserByUsernameAsync(string username);
}