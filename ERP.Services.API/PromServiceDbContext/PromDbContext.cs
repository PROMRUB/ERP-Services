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

        public DbSet<ProvinceEntity>? Provinces { get; set; }
        public DbSet<DistrictEntity>? District { get; set; }
        public DbSet<SubDistrictEntity>? SubDistrict { get; set; }
        public DbSet<OrganizationEntity>? Organizations { get; set; }
        public DbSet<OrganizationNumberEntity>? OrganizationNumbers { get; set; }
        public DbSet<BusinessEntity>? Businesses { get; set; }
        public DbSet<CustomerEntity>? Customers { get; set; }
        public DbSet<ApiKeyEntity>? ApiKeys { get; set; }
        public DbSet<RoleEntity>? Roles { get; set; }
        public DbSet<UserEntity>? Users { get; set; }
        public DbSet<OrganizationUserEntity>? OrganizationUsers { get; set; }
        public DbSet<UserBusinessEntity> UserBusinesses { get; set; }
        public DbSet<UserSessionEntity>? UserSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProvinceEntity>();
            modelBuilder.Entity<DistrictEntity>();
            modelBuilder.Entity<SubDistrictEntity>();
            modelBuilder.Entity<OrganizationEntity>();
            modelBuilder.Entity<OrganizationNumberEntity>();
            modelBuilder.Entity<BusinessEntity>();
            modelBuilder.Entity<CustomerEntity>();
            modelBuilder.Entity<ApiKeyEntity>();
            modelBuilder.Entity<RoleEntity>();
            modelBuilder.Entity<UserEntity>();
            modelBuilder.Entity<OrganizationUserEntity>();
            modelBuilder.Entity<UserBusinessEntity>();
            modelBuilder.Entity<UserSessionEntity>();
        }
    }
}
