// using ERP.Services.API.Entities;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
//
// namespace ERP.Services.API.Configurations;
//
// public class ProjectLowerPriceProductEntityConfiguration : IEntityTypeConfiguration<ProjectLowerPriceProductEntity>
// {
//     public void Configure(EntityTypeBuilder<ProjectLowerPriceProductEntity> builder)
//     {
//         builder.HasKey(x =>
//         new {
//             x.ProductId,x.ProjectId,x.CustomerId
//         });
//
//         builder.HasOne(x => x.Project)
//             .WithMany()
//             .HasForeignKey(x => x.ProjectId);
//
//         builder.HasOne(x => x.Product)
//             .WithMany()
//             .HasForeignKey(x => x.ProductId);
//         
//         builder.HasOne(x => x.Customer)
//             .WithMany()
//             .HasForeignKey(x => x.CustomerId);
//     }
// }