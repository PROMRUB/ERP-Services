using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Product;
using ERP.Services.API.Models.RequestModels.Project;
using ERP.Services.API.Models.ResponseModels.Product;
using ERP.Services.API.Models.ResponseModels.Project;
using ERP.Services.API.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using ERP.Services.API.Utils;

namespace ERP.Services.API.Services.Project
{
    public class ProjectService : BaseService, IProjectService
    {
        private readonly IMapper mapper;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IProjectRepository projectRepository;
        private readonly UserPrincipalHandler userPrincipalHandler;

        public ProjectService(IMapper mapper,
            IOrganizationRepository organizationRepository,
            IProjectRepository projectRepository,
            UserPrincipalHandler userPrincipalHandler)
        {
            this.mapper = mapper;
            this.organizationRepository = organizationRepository;
            this.projectRepository = projectRepository;
            this.userPrincipalHandler = userPrincipalHandler;
        }

        public async Task<List<ProjectResponse>> GetProjectListByBusiness(string orgId, Guid businessId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await projectRepository.GetProjectByBusiness((Guid)organization.OrgId, businessId)
                .Where(x => x.UserId == userPrincipalHandler.Id && x.ProjectStatus == RecordStatus.Active.ToString()).OrderBy(x => x.ProjectCustomId)
                .ToListAsync();
            var result = mapper.Map<List<ProjectEntity>, List<ProjectResponse>>(query);
            foreach (var item in result)
            {
                if (item.ProjectStatus.Equals(RecordStatus.Waiting.ToString()))
                    item.ProjectStatus = "ปกติ";
                else if (item.ProjectStatus.Equals(RecordStatus.Active.ToString()))
                    item.ProjectStatus = "ปกติ";
            }
            return result;
        }
        
        public async Task<PagedList<ProjectResponse>> GetProjectListByBusiness(string orgId, Guid businessId,string keyword,int page,int pageSize)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query =  projectRepository.GetProjectByBusiness((Guid)organization.OrgId, businessId)
                .Where(x => x.UserId == userPrincipalHandler.Id && x.ProjectStatus == RecordStatus.Active.ToString()
                                                                && (string.IsNullOrEmpty(keyword) ||
                                                                    x.ProjectName.Contains(keyword)))
                .OrderBy(x => x.ProjectCustomId);
                
            // var result = mapper.Map<List<ProjectEntity>, List<ProjectResponse>>(query);
            // foreach (var item in result)
            // {
            //     if (item.ProjectStatus.Equals(RecordStatus.Waiting.ToString()))
            //         item.ProjectStatus = "ปกติ";
            //     else if (item.ProjectStatus.Equals(RecordStatus.Active.ToString()))
            //         item.ProjectStatus = "ปกติ";
            // }

            var result = query.Select(x => new ProjectResponse
            {
                ProjectId = x.ProjectId,
                OrgId = x.OrgId,
                BusinessId = x.BusinessId,
                CustomerId = x.CustomerId.Value,
                ProjectCustomId = x.ProjectCustomId,
                ProjectName = x.ProjectName,
                ProjectStatus = x.ProjectStatus.Equals(RecordStatus.Active.ToString() )
                                                        || x.ProjectStatus.Equals(RecordStatus.Waiting.ToString())
                                                           ? "ปกติ":"",
            });

            var paged = await PagedList<ProjectResponse>.Create(result, page, pageSize);
            
            return paged;
        }

        public async Task<ProjectResponse> GetProjectInformationById(string orgId, Guid businessId, Guid projectId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var result = await projectRepository.GetProjectByBusiness((Guid)organization.OrgId, businessId).Where(x => x.UserId == userPrincipalHandler.Id && x.ProjectId == projectId).FirstOrDefaultAsync();
            return mapper.Map<ProjectEntity, ProjectResponse>(result);
        }

        public async Task CreateProject(string orgId, ProjectRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = mapper.Map<ProjectRequest, ProjectEntity>(request);
            var year = DateTime.Now.Year.ToString();
            var runNo = await projectRepository.ProjectNumberAsync((Guid)organization.OrgId, (Guid)request.BusinessId, year, 0);
            query.ProjectCustomId = $"PJ-{year}-{runNo.Allocated.Value.ToString("D5")}" ;
            query.OrgId = organization.OrgId;
            query.UserId = userPrincipalHandler.Id;
            projectRepository.AddProject(query);
            projectRepository.Commit();
        }
        public async Task UpdateProject(string orgId, Guid businessId, Guid projectId, ProjectRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await projectRepository.GetProjectByBusiness((Guid)organization.OrgId, businessId).Where(x => x.UserId == userPrincipalHandler.Id && x.ProjectId == projectId).FirstOrDefaultAsync();
            query.ProjectName = request.ProjectName;
            projectRepository.UpdateProject(query);
            projectRepository.Commit();
        }

        public async Task DeleteProject(string orgId, Guid businessId, Guid projectId, ProjectRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await projectRepository.GetProjectByBusiness((Guid)organization.OrgId, businessId).Where(x => x.UserId == userPrincipalHandler.Id && x.ProjectId == projectId).FirstOrDefaultAsync();
            projectRepository.DeleteProject(query);
            projectRepository.Commit();
        }
    }
}
