using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public class CreateCategoryRequest
{
    public long? Id { get; set; }
    [Required] public string CategoryCode { get; set; } = string.Empty;
    [Required] public string CategoryName { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public long? ParentCategoryId { get; set; }
}

public sealed class UpdateCategoryRequest : CreateCategoryRequest { }
public sealed class CategoryGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }
public sealed class CategoryStatusUpdateRequest { [Range(1, long.MaxValue)] public long Id { get; set; } public bool IsActive { get; set; } }

public sealed class CategoryDto
{
    public long Id { get; set; }
    public string? CategoryCode { get; set; }
    public string? CategoryName { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public string? ParentCategoryName { get; set; }
}
