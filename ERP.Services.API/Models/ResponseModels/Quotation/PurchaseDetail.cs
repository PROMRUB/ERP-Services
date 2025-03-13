namespace ERP.Services.API.Models.ResponseModels.Quotation;

public class PurchaseDetail
{
    public string ServiceType { get; set; } = "N/A";
    public string BuyType { get; set; } = "N/A";
    public string BuyProductCode { get; set; } = "N/A";
    public string SellProductCode { get; set; } = "N/A";
    public string BuyCurrency { get; set; } = "N/A";
    public string SellCurrency { get; set; } = "N/A";
    public int BuyQuantity { get; set; }
    public int SellQuantity { get; set; }

    public string CurrencyInHand { get; set; } = "";
    public string CurrencyLatest { get; set; } = "N/A";
    public string CurrencyEstimate { get; set; } = "";

    public string BuyUnitInHand { get; set; } = "";
    public string BuyUnitLatest { get; set; } = "N/A";
    public string BuyUnitEstimate { get; set; } = "N/A";

    public string WHTInHand { get; set; } = "";
    public string WHTLatest { get; set; } = "N/A";
    public string WHTEstimate { get; set; } = "N/A";


    public string AmountInHand { get; set; } = "";
    public string AmountLatest { get; set; } = "N/A";
    public string AmountEstimate { get; set; } = "N/A";

    public string ExchangeRateInHand { get; set; } = "";
    public string ExchangeRateLatest { get; set; } = "N/A";
    public string ExchangeRateEstimate { get; set; } = "N/A";

    public string AmountThaiBahtInHand { get; set; } = "";
    public string AmountThaiBahtLatest { get; set; } = "N/A";
    public string AmountThaiBahtEstimate { get; set; } = "N/A";

    public string IncotermInHand { get; set; } = "";
    public string IncotermLatest { get; set; } = "N/A";
    public string IncotermEstimate { get; set; } = "N/A";

    public string AdministrativeCostsInHand { get; set; } = "";
    public string AdministrativeCostsLatest { get; set; } = "N/A";
    public string AdministrativeCostsEstimate { get; set; } = "N/A";

    public string CostsInHand { get; set; } = "";
    public string CostsLatest { get; set; } = "N/A";
    public string CostsEstimate { get; set; } = "N/A";

    public string ProfitPerUnitInHand { get; set; } = "";
    public string ProfitPerUnitLatest { get; set; } = "N/A";
    public string ProfitPerUnitEstimate { get; set; } = "N/A";

    public string LowerPriceInHand { get; set; } = "";
    public string LowerPriceLatest { get; set; } = "N/A";
    public string LowerPriceEstimate { get; set; } = "N/A";

    public string OfferPriceInHand { get; set; } = "N/A";
    public string OfferPriceLatest { get; set; } = "N/A";

    public string LowerPriceProfitInHand { get; set; } = "N/A";
    public string LowerPriceProfitLatest { get; set; } = "N/A";
    public float Amount { get; set; }
    public Guid ProductId { get; set; }
}