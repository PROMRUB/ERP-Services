using ERP.Services.API.PromServiceDbContext;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace ERP.Services.API.Repositories
{
    public class BaseRepository
    {
        private const string RESERVE_ORG_ID = "axxxxnotdefinedxxxxxxa";

        protected PromDbContext? context;
        protected string orgId = RESERVE_ORG_ID;

        public void SetCustomOrgId(string customOrgId)
        {
            orgId = customOrgId;
        }

        public IDbContextTransaction BeginTransaction()
        {
            try
            {
                return context!.Database.BeginTransaction();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Commit()
        {
            try
            {
                context!.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
