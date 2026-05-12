using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public sealed class NotificationDto
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

public sealed class SupportTicketMutationRequest
{
    [Required] public string Subject { get; set; } = string.Empty;
    [Required] public string Message { get; set; } = string.Empty;
    public long? OrderId { get; set; }
}

public sealed class SupportTicketReplyRequest
{
    [Range(1, long.MaxValue)] public long TicketId { get; set; }
    [Required] public string Message { get; set; } = string.Empty;
}
