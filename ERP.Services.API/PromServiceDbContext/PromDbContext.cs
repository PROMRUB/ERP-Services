using ERP.Services.API.Configurations;
using ERP.Services.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.PromServiceDbContext
{
    public class PromDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public PromDbContext(IConfiguration configuration, DbContextOptions<PromDbContext> options) : base(options)
        {
            Configuration = configuration;
        }

        public DbSet<BankEntity> Banks { get; set; }
        public DbSet<BankBranchEntity> BankBranches { get; set; }
        public DbSet<ProvinceEntity>? Provinces { get; set; }
        public DbSet<DistrictEntity>? District { get; set; }
        public DbSet<SubDistrictEntity>? SubDistrict { get; set; }
        public DbSet<OrganizationEntity>? Organizations { get; set; }
        public DbSet<OrganizationNumberEntity>? OrganizationNumbers { get; set; }
        public DbSet<BusinessEntity>? Businesses { get; set; }
        public DbSet<CustomerEntity>? Customers { get; set; }
        public DbSet<CustomerNumberEntity> CustomerNumbers { get; set; }
        public DbSet<CustomerContactEntity> CustomerContacts { get; set; }
        public DbSet<ApiKeyEntity>? ApiKeys { get; set; }
        public DbSet<RoleEntity>? Roles { get; set; }
        public DbSet<UserEntity>? Users { get; set; }
        public DbSet<OrganizationUserEntity>? OrganizationUsers { get; set; }
        public DbSet<UserBusinessEntity> UserBusinesses { get; set; }
        public DbSet<UserSessionEntity>? UserSessions { get; set; }
        public DbSet<ProductCategoryEntity>? ProductCategories { get; set; }
        public DbSet<ProductEntity>? Products { get; set; }
        public DbSet<ProjectEntity>? Projects { get; set; }
        public DbSet<ProjectNumberEntity> ProjectNumbers { get; set; }
        public DbSet<ConditionEntity> Conditions { get; set; }
        public DbSet<PaymentAccountEntity> PaymentAccounts { get; set; }
        public DbSet<QuotationEntity> Quotation { get; set; }
        public DbSet<QuotationProductEntity> QuotationProduct { get; set; }
        public DbSet<QuotationProjectEntity> QuotationProject { get; set; }
        // public DbSet<ProjectLowerPriceProductEntity> ProjectLowerPriceProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankEntity>();
            modelBuilder.Entity<BankBranchEntity>();
            modelBuilder.Entity<ProvinceEntity>();
            modelBuilder.Entity<DistrictEntity>();
            modelBuilder.Entity<SubDistrictEntity>();
            modelBuilder.Entity<OrganizationEntity>();
            modelBuilder.Entity<OrganizationNumberEntity>();
            modelBuilder.Entity<BusinessEntity>();
            modelBuilder.Entity<CustomerEntity>();
            modelBuilder.Entity<CustomerNumberEntity>();
            modelBuilder.Entity<CustomerContactEntity>();
            modelBuilder.Entity<ApiKeyEntity>();
            modelBuilder.Entity<RoleEntity>();
            modelBuilder.Entity<UserEntity>();
            modelBuilder.Entity<OrganizationUserEntity>();
            modelBuilder.Entity<UserBusinessEntity>();
            modelBuilder.Entity<UserSessionEntity>();
            modelBuilder.Entity<ProductCategoryEntity>();
            // modelBuilder.Entity<ProductEntity>();
            modelBuilder.Entity<ProjectEntity>();
            modelBuilder.Entity<ProjectNumberEntity>();
            modelBuilder.Entity<ConditionEntity>();
            modelBuilder.Entity<PaymentAccountEntity>();


            modelBuilder.ApplyConfiguration(new QuotationEntityConfiguration());
            modelBuilder.ApplyConfiguration(new QuotationProductEntityConfiguration());
            modelBuilder.ApplyConfiguration(new QuotationProjectEntityConfiguration());
            // modelBuilder.ApplyConfiguration(new ProjectLowerPriceProductEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentAccountEntityConfiguration());
        }
    }
}