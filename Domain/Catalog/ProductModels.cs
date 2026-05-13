using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public sealed class CustomerCatalogListRequest
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SearchText { get; set; }
    public long? CategoryId { get; set; }
    public long? BrandId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }
}

public sealed class CustomerCatalogGetByIdRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class CustomerCatalogProductDto
{
    public long Id { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public decimal? Mrp { get; set; }
    public string? CurrencyCode { get; set; }
    public int StockQuantity { get; set; }
    public bool IsInStock { get; set; }
    public string? PrimaryImageUrl { get; set; }
    public string? CategoryName { get; set; }
    public string? BrandName { get; set; }
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public List<ProductVariantDto> Variants { get; set; } = new();
    public List<string> GalleryImages { get; set; } = new();
}

public sealed class ProductVariantDto
{
    public long Id { get; set; }
    public string? SkuCode { get; set; }
    public string? VariantName { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public List<VariantAttributeDto> Attributes { get; set; } = new();
}

public sealed class VariantAttributeDto
{
    public string? AttributeName { get; set; }
    public string? ValueText { get; set; }
}

public sealed class ProductReviewMutationRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
    [Range(1, 5)] public int Rating { get; set; }
    [Required, StringLength(1000)] public string ReviewText { get; set; } = string.Empty;
}

public sealed class ProductReviewDeleteRequest
{
    [Range(1, long.MaxValue)] public long ReviewId { get; set; }
}

public sealed class ProductReviewListRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public sealed class ProductReviewDto
{
    public long Id { get; set; }
    public string? ReviewerName { get; set; }
    public int Rating { get; set; }
    public string? ReviewText { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateProductRequest
{
    public long? Id { get; set; }
    [Required] public string ProductCode { get; set; } = string.Empty;
    [Required] public string ProductName { get; set; } = string.Empty;
    [Required] public long CategoryId { get; set; }
    [Required] public long BrandId { get; set; }
    [Required] public decimal BasePrice { get; set; }
    public decimal? Mrp { get; set; }
    public int StockQuantity { get; set; }
    public decimal? CostPrice { get; set; }
    public int ReservedQuantity { get; set; }
    public int ReorderLevel { get; set; }
    public bool TrackInventory { get; set; } = true;
    public string? Description { get; set; }
    public string CurrencyCode { get; set; } = "INR";
    public bool HasVariants { get; set; }
    public string? PrimaryImageUrl { get; set; }
}

public sealed class UpdateProductRequest : CreateProductRequest { }

public sealed class ProductGetByIdRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class ProductStatusUpdateRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
    public bool IsActive { get; set; }
}

public sealed class AdminProductDto
{
    public long Id { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public long? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public long? BrandId { get; set; }
    public string? BrandName { get; set; }
    public decimal BasePrice { get; set; }
    public decimal? Mrp { get; set; }
    public decimal? CostPrice { get; set; }
    public string CurrencyCode { get; set; } = "INR";
    public bool TrackInventory { get; set; }
    public int StockQuantity { get; set; }
    public int ReservedQuantity { get; set; }
    public int ReorderLevel { get; set; }
    public bool IsActive { get; set; }
    public bool HasVariants { get; set; }
    public string? Description { get; set; }
    public string? PrimaryImageUrl { get; set; }
}

public class CreateProductVariantRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
    [Required] public string SkuCode { get; set; } = string.Empty;
    [Required] public string VariantName { get; set; } = string.Empty;
    [Required] public string AttributeSummary { get; set; } = string.Empty;
    [Required] public decimal BasePrice { get; set; }
    public decimal? Mrp { get; set; }
    public decimal? CostPrice { get; set; }
    public string CurrencyCode { get; set; } = "INR";
    public bool TrackInventory { get; set; } = true;
    public int StockQuantity { get; set; }
    public int ReservedQuantity { get; set; }
    public int ReorderLevel { get; set; }
}

public sealed class UpdateProductVariantRequest : CreateProductVariantRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class ProductVariantGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }
public sealed class ProductVariantListRequest : PaginationRequest { public long? ProductId { get; set; } }
public sealed class ProductVariantStatusUpdateRequest { [Range(1, long.MaxValue)] public long Id { get; set; } public bool IsActive { get; set; } }

public sealed class ProductImageUploadRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
    public byte[]? ImageContent { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "image/jpeg";
}

public sealed class ProductGalleryImageUploadRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
    [Range(0, int.MaxValue)] public int DisplayOrder { get; set; }
    public bool IsPrimary { get; set; }
    public byte[]? ImageContent { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "image/jpeg";
}

public sealed class ProductGalleryGetByProductIdRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
}

public sealed class ProductGalleryImageDeleteRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
    [Range(1, long.MaxValue)] public long ProductImageId { get; set; }
}

public sealed class ProductPrimaryImageUpdateRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
    [Range(1, long.MaxValue)] public long ProductImageId { get; set; }
}

public sealed class ProductGalleryImageDto
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsPrimary { get; set; }
}

public sealed class ProductImageUploadDto
{
    public long ProductId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public sealed class AdminProductVariantDto
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string SkuCode { get; set; } = string.Empty;
    public string VariantName { get; set; } = string.Empty;
    public string AttributeSummary { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public decimal? Mrp { get; set; }
    public decimal? CostPrice { get; set; }
    public string CurrencyCode { get; set; } = "INR";
    public bool TrackInventory { get; set; }
    public int StockQuantity { get; set; }
    public int ReservedQuantity { get; set; }
    public int AvailableQuantity { get; set; }
    public int ReorderLevel { get; set; }
    public bool IsActive { get; set; }
    public List<VariantAttributeMappingDto> AttributeMappings { get; set; } = new();
}
