namespace ERP.Services.API.Models.RequestModels.Quotation;

public class QuotationProductResource
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public float Discount { get; set; }
    public float Amount { get; set; }
    public int Order { get; set; }
    public bool IsApproved { get; set; }
    public double TotalAmount { get; set; }
    public string Unit { get; set; } = "";
    public decimal LatestCost { get; set; }
    public decimal TotalLatestCost { get; set; }
    public decimal Profit { get; set; }
    public decimal ProfitPercent { get; set; }
    public decimal CostEstimate { get; set; }
    public decimal CostEstimatePercent { get; set; }
    public decimal TotalCostEstimate { get; set; }
    public decimal CostEstimateProfit { get; set; }
    public string TotalProfit { get; set; }
    public decimal CostEstimateProfitPercent { get; set; }
    public decimal TotalCostEstimateProfit { get; set; }
}