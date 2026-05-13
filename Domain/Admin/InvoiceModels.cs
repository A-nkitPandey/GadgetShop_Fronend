using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public sealed class GenerateInvoiceRequest { [Range(1, long.MaxValue)] public long OrderId { get; set; } }
public sealed class InvoiceGetByIdRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }
public sealed class InvoicePrintDocumentRequest { [Range(1, long.MaxValue)] public long Id { get; set; } }

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

public sealed class InvoiceAdminDetailDto
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public string InvoiceNo { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal TaxableAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal GrandTotal { get; set; }
    public DateTime GeneratedAt { get; set; }
    public string GeneratedBy { get; set; } = string.Empty;
}

public sealed class InvoicePrintDocumentDto
{
    public long Id { get; set; }
    public string InvoiceNo { get; set; } = string.Empty;
    public string OrderNo { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = string.Empty;
    public decimal TaxableAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal GrandTotal { get; set; }
    public DateTime GeneratedAt { get; set; }
    public string SuggestedFileName { get; set; } = string.Empty;
    public string PdfFileName { get; set; } = string.Empty;
    public string PdfMimeType { get; set; } = string.Empty;
    public string PdfContentBase64 { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
}
