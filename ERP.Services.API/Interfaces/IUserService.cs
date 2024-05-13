using ERP.Services.API.Models.RequestModels.User;
using ERP.Services.API.Models.ResponseModels.User;

namespace ERP.Services.API.Interfaces
{
    public interface IUserService
    {
        public List<UserResponse> GetUsers(string orgId);
        public Task<UserResponse> GetUserByName(string orgId, string userName);
        public void AddUser(string orgId, UserRequest user);
        public bool IsEmailExist(string orgId, string email);
        public bool IsUserNameExist(string orgId, string userName);
        public bool IsUserIdExist(string orgId, string userId);
    }
}
