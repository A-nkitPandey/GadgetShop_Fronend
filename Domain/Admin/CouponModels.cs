using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public class CreateCouponRequest
{
    public long? Id { get; set; }
    [Required] public string CouponCode { get; set; } = string.Empty;
    [Required] public string CouponName { get; set; } = string.Empty;
    [Required] public string DiscountType { get; set; } = string.Empty;
    [Required] public decimal DiscountValue { get; set; }
    public decimal? MinimumOrderAmount { get; set; }
    public decimal? MaximumDiscountAmount { get; set; }
    public string? Description { get; set; }
    public DateTime ValidFrom { get; set; } = DateTime.Today;
    public DateTime ValidTo { get; set; } = DateTime.Today.AddDays(30);
}

public sealed class UpdateCouponRequest : CreateCouponRequest { }
public sealed class CouponGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }
public sealed class CouponStatusUpdateRequest { [Range(1, long.MaxValue)] public long Id { get; set; } public bool IsActive { get; set; } }

public sealed class CouponDto
{
    public long Id { get; set; }
    public string? CouponCode { get; set; }
    public string? CouponName { get; set; }
    public string? DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public bool IsActive { get; set; }
}
