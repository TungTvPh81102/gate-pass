using System;
using System.Collections.Generic;

namespace BackEnd.Models.Entities;

public partial class Department
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int? ParentId { get; set; }

    public byte Stat { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<Department> InverseParent { get; set; } = new List<Department>();

    public virtual Department? Parent { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
