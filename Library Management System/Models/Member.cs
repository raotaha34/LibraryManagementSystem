using System;
using System.Collections.Generic;

namespace Library_Management_System.Models;

public partial class Member
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime UpdatedAt { get; set; }

    public string UpdatedBy { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public int RoleId { get; set; }

    public string PasswordHash { get; set; } = null!;

    public virtual RoleMaster Role { get; set; } = null!;
}
