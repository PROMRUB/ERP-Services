using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.Services.API.Entities
{
    [Table("OrganizationNo")]
    public class OrganizationNumberEntity
    {
        [Key]
        [Column("org_id")]
        public Guid? OrgId { get; set; }

        [Column("org_date")]
        public string? OrgDate { get; set; }

        [Column("allocated")]
        public int? Allocated { get; set; }
    }
}
