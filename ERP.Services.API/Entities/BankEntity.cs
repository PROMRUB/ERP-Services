using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Entities
{
    [Table("Bank")]
    public class BankEntity
    {
        [Key]
        [Column("bank_id")]
        public Guid? BankId { get; set; }

        [Column("bank_code")]
        public string? BankCode { get; set; }

        [Column("bank_abbr")]
        public string? BankAbbr { get; set; }

        [Column("bank_name_th")]
        public string? BankTHName { get; set; }

        [Column("bank_name_en")]
        public string? BankENName { get; set; }
    }
}
