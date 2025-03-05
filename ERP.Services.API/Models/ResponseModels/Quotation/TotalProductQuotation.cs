namespace ERP.Services.API.Models.ResponseModels.Quotation;

public class TotalProductQuotation
{
    public Guid QuotationId { get; set; }
    public double TotalAmount { get; set; }
    public double TotalCost { get; set; }
    public double Profit { get; set; }
    public double TotalProfitPercent { get; set; }
    public decimal TotalEstimate { get; set; }
    public double TotalEstimatePercent { get; set; }
    public decimal TotalEstimateProfit { get; set; }
    public double TotalEstimateProfitPercent { get; set; }
}