using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Models.RequestModels.Project;
using ERP.Services.API.Models.ResponseModels.Project;
using ERP.Services.API.Repositories;
using ERP.Services.API.Utils;

namespace ERP.Services.API.Interfaces
{
    public interface IProjectService
    {
        public Task<List<ProjectResponse>> GetProjectListByBusiness(string orgId, Guid businessId);
        public Task<PagedList<ProjectResponse>> GetProjectListByBusiness(string orgId, Guid businessId,string keyword,int page,int pageSize);
        public Task<ProjectResponse> GetProjectInformationById(string orgId, Guid businessId, Guid projectId);
        public Task CreateProject(string orgId, ProjectRequest request);
        public Task UpdateProject(string orgId, Guid businessId, Guid projectId, ProjectRequest request);
        public Task DeleteProject(string orgId, Guid businessId, Guid projectId, ProjectRequest request);
    }
}
