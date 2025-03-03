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
    public double LatestCost { get; set; }
    public double TotalLatestCost { get; set; }
    public double Profit { get; set; }
    public double ProfitPercent { get; set; }
    public double CostEstimate { get; set; }
    public double CostEstimatePercent { get; set; }
    public double TotalCostEstimate { get; set; }
    public double CostEstimateProfit { get; set; }
    public double TotalProfit { get; set; }
    public double CostEstimateProfitPercent { get; set; }
    public int TotalCostEstimateProfit { get; set; }
}