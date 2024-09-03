using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Models.RequestModels.Condition;
using ERP.Services.API.Models.RequestModels.Project;
using ERP.Services.API.Models.ResponseModels.Condition;
using ERP.Services.API.Models.ResponseModels.Project;
using ERP.Services.API.Repositories;
using ERP.Services.API.Utils;

namespace ERP.Services.API.Interfaces
{
    public interface IConditionService
    {
        public Task<List<ConditionResponse>> GetConditionListByBusiness(string orgId, Guid businessId);
        public Task<PagedList<ConditionResponse>> GetConditionListByBusiness(string orgId, Guid businessId,
            string keyword,int page,int pageSize);
        public Task<ConditionResponse> GetConditionInformationById(string orgId, Guid businessId, Guid conditionId);
        public Task CreateCondition(string orgId, ConditionRequest request);
        public Task UpdateCondition(string orgId, Guid businessId, Guid conditionId, ConditionRequest request);
        public Task DeleteCondition(string orgId, Guid businessId, Guid conditionId, ConditionRequest request);
        public Task ImportCondition(string orgId, Guid businessId, IFormFile request);
    }
}
