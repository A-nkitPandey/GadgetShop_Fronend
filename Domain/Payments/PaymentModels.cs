using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public sealed class CreatePaymentOrderRequest
{
    [Range(1, long.MaxValue)] public long OrderId { get; set; }
    [StringLength(100)] public string? IdempotencyKey { get; set; }
}

public sealed class VerifyPaymentRequest
{
    public string? RazorpayOrderId { get; set; }
    public string? RazorpayPaymentId { get; set; }
    public string? RazorpaySignature { get; set; }
    public long OrderId { get; set; }
}

public sealed class RetryPaymentRequest
{
    [Range(1, long.MaxValue)] public long OrderId { get; set; }
    [StringLength(100)] public string? IdempotencyKey { get; set; }
}

public sealed class PaymentOrderDto
{
    public string? GatewayOrderId { get; set; }
    public decimal Amount { get; set; }
    public string? Currency { get; set; }
    public string? GatewayKey { get; set; }
    public long OrderId { get; set; }
    public string? UserEmail { get; set; }
    public string? UserName { get; set; }
    public string? UserPhone { get; set; }
}

public sealed class PaymentGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }
public sealed class PaymentStatusUpdateRequest { [Range(1, long.MaxValue)] public long Id { get; set; } [Required] public string Status { get; set; } = string.Empty; }

public sealed class ProcessPaymentRefundRequest
{
    [Range(1, long.MaxValue)] public long PaymentId { get; set; }
    [Range(0.01, double.MaxValue)] public decimal RefundAmount { get; set; }
    public string? Remarks { get; set; }
    public string? Reason { get => Remarks; set => Remarks = value; }
}

public sealed class PaymentAdminDto
{
    public long Id { get; set; }
    public string? PaymentNo { get; set; }
    public string? OrderNo { get; set; }
    public string? CustomerName { get; set; }
    public decimal Amount { get; set; }
    public decimal RefundedAmount { get; set; }
    public string? CurrencyCode { get; set; }
    public string? GatewayName { get; set; }
    public string? TransactionId { get; set; }
    public string? PaymentStatus { get; set; }
    public DateTime? PaymentDate { get; set; }
    public DateTime? RefundedAt { get; set; }
}
