using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public sealed class PlaceOrderRequest
{
    [Range(1, long.MaxValue)] public long ShippingAddressId { get; set; }
    [Range(1, long.MaxValue)] public long BillingAddressId { get; set; }
    public string? CouponCode { get; set; }
    public string? CustomerNote { get; set; }
    public string? IdempotencyKey { get; set; }
}
