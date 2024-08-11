using ERP.Services.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Services.API.Configurations;

public class QuotationEntityConfiguration : IEntityTypeConfiguration<QuotationEntity>
{
    public void Configure(EntityTypeBuilder<QuotationEntity> builder)
    {
        builder.HasKey(b => b.QuotationId);
        builder.HasOne(b => b.IssuedByUser)
            .WithMany()
            .IsRequired(false)
            .HasForeignKey(x => x.IssuedById);
        builder.HasOne(b => b.SalePerson)
            .WithMany()
            .IsRequired(false)
            .HasForeignKey(x => x.SalePersonId);
        builder.HasOne(b => b.PaymentAccountEntity)
            .WithMany()
            .IsRequired(false)
            .HasForeignKey(x => x.PaymentId);
        
    }
}

public class QuotationProductEntityConfiguration : IEntityTypeConfiguration<QuotationProductEntity>
{
    public void Configure(EntityTypeBuilder<QuotationProductEntity> builder)
    {
        builder.HasKey(b => new { b.QuotationId, b.ProductId });
        builder.HasOne(x => x.Quotation)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.QuotationId);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId);
    }
}

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