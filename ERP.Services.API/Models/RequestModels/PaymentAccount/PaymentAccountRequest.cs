using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.Services.API.Models.RequestModels.PaymentAccount
{
    public class PaymentAccountRequest
    {
        public Guid? PaymentAccountId { get; set; }
        public Guid? OrgId { get; set; }
        public Guid? BusinessId { get; set; }
        public string? PaymentAccountName { get; set; }
        public string? AccountType { get; set; }
        public Guid? AccountBank { get; set; }
        public Guid? AccountBrn { get; set; }
        public string? PaymentAccountNo { get; set; }
        public string? AccountStatus { get; set; }
    }
}
