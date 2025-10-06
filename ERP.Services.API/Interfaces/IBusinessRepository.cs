using ERP.Services.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Interfaces
{
    public interface IBusinessRepository
    {
        public IQueryable<BusinessEntity> GetBusinesses(Guid orgId);
        public IQueryable<BusinessEntity> GetBusinessesQuery();
        public IQueryable<UserBusinessEntity> GetUserBusinessQuery();
        public IQueryable<UserBusinessEntity> GetUserBusinessList(Guid? userId, Guid orgId);
        public IQueryable<UserBusinessEntity> GetBusinessUserList();
        public void SetCustomOrgId(string customOrgId);
        public bool IsCustomBusinessIdExist(string orgCustomId);
        public Task AddBusiness(BusinessEntity bus);
        public Task UpdateBusiness(BusinessEntity bus);
        public void UpdateUserBusiness(List<UserBusinessEntity> bus);
        public DbContext Context();
    }
}