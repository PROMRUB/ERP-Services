using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.PromServiceDbContext;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Repositories
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public CustomerRepository(PromDbContext context) {
            this.context = context;
        }

        public IQueryable<CustomerEntity> GetCustomerByBusiness(Guid orgId, Guid bussinessId)
        {
            var query = context.Customers!.Where(x => x.OrgId == orgId && x.BusinessId == bussinessId);
            return query;
        }

        public void CreateCustomer(CustomerEntity query)
        {
            context.Customers!.Add(query);
        }

        public void UpdateCustomer(CustomerEntity query)
        {
            context.Customers!.Update(query);
        }

        public void DeleteCustomer(CustomerEntity query)
        {
            query.CusStatus = RecordStatus.InActive.ToString();
            context.Customers!.Update(query);
        }
        public async Task<CustomerNumberEntity> CustomerNumberAsync(Guid orgId, Guid businessId, string alphabet)
        {
            try
            {
                var query = await context!.CustomerNumbers!.Where(x => x.OrgId == orgId && x.BusinessId == businessId && x.Character == alphabet).FirstOrDefaultAsync();
                bool HaveCustomerNo = true;
                while (HaveCustomerNo)
                {
                    if (query == null)
                    {
                        var newRec = new CustomerNumberEntity
                        {
                            CusNoId = Guid.NewGuid(),
                            OrgId = orgId,
                            BusinessId = businessId,
                            Character = alphabet,
                            Allocated = 0
                        };
                        context.CustomerNumbers!.Add(newRec);
                        context.SaveChanges();
                        query = await context.CustomerNumbers!.Where(x => x.OrgId == orgId && x.BusinessId == businessId && x.Character == alphabet).FirstOrDefaultAsync();
                    }
                    else
                    {
                        HaveCustomerNo = false;
                    }
                }
                query!.Allocated++;
                context.SaveChanges();
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
