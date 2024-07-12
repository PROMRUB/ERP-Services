using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Entities
{
    [Table("BankBranch")]
    public class BankBranchEntity
    {
        [Key]
        [Column("bank_branch_id")]
        public Guid BankBranchId { get; set; }

        [Column("bank_code")]
        public string? BankCode { get; set; }

        [Column("bank_branch_code")]
        public string? BankBranchCode { get; set; }

        [Column("bank_branch_name_th")]
        public string? BankBranchTHName { get; set; }

        [Column("bank_branch_name_en")]
        public string? BankBranchENName { get; set; }
    }
}
