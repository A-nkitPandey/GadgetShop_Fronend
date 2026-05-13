using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public class CreateRoleRequest
{
    [Required] public string RoleCode { get; set; } = string.Empty;
    [Required] public string RoleName { get; set; } = string.Empty;
    public List<long> PermissionIds { get; set; } = new();
}

public sealed class UpdateRoleRequest : CreateRoleRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class RoleGetByIdRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class RoleListRequest : PaginationRequest { }

public sealed class RoleStatusUpdateRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
    public bool IsActive { get; set; }
}

public sealed class PermissionOptionDto
{
    public long Id { get; set; }
    public string PermissionCode { get; set; } = string.Empty;
    public string PermissionName { get; set; } = string.Empty;
    public string ModuleName { get; set; } = string.Empty;
    public bool IsAssigned { get; set; }
}

public sealed class RoleDto
{
    public long Id { get; set; }
    public string RoleCode { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int PermissionCount { get; set; }
    public List<PermissionOptionDto> Permissions { get; set; } = new();
}
