using BackEnd.DTOs.Responses;

namespace BackEnd.Services.Users;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetAllUsersAsync();
}