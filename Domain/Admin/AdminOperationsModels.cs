namespace GadgetShop.Models;

public sealed class ReleaseExpiredCartReservationsRequest
{
    public int BatchSize { get; set; } = 100;
}

public sealed class RecalculateOrderTotalsRequest
{
    public long? OrderId { get; set; }
}

public sealed class AdminOperationResultDto
{
    public int AffectedCount { get; set; }
    public string Message { get; set; } = string.Empty;
}

public sealed class AdminDataIntegrityCheckDto
{
    public long OrdersWithoutPayment { get; set; }
    public long OrdersWithoutInvoice { get; set; }
    public long DeliveredOrdersWithoutShipment { get; set; }
    public long UsersWithoutRole { get; set; }
    public long PaymentsWithoutTransactionId { get; set; }
    public long ActiveProductsWithoutCategoryOrBrand { get; set; }
}
