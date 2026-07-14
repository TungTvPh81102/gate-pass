using BackEnd.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var result = await _userService.GetAllUsersAsync();
            
            return Success(result, result.Any() ? "Get users successfully" : "No users found");
        }
    }
}