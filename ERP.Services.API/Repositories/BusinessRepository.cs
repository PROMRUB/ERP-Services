﻿using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.PromServiceDbContext;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Repositories
{
    public class BusinessRepository : BaseRepository, IBusinessRepository
    {
        public BusinessRepository(PromDbContext context)
        {
            this.context = context;
        }

        public IQueryable<BusinessEntity> GetBusinesses(Guid orgId)
        {
            var query = context!.Businesses!.Where(x => x.OrgId == orgId && x.BusinessStatus == RecordStatus.Active.ToString());
            return query;
        }
        
        public IQueryable<BusinessEntity> GetBusinessesQuery()
        {
            var query = context.Businesses.Where(x =>  x.BusinessStatus == RecordStatus.Active.ToString()).AsQueryable();
            return query;
        }

        public IQueryable<UserBusinessEntity> GetUserBusinessQuery()
        {
            return context.UserBusinesses.AsQueryable();
        }


        public IQueryable<UserBusinessEntity> GetBusinessUserList()
        {
            var query = context!.UserBusinesses!;
            return query;
        }

        public IQueryable<UserBusinessEntity> GetUserBusinessList(Guid? userId, Guid orgId)
        {
            var query = context!.UserBusinesses!.Where(x => x.OrgId == orgId && (!userId.HasValue || x.UserId == userId));
            return query;
        }

        public bool IsCustomBusinessIdExist(string orgCustomId)
        {
            var count = context!.Businesses!
                .Where(x => x!.BusinessCustomId!.Equals(orgCustomId))
                .Count();
            return count >= 1;
        }

        public async Task AddBusiness(BusinessEntity bus)
        {
            context!.Businesses!.Add(bus);
            await context.SaveChangesAsync();
        }

        public void UpdateUserBusiness(List<UserBusinessEntity> bus)
        {
            context.UpdateRange(bus);
        }

        public DbContext Context()
        {
            return context;
        }
    }
}