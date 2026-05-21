using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public sealed class NotificationDto
{
    public long Id { get; set; }
    public string? NotificationType { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
    public string? ReferenceType { get; set; }
    public string? ReferenceNo { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

public sealed class NotificationCountDto
{
    public int TotalCount { get; set; }
    public int UnreadCount { get; set; }
}

public sealed class RegisterPushDeviceRequest
{
    [Required] public string DeviceToken { get; set; } = string.Empty;
    [Required] public string DevicePlatform { get; set; } = string.Empty;
}

public sealed class RemovePushDeviceRequest
{
    [Required] public string DeviceToken { get; set; } = string.Empty;
}

public sealed class SupportTicketMutationRequest
{
    [Range(1, long.MaxValue)] public long? OrderId { get; set; }
    [Required] public string Subject { get; set; } = string.Empty;
    [Required] public string InitialMessage { get; set; } = string.Empty;
    public string Priority { get; set; } = "MEDIUM";
}

public sealed class SupportTicketReplyRequest
{
    [Range(1, long.MaxValue)] public long SupportTicketId { get; set; }
    [Required] public string MessageText { get; set; } = string.Empty;
}

public sealed class AdminSupportTicketListRequest : PaginationRequest
{
    public string? Status { get; set; }
    public string? Priority { get; set; }
}

public sealed class AdminSupportTicketReplyRequest
{
    [Range(1, long.MaxValue)] public long SupportTicketId { get; set; }
    [Required] public string MessageText { get; set; } = string.Empty;
    public string? Status { get; set; }
}

public sealed class SupportTicketDto
{
    public long Id { get; set; }
    public string TicketNo { get; set; } = string.Empty;
    public long? OrderId { get; set; }
    public string? OrderNo { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string InitialMessage { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<SupportTicketMessageDto> Messages { get; set; } = new();
}

public sealed class SupportTicketMessageDto
{
    public long Id { get; set; }
    public long SupportTicketId { get; set; }
    public long UserId { get; set; }
    public bool IsAdminReply { get; set; }
    public string MessageText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
