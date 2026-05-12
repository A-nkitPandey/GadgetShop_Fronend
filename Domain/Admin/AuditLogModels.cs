using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public sealed class AuditLogListRequest : PaginationRequest { }
public sealed class AuditLogGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }

public sealed class AuditLogDto
{
    public long Id { get; set; }
    public string? ControllerName { get; set; }
    public string? ActionName { get; set; }
    public string? HttpMethod { get; set; }
    public string? UserCode { get; set; }
    public int? ResponseCode { get; set; }
    public bool IsSuccess { get; set; }
    public DateTime CreatedAt { get; set; }
}
