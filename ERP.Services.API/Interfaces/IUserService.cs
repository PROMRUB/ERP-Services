using ERP.Services.API.Entities;
using ERP.Services.API.Models.RequestModels.Authorization;
using ERP.Services.API.Models.RequestModels.User;
using ERP.Services.API.Models.ResponseModels.Organization;
using ERP.Services.API.Models.ResponseModels.User;

namespace ERP.Services.API.Interfaces
{
    public interface IUserService
    {
        public List<UserResponse> GetUsers(string orgId);
        public Task<UserResponse> GetUserByName(string orgId, string userName);
        public IQueryable<UserBusinessEntity> GetUserBusiness();
        public void AddUser(string orgId, UserRequest user);
        public Task AddUserToBusinessAsync(string orgId, AddUserToBusinessRequest request);
        public bool IsEmailExist(string orgId, string email);
        public bool IsUserNameExist(string orgId, string userName);
        public bool IsUserIdExist(string orgId, string userId);
        public Task<AuthorizationResponse> SignIn(string orgId, SignInRequest user);
        public Task<OrganizationUserResponse> GetUserProfile();
    }
}
