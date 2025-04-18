﻿using ERP.Services.API.Entities;

namespace ERP.Services.API.Interfaces
{
    public interface IApiKeyRepository
    {
        public void SetCustomOrgId(string customOrgId);
        public Task<ApiKeyEntity> GetApiKey(string apiKey);
        public Task<ApiKeyEntity> GetApiKey(Guid apiKey);
        public void AddApiKey(ApiKeyEntity apiKey);
        public void DeleteApiKeyById(string keyId);
        public IEnumerable<ApiKeyEntity> GetApiKeys();
        public void Commit();
    }
}
