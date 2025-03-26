using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Entities;

[Table("QuotationProduct")]
public class QuotationProductEntity
{
    [Column("quotation_product_id")]
    public Guid? QuotationProductId { get; set; }
    [Column("quotation_id")] public Guid QuotationId { get; set; }
    public QuotationEntity Quotation { get; set; }
    [Column("product_id")] public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; }

    [Column("quantity")] public int Quantity { get; set; }

    [Column("price")] public float Price { get; set; }
    [Column("order")] public int Order { get; set; }
    [Column("amount")] public float Amount { get; set; }
    [Column("discount")] public float Discount { get; set; }
    
    [Column("amount_before_vat")] public decimal AmountBeforeVat { get; set; }
    [Column("sum_of_discount")] public decimal SumOfDiscount { get; set; }
    [Column("real_price_msrp")] public decimal RealPriceMsrp { get; set; }
    [Column("estimate_price")] public decimal EstimatePrice { get; set; }
    [Column("latest_cost")]public decimal LatestCost { get; set; }
    [Column("profit")]public decimal Profit { get; set; }
    [Column("profit_percent")]public decimal ProfitPercent { get; set; }
    [Column("cost_estimate")]public decimal CostEstimate { get; set; }
    [Column("cost_estimate_percent")]public decimal CostEstimatePercent { get; set; }
    [Column("cost_estimate_profit")]public decimal CostEstimateProfit { get; set; }
    [Column("cost_estimate_profit_percent")]public decimal CostEstimateProfitPercent { get; set; }
    [Column("administrative_costs")] public decimal AdministrativeCosts { get; set; }
    [Column("import_duty")] public decimal ImportDuty { get; set; }
    [Column("wht")] public decimal WHT { get; set; }
}