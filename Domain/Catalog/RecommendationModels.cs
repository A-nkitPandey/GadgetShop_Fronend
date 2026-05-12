namespace GadgetShop.Models;

public sealed class BackendRecommendationRequest
{
    public long? ProductId { get; set; }
    public int PageSize { get; set; } = 8;
    public string? SessionId { get; set; }
}
