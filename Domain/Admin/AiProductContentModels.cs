namespace GadgetShop.Models;

public sealed class GenerateProductContentRequestModel
{
    public string ProductName { get; set; } = string.Empty;
    public string? BrandName { get; set; }
    public string? CategoryName { get; set; }
}

public sealed class GenerateProductContentResponseModel
{
    public string Provider { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
    public List<ProductContentSuggestionModel> Suggestions { get; set; } = new();
}

public sealed class ProductContentSuggestionModel
{
    public string ShortDescription { get; set; } = string.Empty;
    public string FullDescription { get; set; } = string.Empty;
    public List<string> Highlights { get; set; } = new();
    public string SeoTitle { get; set; } = string.Empty;
    public List<string> SeoKeywords { get; set; } = new();
}
