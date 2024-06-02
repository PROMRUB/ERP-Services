using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.Services.API.Entities
{
    [Table("CustomerNo")]
    public class CustomerNumberEntity
    {
        [Key]
        [Column("cus_no_id")]
        public Guid? CusNoId { get; set; }

        [Column("org_id")]
        public Guid? OrgId { get; set; }

        [Column("business_id")]
        public Guid? BusinessId { get; set; }

        [Column("character")]
        public string? Character { get; set; }

        [Column("allocated")]
        public int? Allocated { get; set; }
    }
}
