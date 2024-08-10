using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.PromServiceDbContext;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ProjectNumberEntity> ProjectNumberAsync(Guid orgId, Guid businessId, string year, int mode)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var query = await context.ProjectNumbers!
                        .Where(x => x.OrgId == orgId && x.BusinessId == businessId && x.Year == year)
                        .FirstOrDefaultAsync();

                    bool haveProjectNo = true;
                    while (haveProjectNo)
                    {
                        if (query == null)
                        {
                            var newRec = new ProjectNumberEntity
                            {
                                CusNoId = Guid.NewGuid(),
                                OrgId = orgId,
                                BusinessId = businessId,
                                Year = year,
                                Allocated = 0
                            };
                            context.ProjectNumbers!.Add(newRec);
                            await context.SaveChangesAsync();

                            query = await context.ProjectNumbers!
                                .Where(x => x.OrgId == orgId && x.BusinessId == businessId && x.Year == year)
                                .FirstOrDefaultAsync();
                        }
                        else
                        {
                            haveProjectNo = false;
                        }
                    }

                    query!.Allocated++;
                    await context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return query;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
