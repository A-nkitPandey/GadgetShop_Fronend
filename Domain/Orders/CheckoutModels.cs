using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public sealed class PlaceOrderRequest
{
    [Range(1, long.MaxValue)] public long AddressId { get; set; }
    public string? Notes { get; set; }
}
