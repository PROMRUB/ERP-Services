﻿using ERP.Services.API.Entities;
using ERP.Services.API.Interfaces;
using ERP.Services.API.PromServiceDbContext;
using Microsoft.EntityFrameworkCore;
using PasswordGenerator;

namespace ERP.Services.API.Repositories
{
    public class ApiKeyRepository : BaseRepository, IApiKeyRepository
    {
        private readonly Password password = new Password(includeLowercase: true,
                       includeUppercase: true,
                       includeNumeric: true,
                       includeSpecial: false,
                       passwordLength: 32);
        public ApiKeyRepository(PromDbContext context)
        {
            this.context = context;
        }

        public Task<ApiKeyEntity> GetApiKey(string apiKey)
        {
            var result = context!.ApiKeys!
                .Where(x => x.OrgId!.Equals(orgId) && x.ApiKey!.Equals(apiKey)).FirstOrDefaultAsync();
            return result!;
        }

        public Task<ApiKeyEntity> GetApiKey(Guid apiKey)
        {
            var result = context!.ApiKeys!
                .Where(x => x.OrgId!.Equals(orgId) && x.KeyId!.Equals(apiKey)).FirstOrDefaultAsync();
            return result!;
        }

        public void AddApiKey(ApiKeyEntity apiKey)
        {
            apiKey.OrgId = orgId;
            apiKey.ApiKey = password.Next();
            context!.ApiKeys!.Add(apiKey);
            context.SaveChanges();
        }

        public void DeleteApiKeyById(string keyId)
        {
            Guid id = Guid.Parse(keyId);

            var query = context!.ApiKeys!
                .Where(x => x.OrgId!.Equals(orgId) && x.KeyId.Equals(id)).FirstOrDefault();
            if (query != null)
            {
                context!.ApiKeys!.Remove(query);
                context.SaveChanges();
            }
        }

        public IEnumerable<ApiKeyEntity> GetApiKeys()
        {
            var query = context!.ApiKeys!
                .Where(x => x.OrgId!.Equals(orgId));
            return query;
        }
    }
}
