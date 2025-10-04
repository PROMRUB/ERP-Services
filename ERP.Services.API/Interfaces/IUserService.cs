using ERP.Services.API.Entities;
using ERP.Services.API.Models.RequestModels.Authorization;
using ERP.Services.API.Models.RequestModels.User;
using ERP.Services.API.Models.ResponseModels.Organization;
using ERP.Services.API.Models.ResponseModels.User;

namespace ERP.Services.API.Interfaces
{
    public interface IUserService
    {
        public Task<List<OrganizationUserResponse>> GetUsers(string orgId, Guid businessId, string role);
        public Task<UserResponse> GetUserByName(string orgId, string userName);
        public void AddUser(string orgId, UserRequest user);
        public Task<List<UserToBusinessResponse>> GetUserToBusinessAllAsync(string orgCustomId, Guid userId);
        public Task AddUserToBusinessAsync(string orgId, AddUserToBusinessRequest request);
        public Task RemoveUserToBusinessAsync(string orgId, AddUserToBusinessRequest request);
        public Task AddRoleToUser(string orgId, Guid userId, Guid businessId, AddUserToBusinessRequest request);
        public Task<IQueryable<UserBusinessEntity>> GetUserBusiness(string orgId);
        public bool IsEmailExist(string orgId, string email);
        public bool IsUserNameExist(string orgId, string userName);
        public bool IsUserIdExist(string orgId, string userId);
        public Task<AuthorizationResponse> SignIn(string orgId, SignInRequest user);
        public Task<OrganizationUserResponse> GetUserProfile();
        public Task<OrganizationUserResponse> GetRole(string orgId, Guid businessId);
        public Task RunningUser(string id, Guid businessId);
        public Task ChangeAllPassword();
    }
}