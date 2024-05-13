using ERP.Services.API.Models.RequestModels.ApiKey;
using ERP.Services.API.Models.ResponseModels.ApiKey;

namespace ERP.Services.API.Interfaces
{
    public interface IApiKeyService
    {
        public List<ApiKeyResponse> GetApiKeys(string orgId);
        public Task<ApiKeyResponse> GetApiKey(string orgId, string apiKey);
        public Task<ApiKeyResponse> VerifyApiKey(string orgId, string apiKey);
        public void AddApiKey(string orgId, ApiKeyRequest apiKey);
        public Task<bool> Update(string orgId, ApiKeyRequest apiKey, Guid key);
        public void DeleteApiKeyById(string orgId, string keyId);
    }
}
