namespace GadgetShop.Models;

public sealed class RunArchiveRequest
{
    public bool IsDryRun { get; set; }
}

public sealed class RunArchiveDto
{
    public string BatchId { get; set; } = string.Empty;
    public bool IsDryRun { get; set; }
    public DateTime CutoffDate { get; set; }
    public List<ArchiveTableResultDto> Results { get; set; } = new();
}

public sealed class ArchiveTableResultDto
{
    public string TableName { get; set; } = string.Empty;
    public DateTime CutoffDate { get; set; }
    public int SelectedCount { get; set; }
    public int ArchivedCount { get; set; }
    public int DeletedCount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
}
