using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public sealed class GenerateInvoiceRequest { [Range(1, long.MaxValue)] public long OrderId { get; set; } }
public sealed class InvoiceGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }
public sealed class InvoicePrintDocumentRequest { [Range(1, long.MaxValue)] public long InvoiceId { get; set; } }

public sealed class InvoiceAdminDto
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string? OrderNo { get; set; }
    public string? InvoiceNo { get; set; }
    public string? CustomerName { get; set; }
    public decimal GrandTotal { get; set; }
    public DateTime GeneratedAt { get; set; }
}
