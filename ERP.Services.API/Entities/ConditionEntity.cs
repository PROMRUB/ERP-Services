using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Entities
{
    [Table("Conditions")]
    [Index(nameof(ConditionId), nameof(OrgId), nameof(BusinessId), IsUnique = true)]
    public class ConditionEntity
    {

        [Key]
        [Column("condition_id")]
        public Guid? ConditionId { get; set; }

        [Column("org_id")]
        public Guid? OrgId { get; set; }

        [Column("business_id")]
        public Guid? BusinessId { get; set; }

        [Column("condition_description")]
        public string? ConditionDescription { get; set; }

        [Column("order_by")]
        public int? OrderBy { get; set; }

        [Column("condition_status")]
        public string? ConditionStatus { get; set; }
    }
}
