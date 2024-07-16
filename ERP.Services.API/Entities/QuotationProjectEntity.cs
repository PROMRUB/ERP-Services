using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Entities;

[Table("QuotationProject")]
[Index(nameof(QuotationId), nameof(ProjectId), IsUnique = true)]
public class QuotationProjectEntity
{
    [Key] [Column("quotation_id")] public Guid QuotationId { get; set; }
    [Column("project_id")] public Guid ProjectId { get; set; }

    public ProjectEntity Project { get; set; }

    [Column("lead_time")] public int LeadTime { get; set; }

    [Column("warranty")] public int Warranty { get; set; }

    [Column("payment_condition")] public string PaymentCondition { get; set; }

    [Column("po")] public string Po { get; set; }
    [Column("order")] public int Order { get; set; }
}