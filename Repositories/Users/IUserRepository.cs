using BackEnd.DTOs.Responses;
using BackEnd.Models.Entities;

namespace BackEnd.Repositories.Users;

public interface IUserRepository
{
    Task<IEnumerable<UserResponse>> GetAllUsersAsync();
    
    // Task<User?> GetUserByIdAsync(int id);
    // Task<bool> ExistsUserAsync(string email);
}