using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.PromServiceDbContext;

namespace ERP.Services.API.Repositories
{
    public class ProjectRepository : BaseRepository, IProjectRepository
    {
        public ProjectRepository(PromDbContext context) {
            this.context = context;
        }

        public IQueryable<ProjectEntity> GetProjectByBusiness(Guid orgId, Guid businessId)
        {
            return context.Projects.Where(x => x.OrgId == orgId && x.BusinessId == businessId);
        }

        public void AddProject(ProjectEntity query)
        {
            try
            {
                context.Projects.Add(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateProject(ProjectEntity query)
        {
            try
            {
                context.Projects.Update(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteProject(ProjectEntity query)
        {
            try
            {
                query.ProjectStatus = RecordStatus.InActive.ToString();
                context.Projects.Update(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
