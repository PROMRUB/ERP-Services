using ERP.Services.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Services.API.Configurations;

public class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        // builder.HasOne(b => b.CategoryEntity)
        //     .WithMany()
        //     .IsRequired(false)
        //     .HasForeignKey(x => x.ProductCatId);
        // builder.HasOne(b => b.SubCategoryEntity)
        //     .WithMany()
        //     .IsRequired(false)
        //     .HasForeignKey(x => x.ProductSubCatId);
    }
}