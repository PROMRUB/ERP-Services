using ERP.Services.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Interfaces
{
    public interface IOrganizationRepository
    {
        public void SetCustomOrgId(string customOrgId);
        public Task<OrganizationEntity> GetOrganization();
        public IQueryable<OrganizationEntity> GetOrganizationList();
        public void AddUserToOrganization(OrganizationUserEntity user);
        public void UpdateUserToOrganization(OrganizationUserEntity user);
        public IQueryable<OrganizationUserEntity> GetUserListAsync();
        public DbContext Context();
        public void UpdateUserRange(List<OrganizationUserEntity> userList);
        public bool IsUserNameExist(string userName);
        public bool IsCustomOrgIdExist(string orgCustomId);
        public Task<OrganizationUserEntity> GetUserInOrganization(string userName);
        public Task AddOrganization(OrganizationEntity org);
        public Task<IEnumerable<OrganizationUserEntity>> GetUserAllowedOrganizationAsync(string userName);
        public Task<OrganizationNumberEntity> OrganizationNumberAsync();
        public void Commit();
    }
}
