using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public sealed class OrderGetByIdRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class OrderListRequest
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public sealed class CancelMyOrderRequest
{
    [Range(1, long.MaxValue)] public long OrderId { get; set; }
    public string? Remarks { get; set; }
}

public sealed class ReorderRequest
{
    [Range(1, long.MaxValue)] public long OrderId { get; set; }
}

public sealed class OrderDto
{
    public long Id { get; set; }
    public string? OrderNo { get; set; }
    public string? CustomerName { get; set; }
    public string? Status { get; set; }
    public int TotalQuantity { get; set; }
    public decimal TotalAmount { get; set; }
    public string? CurrencyCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
    public OrderAddressDto? DeliveryAddress { get; set; }
    public string? PaymentStatus { get; set; }
    public string? TrackingNo { get; set; }
    public OrderShipmentDto? Shipment { get; set; }
    public List<OrderReturnDto> Returns { get; set; } = new();
}

public sealed class OrderItemDto
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ImageUrl { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public sealed class OrderAddressDto
{
    public string? FullName { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PinCode { get; set; }
    public string? PhoneNumber { get; set; }
}

public sealed class UpdateOrderStatusRequest
{
    [Range(1, long.MaxValue)] public long OrderId { get; set; }
    [Required] public string Status { get; set; } = string.Empty;
    public string? Remarks { get; set; }
}

public sealed class CreateOrderShipmentRequest
{
    [Range(1, long.MaxValue)] public long OrderId { get; set; }
    [Required] public string CourierPartner { get; set; } = string.Empty;
    public string? TrackingNo { get; set; }
    public string? Remarks { get; set; }
}

public sealed class UpdateOrderShipmentStatusRequest
{
    [Range(1, long.MaxValue)] public long OrderId { get; set; }
    [Required] public string ShipmentStatus { get; set; } = string.Empty;
    public string? Remarks { get; set; }
}

public sealed class OrderShipmentGetByOrderIdRequest { [Range(1, long.MaxValue)] public long OrderId { get; set; } }
public sealed class OrderShipmentListRequest : PaginationRequest { public string? ShipmentStatus { get; set; } }
public sealed class OrderReturnGetByIdRequest { [Range(1, long.MaxValue)] public long ReturnId { get; set; } }
public sealed class OrderReturnListRequest : PaginationRequest { public string? ReturnStatus { get; set; } }
public sealed class UpdateOrderReturnStatusRequest { [Range(1, long.MaxValue)] public long Id { get; set; } [Required] public string ReturnStatus { get; set; } = string.Empty; public string? AdminRemarks { get; set; } }
public sealed class MyReturnListRequest : PaginationRequest { }

public sealed class CreateOrderReturnRequest
{
    [Range(1, long.MaxValue)] public long OrderId { get; set; }
    [Range(1, long.MaxValue)] public long OrderItemId { get; set; }
    [Range(1, int.MaxValue)] public int Quantity { get; set; } = 1;
    [Required] public string Reason { get; set; } = string.Empty;
}

public sealed class OrderShipmentDto
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public string ShipmentNo { get; set; } = string.Empty;
    public string ShipmentStatus { get; set; } = string.Empty;
    public string CourierPartner { get; set; } = string.Empty;
    public string? TrackingNo { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string? Remarks { get; set; }
}

public sealed class OrderReturnDto
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public string ReturnNo { get; set; } = string.Empty;
    public string ReturnStatus { get; set; } = string.Empty;
    public string? RefundStatus { get; set; }
    public string? Reason { get; set; }
    public int Quantity { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
