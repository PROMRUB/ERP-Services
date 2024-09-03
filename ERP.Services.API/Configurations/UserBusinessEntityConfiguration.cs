// using ERP.Services.API.Entities;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
//
// namespace ERP.Services.API.Configurations;
//
// public class UserBusinessEntityConfiguration : IEntityTypeConfiguration<UserBusinessEntity>
// {
//     public void Configure(EntityTypeBuilder<UserBusinessEntity> builder)
//     {
//         builder.HasOne(x => x.User)
//             .WithOne()
//             .HasForeignKey<UserBusinessEntity>(x => x.UserId);
//     }
// }