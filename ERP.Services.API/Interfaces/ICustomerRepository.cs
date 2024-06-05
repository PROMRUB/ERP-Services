using ERP.Services.API.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace ERP.Services.API.Interfaces
{
    public interface ICustomerRepository
    {

        public IQueryable<CustomerEntity> GetCustomerByBusiness(Guid orgId, Guid bussinessId);
        public void CreateCustomer(CustomerEntity query);
        public void UpdateCustomer(CustomerEntity query);
        public void DeleteCustomer(CustomerEntity query);
        public Task<CustomerNumberEntity> CustomerNumberAsync(Guid orgId, Guid businessId, string alphabet, int mode);
        public IDbContextTransaction BeginTransaction();
        public void Commit();
    }
}
