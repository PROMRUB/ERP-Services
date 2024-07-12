using ERP.Services.API.Entities;
using ERP.Services.API.Interfaces;
using ERP.Services.API.PromServiceDbContext;

namespace ERP.Services.API.Repositories
{
    public class SystemConfigRepository : BaseRepository, ISystemConfigRepository
    {
        public SystemConfigRepository(PromDbContext context)
        {
            this.context = context;
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return context.Set<T>();
        }

        public async Task<T> GetByIdAsync<T>(Guid id) where T : class
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task AddAsync<T>(T entity) where T : class 
        {
            await context.Set<T>().AddAsync(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            context.Set<T>().Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Set<T>().Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
