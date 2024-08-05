using ERP.Services.API.Models.Authentications;

namespace ERP.Services.API.Interfaces
{
    public interface IAuthenticationRepo
    {
        public Task<Models.Authentications.User>? Authenticate(string orgId, string user, string password, HttpRequest request);
    }
}
