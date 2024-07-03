using ERP.Services.API.Entities;
using ERP.Services.API.Enum;

namespace ERP.Services.API.Interfaces
{
    public interface IProjectRepository
    {
        public IQueryable<ProjectEntity> GetProjectByBusiness(Guid orgId, Guid businessId);
        public void AddProject(ProjectEntity query);
        public void UpdateProject(ProjectEntity query);
        public void DeleteProject(ProjectEntity query);
        public void Commit();
    }
}
