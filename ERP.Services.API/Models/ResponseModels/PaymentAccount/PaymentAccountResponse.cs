using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Models.ResponseModels.PaymentAccount
{
    public class PaymentAccountResponse
    {
        public Guid? PaymentAccountId { get; set; }
        public Guid? OrgId { get; set; }
        public string? PaymentAccountName { get; set; }
        public string? AccountType { get; set; }
        public Guid? AccountBank { get; set; }
        public string? AccountBankName { get; set; }
        public Guid? AccountBrn { get; set; }
        public string? AccountBankBrn { get; set; }
        public string? PaymentAccountNo { get; set; }
        public string? AccountStatus { get; set; }
    }
}
