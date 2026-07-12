using System;
using System.Collections.Generic;

namespace BackEnd.Models.Entities;

public partial class User
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    public string EmployeeId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Avatar { get; set; }

    public string Password { get; set; } = null!;

    public string? VerificationToken { get; set; }

    public DateTime? EmailVerifiedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiresAt { get; set; }

    public byte LockCount { get; set; }

    public DateTime? LockedAt { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual Department Department { get; set; } = null!;
}
