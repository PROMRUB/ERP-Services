namespace ERP.Services.API.Models.ResponseModels.Quotation;

public class QuotationDashboardResponse
{
    public int TotalQuotations { get; set; }
    public decimal TotalValueInclVat { get; set; }
    public decimal TotalValueExVat { get; set; } 
    public List<TopProductItem> TopProducts { get; set; } = new();
    public List<TopCustomerItem> TopCustomers { get; set; } = new();
}

public class TopProductItem
{
    public Guid ProductId { get; set; }
    public string? ProductName { get; set; } 
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }
}

public sealed class TopCustomerItem
{
    public Guid CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public decimal TotalAmount { get; set; }  
}