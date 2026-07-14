namespace BackEnd.DTOs.Responses;

public class UserResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string EmployeeId { get; set; } = string.Empty;

    public int DepartmentId { get; set; }
    
    public string DepartmentName { get; set; } = string.Empty;
    
    public string Email { get; set; } = null!;
    
    public string Avatar { get; set; } =  string.Empty;
    
    public byte Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}