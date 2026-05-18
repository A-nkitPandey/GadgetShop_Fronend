namespace GadgetShop.Models;

public sealed class AdminDashboardReportRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int TopCount { get; set; } = 5;
}

public sealed class AdminDashboardSummaryDto
{
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalProducts { get; set; }
    public int TotalCustomers { get; set; }
    public int PendingOrders { get; set; }
    public int LowStockProducts { get; set; }
}

public sealed class AdminDashboardOverviewDto
{
    public AdminDashboardSummaryDto? Summary { get; set; }
    public List<SalesChartPoint> MonthlySales { get; set; } = new();
    public List<TopProductDto> TopProducts { get; set; } = new();
    public List<RecentOrderDto> RecentOrders { get; set; } = new();
}

public sealed class SalesChartPoint
{
    public string? Label { get; set; }
    public decimal Amount { get; set; }
    public int OrderCount { get; set; }
}

public sealed class TopProductDto
{
    public string? ProductName { get; set; }
    public int SoldQty { get; set; }
    public decimal Revenue { get; set; }
}

public sealed class RecentOrderDto
{
    public long OrderId { get; set; }
    public string? OrderNo { get; set; }
    public string? CustomerName { get; set; }
    public decimal Amount { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
