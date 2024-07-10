using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Product;
using ERP.Services.API.Models.RequestModels.Project;
using ERP.Services.API.Models.ResponseModels.Product;
using ERP.Services.API.Models.ResponseModels.Project;
using ERP.Services.API.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ERP.Services.API.Services.Project
{
    public class ProjectService : BaseService, IProjectService
    {
        private readonly IMapper mapper;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IProjectRepository projectRepository;

        public ProjectService(IMapper mapper,
            IOrganizationRepository organizationRepository,
            IProjectRepository projectRepository)
        {
            this.mapper = mapper;
            this.organizationRepository = organizationRepository;
            this.projectRepository = projectRepository;
        }

        public async Task<List<ProjectResponse>> GetProjectListByBusiness(string orgId, Guid businessId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var result = await projectRepository.GetProjectByBusiness((Guid)organization.OrgId, businessId).Where(x => x.ProjectStatus == RecordStatus.Active.ToString()).OrderBy(x => x.ProjectCustomId).ToListAsync();
            return mapper.Map<List<ProjectEntity>, List<ProjectResponse>>(result);
        }

        public async Task<ProjectResponse> GetProjectInformationById(string orgId, Guid businessId, Guid projectId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var result = await projectRepository.GetProjectByBusiness((Guid)organization.OrgId, businessId).Where(x => x.ProjectId == projectId).FirstOrDefaultAsync();
            return mapper.Map<ProjectEntity, ProjectResponse>(result);
        }

        public async Task CreateProject(string orgId, ProjectRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = mapper.Map<ProjectRequest, ProjectEntity>(request);
            var year = DateTime.Now.Year.ToString();
            var runNo = await projectRepository.ProjectNumberAsync((Guid)organization.OrgId, (Guid)request.BusinessId, year, 0);
            request.ProjectCustomId = $"PJ-{year}-{runNo.Allocated.Value.ToString("D5")}" ;
            request.OrgId = organization.OrgId;
            projectRepository.AddProject(query);
            projectRepository.Commit();
        }
        public async Task UpdateProject(string orgId, Guid businessId, Guid projectId, ProjectRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await projectRepository.GetProjectByBusiness((Guid)organization.OrgId, businessId).Where(x => x.ProjectId == projectId).FirstOrDefaultAsync();
            query.ProjectName = request.ProjectName;
            projectRepository.UpdateProject(query);
            projectRepository.Commit();
        }

        public async Task DeleteProject(string orgId, Guid businessId, Guid projectId, ProjectRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await projectRepository.GetProjectByBusiness((Guid)organization.OrgId, businessId).Where(x => x.ProjectId == projectId).FirstOrDefaultAsync();
            projectRepository.DeleteProject(query);
            projectRepository.Commit();
        }
    }
}
