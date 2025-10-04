using ERP.Services.API.Entities;

namespace ERP.Services.API.Interfaces
{
    public interface IUserRepository
    {
        public void SetCustomOrgId(string customOrgId);
        public IEnumerable<UserEntity> GetUsers();
        public IQueryable<OrganizationUserEntity> GetUserProfiles();
        public Task<UserEntity> GetUserByName(string userName);
        public void AddUser(UserEntity user);
        public Task<List<UserBusinessEntity>> GetUserToBusinessAllAsync(Guid orgId, Guid userId,
            CancellationToken ct = default);
        public Task AddUserToBusinessAsync(UserBusinessEntity user, CancellationToken ct = default);
        public Task RemoveUserToBusinessAsync(UserBusinessEntity user, CancellationToken ct = default);
        public void AddRoleToUser(Guid UserId, Guid BusinessId, UserBusinessEntity user);
        public bool IsEmailExist(string email);
        public bool IsUserNameExist(string userName);
        public bool IsUserIdExist(string userId);
        public IQueryable<OrganizationUserEntity> GetUserSignIn(string username, string password);
        public IQueryable<UserSessionEntity> GetUserSession();
        public Task CreateUserSession(UserSessionEntity session);
    }
}