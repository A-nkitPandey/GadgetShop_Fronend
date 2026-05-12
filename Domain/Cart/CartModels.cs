using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public sealed class AddCartItemRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
    public long? VariantId { get; set; }
    [Range(1, int.MaxValue)] public int Quantity { get; set; } = 1;
}

public sealed class UpdateCartItemQuantityRequest
{
    [Range(1, long.MaxValue)] public long CartItemId { get; set; }
    [Range(1, int.MaxValue)] public int Quantity { get; set; }
}

public sealed class RemoveCartItemRequest
{
    [Range(1, long.MaxValue)] public long CartItemId { get; set; }
}

public sealed class ApplyCartCouponRequest
{
    [Required] public string CouponCode { get; set; } = string.Empty;
}

public sealed class CartDto
{
    public long Id { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string? AppliedCouponCode { get; set; }
    public string? CurrencyCode { get; set; }
}

public sealed class CartItemDto
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public long? VariantId { get; set; }
    public string? ProductName { get; set; }
    public string? VariantName { get; set; }
    public string? ImageUrl { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public int AvailableStock { get; set; }
}
