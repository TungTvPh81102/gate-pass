using BackEnd.DTOs.Responses;
using BackEnd.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories.Users;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .Select(u => new UserResponse
            {
                Id = u.Id,
                Name = u.Name,
                DepartmentId = u.DepartmentId,
                DepartmentName = u.Department.Name ?? string.Empty,
                Email = u.Email,
                EmployeeId = u.EmployeeId,
                Avatar = u.Avatar ?? string.Empty, 
                Status = u.Status,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
            })
            .ToListAsync();
    }
}