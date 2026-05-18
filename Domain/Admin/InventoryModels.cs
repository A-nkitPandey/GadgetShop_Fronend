using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public sealed class InventoryAdjustmentRequest
{
    [Range(1, long.MaxValue)] public long ProductId { get; set; }
    public long? VariantId { get; set; }
    [Range(1, int.MaxValue)] public int Quantity { get; set; }
    public string? ReferenceNo { get; set; }
    public string? ReferenceType { get; set; }
    public string? Remarks { get; set; }
}

public sealed class ProductInventoryTransactionListRequest : PaginationRequest
{
    public long? ProductId { get; set; }
}

public sealed class ProductInventoryTransactionGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }

public sealed class InventoryAdminReportRequest
{
    public long? ProductId { get; set; }
    public bool LowStockOnly { get; set; } = true;
}

public sealed class InventoryTransactionDto
{
    public long Id { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? VariantName { get; set; }
    public string? TransactionType { get; set; }
    public int Quantity { get; set; }
    public string? ReferenceNo { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
}

public sealed class InventoryLowStockDto
{
    public long ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? VariantName { get; set; }
    public int AvailableQuantity { get; set; }
    public int ReorderLevel { get; set; }
}
