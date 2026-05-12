using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public sealed class WishlistMutationRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
}

public sealed class WishlistRemoveRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
}

public sealed class WishlistItemDto
{
    public long ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public bool IsInStock { get; set; }
}
