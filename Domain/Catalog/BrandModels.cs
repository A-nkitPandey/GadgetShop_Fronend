using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public class CreateBrandRequest
{
    public long? Id { get; set; }
    [Required] public string BrandCode { get; set; } = string.Empty;
    [Required] public string BrandName { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? Description { get; set; }
}

public sealed class UpdateBrandRequest : CreateBrandRequest { }
public sealed class BrandGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }
public sealed class BrandStatusUpdateRequest { [Range(1, long.MaxValue)] public long Id { get; set; } public bool IsActive { get; set; } }

public sealed class BrandDto
{
    public long Id { get; set; }
    public string? BrandCode { get; set; }
    public string? BrandName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
