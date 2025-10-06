using ERP.Services.API.Models.RequestModels.Organization;
using ERP.Services.API.Models.ResponseModels.Organization;

namespace ERP.Services.API.Interfaces
{
    public interface IOrganizationService
    {
        public Task<OrganizationResponse> GetOrganization(string orgId);
        public Task<OrganizationResponse> GetOrganizationByTaxId(string orgId, string taxId, string brnId);
        public void AddUserToOrganization(string orgId, OrganizationUserRequest user);
        public void UpdateUserToOrganization(string orgId, OrganizationUserRequest user);
        public bool IsUserNameExist(string orgId, string userName);
        public Task<bool> VerifyUserInOrganization(string orgId, string userName);
        public Task<bool> AddOrganization(string orgId, OrganizationRequest org);
        public Task<bool> AddBusiness(string orgId, OrganizationRequest org);
        public Task<bool> UpdateBusiness(string orgId, string businessId, OrganizationRequest req);
        public Task<List<OrganizationResponse>> GetBusiness(string orgId);
        public Task<OrganizationResponse> GetBusinessById(string orgId, string businessId);
        public Task<int> RemoveBusiness(string orgId, Guid businessId, CancellationToken ct = default);
        public Task<bool> UpdateOrganization(string orgId, OrganizationRequest org);
        public Task<bool> UpdateSecurity(string orgId, OrganizationRequest org);
        public Task<List<OrganizationUserResponse>> GetUserAllowedOrganization(string userName);
        public Task<List<OrganizationUserResponse>> GetUserAllUser(string orgId);
    }
}