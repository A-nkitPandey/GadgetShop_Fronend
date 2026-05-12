using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public sealed class CreateAdminUserRequest
{
    [Required] public string UserName { get; set; } = string.Empty;
    [Required] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
    [Required] public string RoleCode { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}

public sealed class UpdateAdminUserRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
    [Required] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string RoleCode { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}

public sealed class AdminUserGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }
public sealed class AdminUserListRequest : PaginationRequest { }
public sealed class AdminUserStatusUpdateRequest { [Range(1, long.MaxValue)] public long Id { get; set; } public bool IsActive { get; set; } }
public sealed class AdminUserArchiveRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }
public sealed class AdminUserPasswordResetRequest { [Range(1, long.MaxValue)] public long Id { get; set; } [Required] public string NewPassword { get; set; } = string.Empty; }

public sealed class AdminUserDto
{
    public long Id { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public List<string> Roles { get; set; } = new();
}
