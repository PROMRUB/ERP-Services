using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Condition;
using ERP.Services.API.Models.RequestModels.Product;
using ERP.Services.API.Models.RequestModels.Project;
using ERP.Services.API.Models.ResponseModels.Condition;
using ERP.Services.API.Models.ResponseModels.Product;
using ERP.Services.API.Models.ResponseModels.Project;
using ERP.Services.API.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ERP.Services.API.Services.Condition
{
    public class ConditionService : BaseService, IConditionService
    {
        private readonly IMapper mapper;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IConditionRepository conditionRepository;

        public ConditionService(IMapper mapper,
            IOrganizationRepository organizationRepository,
            IConditionRepository conditionRepository)
        {
            this.mapper = mapper;
            this.organizationRepository = organizationRepository;
            this.conditionRepository = conditionRepository;
        }

        public async Task<List<ConditionResponse>> GetConditionListByBusiness(string orgId, Guid businessId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var result = await conditionRepository.GetConditionByBusiness((Guid)organization.OrgId, businessId).Where(x => x.ConditionStatus == RecordStatus.Active.ToString()).OrderBy(x => x.OrderBy).ToListAsync();
            return mapper.Map<List<ConditionEntity>, List<ConditionResponse>>(result);
        }

        public async Task<ConditionResponse> GetConditionInformationById(string orgId, Guid businessId, Guid conditionId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var result = await conditionRepository.GetConditionByBusiness((Guid)organization.OrgId, businessId).Where(x => x.ConditionId == conditionId).FirstOrDefaultAsync();
            return mapper.Map<ConditionEntity, ConditionResponse>(result);
        }

        public async Task CreateCondition(string orgId, ConditionRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = mapper.Map<ConditionRequest, ConditionEntity>(request);
            request.OrgId = organization.OrgId;
            conditionRepository.AddCondition(query);
            conditionRepository.Commit();
        }
        public async Task UpdateCondition(string orgId, Guid businessId, Guid conditionId, ConditionRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await conditionRepository.GetConditionByBusiness((Guid)organization.OrgId, businessId).Where(x => x.ConditionId == conditionId).FirstOrDefaultAsync();
            query.ConditionDescription = request.ConditionDescription;
            conditionRepository.UpdateCondition(query);
            conditionRepository.Commit();
        }

        public async Task DeleteCondition(string orgId, Guid businessId, Guid conditionId, ConditionRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await conditionRepository.GetConditionByBusiness((Guid)organization.OrgId, businessId).Where(x => x.ConditionId == conditionId).FirstOrDefaultAsync();
            conditionRepository.DeleteCondition(query);
            conditionRepository.Commit();
        }
    }
}
