using ERP.Services.API.Entities;

namespace ERP.Services.API.Interfaces
{
    public interface IBusinessRepository
    {
        public IQueryable<BusinessEntity> GetBusinesses(Guid orgId);
        public IQueryable<BusinessEntity> GetBusinessesQuery();
        public IQueryable<UserBusinessEntity> GetUserBusinessList(Guid? userId, Guid orgId);
        public IQueryable<UserBusinessEntity> GetBusinessUserList(Guid orgId, Guid businessId);
        public void SetCustomOrgId(string customOrgId);
        public bool IsCustomBusinessIdExist(string orgCustomId);
        public Task AddBusiness(BusinessEntity bus);
    }
}