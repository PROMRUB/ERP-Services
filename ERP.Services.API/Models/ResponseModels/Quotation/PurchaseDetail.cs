namespace ERP.Services.API.Models.ResponseModels.Quotation;

public class PurchaseDetail
{
    public string ServiceType { get; set; } = "N/A";
    public string BuyType { get; set; } = "N/A";
    public string BuyProductCode { get; set; } = "N/A";
    public string SellProductName { get; set; } = "N/A";
    public string BuyCurrency { get; set; } = "N/A";
    public string SellCurrency { get; set; } = "N/A";
    public int BuyQuantity { get; set; }
    public int SellQuantity { get; set; }

    public string CurrencyInHand { get; set; } = "N/A";
    public string CurrencyLatest { get; set; } = "N/A";
    public string CurrencyEstimate { get; set; } = "";

    public string BuyUnitInHand { get; set; } = "N/A";
    public string BuyUnitLatest { get; set; } = "N/A";
    public string BuyUnitEstimate { get; set; } = "N/A";

    public string WHTInHand { get; set; } = "N/A";
    public string WHTLatest { get; set; } = "N/A";
    public string WHTEstimate { get; set; } = "N/A";


    public string AmountInHand { get; set; } = "N/A";
    public string AmountLatest { get; set; } = "N/A";
    public string AmountEstimate { get; set; } = "N/A";

    public string ExchangeRateInHand { get; set; } = "N/A";
    public string ExchangeRateLatest { get; set; } = "N/A";
    public string ExchangeRateEstimate { get; set; } = "N/A";

    public string AmountThaiBahtInHand { get; set; } = "N/A";
    public string AmountThaiBahtLatest { get; set; } = "N/A";
    public string AmountThaiBahtEstimate { get; set; } = "N/A";

    public string IncotermInHand { get; set; } = "N/A";
    public string IncotermLatest { get; set; } = "N/A";
    public string IncotermEstimate { get; set; } = "N/A";

    public string AdministrativeCostsInHand { get; set; } = "N/A";
    public string AdministrativeCostsLatest { get; set; } = "N/A";
    public string AdministrativeCostsEstimate { get; set; } = "N/A";

    public string CostsInHand { get; set; } = "N/A";
    public string CostsLatest { get; set; } = "N/A";
    public string CostsEstimate { get; set; } = "N/A";

    public string ProfitPerUnitInHand { get; set; } = "N/A";
    public string ProfitPerUnitLatest { get; set; } = "N/A";
    public string ProfitPerUnitEstimate { get; set; } = "N/A";

    public string LowerPriceInHand { get; set; } = "N/A";
    public string LowerPriceLatest { get; set; } = "N/A";
    public string LowerPriceEstimate { get; set; } = "N/A";

    public string OfferPriceInHand { get; set; } = "N/A";
    public string OfferPriceLatest { get; set; } = "N/A";

    public string LowerPriceProfitInHand { get; set; } = "N/A";
    public string LowerPriceProfitLatest { get; set; } = "N/A";
}