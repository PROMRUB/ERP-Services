using ERP.Services.API.Entities;
using ERP.Services.API.Interfaces;
using ERP.Services.API.PromServiceDbContext;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Repositories
{
    public class OrganizationRepository : BaseRepository, IOrganizationRepository
    {
        public OrganizationRepository(PromDbContext context)
        {
            this.context = context;
        }

        public Task<OrganizationEntity> GetOrganization()
        {
            var result = context!.Organizations!
                .Where(x => x.OrgCustomId!.Equals(orgId))
                .FirstOrDefaultAsync();
            return result!;
        }

        public IQueryable<OrganizationEntity> GetOrganizationList()
        {
            return context!.Organizations!;
        }

        public void AddUserToOrganization(OrganizationUserEntity user)
        {
            user.OrgCustomId = orgId;
            context!.OrganizationUsers!.Add(user);
            context.SaveChanges();
        }

        public void UpdateUserToOrganization(OrganizationUserEntity user)
        {
            var existing = context!.OrganizationUsers!
                .SingleOrDefault(x => x.OrgUserId == user.OrgUserId);

            if (existing == null)
                throw new KeyNotFoundException("1104");

            if (!string.IsNullOrWhiteSpace(user.Username) &&
                !string.Equals(existing.Username, user.Username, StringComparison.OrdinalIgnoreCase))
            {
                var dup = context.OrganizationUsers!
                    .Any(x => x.OrgCustomId == orgId
                              && x.Username!.ToLower() == user.Username!.ToLower()
                              && x.OrgUserId != existing.OrgUserId);
                if (dup)
                    throw new ArgumentException("1111");
            }

            if (!string.IsNullOrWhiteSpace(user.Username))
                existing.Username = user.Username!.Trim();

            if (!string.IsNullOrWhiteSpace(user.FirstNameTh))
                existing.FirstNameTh = user.FirstNameTh!.Trim();

            if (!string.IsNullOrWhiteSpace(user.LastnameTh))
                existing.LastnameTh = user.LastnameTh!.Trim();

            if (user.email != null) 
                existing.email = user.email;

            if (user.TelNo != null)
                existing.TelNo = user.TelNo;
            
            context.SaveChanges();
        }

        public IQueryable<OrganizationUserEntity> GetUserListAsync()
        {
            return context!.OrganizationUsers!;
        }

        public DbContext Context()
        {
            return context;
        }

        public void UpdateUserRange(List<OrganizationUserEntity> userList)
        {
            context.UpdateRange(userList);
        }

        public async Task<IEnumerable<OrganizationUserEntity>> GetUserAllowedOrganizationAsync(string userName)
        {
            var query = await context!.OrganizationUsers!.Where(x => x!.Username!.Equals(userName))
                .OrderByDescending(e => e.OrgCustomId).ToListAsync();
            return query;
        }

        public bool IsUserNameExist(string userName)
        {
            var count = context!.OrganizationUsers!
                .Where(x => x!.Username!.Equals(userName) && x!.OrgCustomId!.Equals(orgId))
                .Count();
            return count >= 1;
        }

        public bool IsCustomOrgIdExist(string orgCustomId)
        {
            var count = context!.Organizations!
                .Where(x => x!.OrgCustomId!.Equals(orgCustomId))
                .Count();
            return count >= 1;
        }

        public async Task<OrganizationUserEntity> GetUserInOrganization(string userName)
        {
            var query = await context!.OrganizationUsers!
                .Where(x => x!.Username!.Equals(userName) && x!.OrgCustomId!.Equals(orgId))
                .FirstOrDefaultAsync();
            return query!;
        }

        public async Task AddOrganization(OrganizationEntity org)
        {
            context!.Organizations!.Add(org);
            await context.SaveChangesAsync();
        }

        public async Task<OrganizationNumberEntity> OrganizationNumberAsync()
        {
            try
            {
                var currentDate = DateTime.Today.Date.ToUniversalTime().ToString("yyyyMMdd");
                var query = await context!.OrganizationNumbers!.Where(x => x.OrgDate == currentDate)
                    .FirstOrDefaultAsync();
                bool HaveOrganization = true;
                while (HaveOrganization)
                {
                    if (query == null)
                    {
                        var newRec = new OrganizationNumberEntity
                        {
                            OrgId = Guid.NewGuid(),
                            OrgDate = currentDate,
                            Allocated = 0
                        };
                        context.OrganizationNumbers!.Add(newRec);
                        context.SaveChanges();
                        query = await context.OrganizationNumbers!.Where(x => x.OrgDate == currentDate)
                            .FirstOrDefaultAsync();
                    }
                    else
                    {
                        HaveOrganization = false;
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