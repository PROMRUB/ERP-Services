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