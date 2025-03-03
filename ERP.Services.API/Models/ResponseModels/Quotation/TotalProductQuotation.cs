namespace ERP.Services.API.Models.ResponseModels.Quotation;

public class TotalProductQuotation
{
    public Guid QuotationId { get; set; }
    public double TotalAmount { get; set; }
    public double TotalCost { get; set; }
    public double Profit { get; set; }
    public double TotalProfitPercent { get; set; }
    public double TotalEstimate { get; set; }
    public double TotalEstimatePercent { get; set; }
    public double TotalEstimateProfit { get; set; }
    public double TotalEstimateProfitPercent { get; set; }
}