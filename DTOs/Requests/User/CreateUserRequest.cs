namespace BackEnd.DTOs.Requests.User;

public class CreateUserRequest
{
    public int DepartmentId { get; set; }
    
    public string EmployeeId { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    
    public string? Avatar { get; set; }
    
    public string Password { get; set; } = null!;

    public byte Status { get; set; } = 1;
    
    public string? CreatedBy { get; set; }
}