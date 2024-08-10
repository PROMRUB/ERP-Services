using ERP.Services.API.Entities;
using ERP.Services.API.Interfaces;
using ERP.Services.API.PromServiceDbContext;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(PromDbContext context)
        {
            this.context = context;
        }

        public void AddUser(UserEntity user)
        {
            context!.Users!.Add(user);
            context.SaveChanges();
        }

        public void AddUserToBusiness(UserBusinessEntity user)
        {
            context!.UserBusinesses!.Add(user);
            context.SaveChanges();
        }

        public IEnumerable<UserEntity> GetUsers()
        {
            var query = context!.Users!.Where(p => !p.UserName!.Equals(null)).ToList();
            return query;
        }

        public IQueryable<OrganizationUserEntity> GetUserProfiles()
        {
            var query = context!.OrganizationUsers!;
            return query;
        }

        public bool IsEmailExist(string email)
        {
            var count = context!.Users!.Where(p => p!.UserEmail!.Equals(email)).Count();
            return count >= 1;
        }

        public bool IsUserNameExist(string userName)
        {
            var count = context!.Users!.Where(p => p!.UserName!.Equals(userName)).Count();
            return count >= 1;
        }

        public bool IsUserIdExist(string userId)
        {
            try
            {
                Guid id = Guid.Parse(userId);
                var count = context!.Users!.Where(p => p!.UserId!.Equals(id)).Count();
                return count >= 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<UserEntity> GetUserByName(string userName)
        {
            var u = await context!.Users!.Where(p => p!.UserName!.Equals(userName)).FirstOrDefaultAsync();
            return u!;
        }

        public IQueryable<OrganizationUserEntity> GetUserSignIn(string username, string password)
        {
            try
            {
                var query = context!.OrganizationUsers!.Where(x => x.Username!.Equals(username) && x.Password!.Equals(password));
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<UserSessionEntity> GetUserSession()
        {
            return context!.UserSessions!;
        }

        public async Task CreateUserSession(UserSessionEntity session)
        {
            context!.UserSessions!.Add(session);
            await context.SaveChangesAsync();
        }
    }
}
