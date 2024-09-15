using ERP.Services.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Services.API.Configurations;

public class QuotationProductEntityConfiguration : IEntityTypeConfiguration<QuotationProductEntity>
{
    public void Configure(EntityTypeBuilder<QuotationProductEntity> builder)
    {
        builder.HasKey(b => b.QuotationProductId);
        builder.HasOne(x => x.Quotation)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.QuotationId);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId);
    }
}