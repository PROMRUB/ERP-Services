using ERP.Services.API.Entities;

namespace ERP.Services.API.Interfaces
{
    public interface ISystemConfigRepository
    {
        IQueryable<T> GetAll<T>() where T : class;
        Task<T> GetByIdAsync<T>(Guid id) where T : class;
        Task AddAsync<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Commit();
    }
}
