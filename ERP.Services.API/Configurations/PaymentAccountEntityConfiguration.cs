using ERP.Services.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Services.API.Configurations;

public class PaymentAccountEntityConfiguration : IEntityTypeConfiguration<PaymentAccountEntity>
{
    public void Configure(EntityTypeBuilder<PaymentAccountEntity> builder)
    {
        builder.HasOne(x => x.BankEntity)
            .WithMany()
            .HasForeignKey(x => x.BankId);

        builder.HasOne(x => x.BankBranchEntity)
            .WithMany()
            .HasForeignKey(x => x.BankBranchId);
    }
}