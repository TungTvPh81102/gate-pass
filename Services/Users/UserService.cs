using BackEnd.DTOs.Responses;
using BackEnd.Repositories.Users;

namespace BackEnd.Services.Users;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IUserRepository _userRepository;

    public UserService(ILogger<UserService> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
    {
        try
        {
            _logger.LogInformation("Getting all users");

            var users = await _userRepository.GetAllUsersAsync();

            return users;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while getting all users");
            throw;
        }
    }
}