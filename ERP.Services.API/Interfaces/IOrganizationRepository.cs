using ERP.Services.API.Entities;

namespace ERP.Services.API.Interfaces
{
    public interface IOrganizationRepository
    {
        public void SetCustomOrgId(string customOrgId);
        public Task<OrganizationEntity> GetOrganization();
        public void AddUserToOrganization(OrganizationUserEntity user);
        public bool IsUserNameExist(string userName);
        public bool IsCustomOrgIdExist(string orgCustomId);
        public Task<OrganizationUserEntity> GetUserInOrganization(string userName);
        public Task AddOrganization(OrganizationEntity org);
        public Task<IEnumerable<OrganizationUserEntity>> GetUserAllowedOrganizationAsync(string userName);
        public Task<OrganizationNumberEntity> OrganizationNumberAsync();
        public void Commit();
    }
}
