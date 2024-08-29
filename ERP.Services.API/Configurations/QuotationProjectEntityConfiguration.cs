using ERP.Services.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Services.API.Configurations;

public class QuotationProjectEntityConfiguration : IEntityTypeConfiguration<QuotationProjectEntity>
{
    public void Configure(EntityTypeBuilder<QuotationProjectEntity> builder)
    {
        builder.HasKey(b => new { b.QuotationId, b.ProjectId });
        builder.HasOne(x => x.Quotation)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.QuotationId);

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId);
        
        builder.HasOne(b => b.PaymentCondition)
            .WithMany()
            .IsRequired(false)
            .HasForeignKey(x => x.PaymentConditionId);
    }
}