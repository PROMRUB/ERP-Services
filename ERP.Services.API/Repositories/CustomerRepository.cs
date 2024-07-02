using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.PromServiceDbContext;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Repositories
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public CustomerRepository(PromDbContext context)
        {
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
        public IQueryable<CustomerContactEntity> GetCustomerContactByCustomer(Guid orgId, Guid customerId)
        {
            var query = context.CustomerContacts!.Where(x => x.OrgId == orgId && x.CusId == customerId);
            return query;
        }
        public void CreateCustomerContact(CustomerContactEntity query)
        {
            context.CustomerContacts!.Add(query);
        }

        public void UpdateCustomerContact(CustomerContactEntity query)
        {
            context.CustomerContacts!.Update(query);
        }

        public void DeleteCustomerContact(CustomerContactEntity query)
        {
            query.CusConStatus = RecordStatus.InActive.ToString();
            context.CustomerContacts!.Update(query);
        }
        public async Task<CustomerNumberEntity> CustomerNumberAsync(Guid orgId, Guid businessId, string alphabet, int mode)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var query = await context.CustomerNumbers!
                        .Where(x => x.OrgId == orgId && x.BusinessId == businessId && x.Character == alphabet)
                        .FirstOrDefaultAsync();

                    bool haveCustomerNo = true;
                    while (haveCustomerNo)
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
                            await context.SaveChangesAsync();

                            query = await context.CustomerNumbers!
                                .Where(x => x.OrgId == orgId && x.BusinessId == businessId && x.Character == alphabet)
                                .FirstOrDefaultAsync();
                        }
                        else
                        {
                            haveCustomerNo = false;
                        }
                    }

                    query!.Allocated++;
                    await context.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    return query;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction in case of an error
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
