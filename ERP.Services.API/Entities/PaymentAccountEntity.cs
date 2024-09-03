using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Entities
{
    [Table("PaymentAccounts")]
    [Index(nameof(PaymentAccountId), nameof(OrgId), nameof(BusinessId), IsUnique = true)]
    public class PaymentAccountEntity
    {
        [Key]
        [Column("payment_account_id")]
        public Guid? PaymentAccountId { get; set; }

        [Column("org_id")]
        public Guid? OrgId { get; set; }

        [Column("business_id")]
        public Guid? BusinessId { get; set; }

        [Column("payment_account_name")]
        public string? PaymentAccountName { get; set; }

        [Column("account_type")]
        public string? AccountType { get; set; }

        [Column("account_bank_id")]
        public Guid? BankId { get; set; }
        public BankEntity BankEntity { get; set; }

        [Column("account_brn_id")]
        public Guid? BankBranchId { get; set; }
        public BankBranchEntity BankBranchEntity { get; set; }


        [Column("account_no")]
        public string? PaymentAccountNo { get; set; }

        [Column("account_status")]
        public string? AccountStatus { get; set; }
    }
}
