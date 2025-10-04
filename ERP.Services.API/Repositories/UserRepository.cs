using ERP.Services.API.Entities;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.Authentications;
using ERP.Services.API.PromServiceDbContext;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task AddUserToBusinessAsync(UserBusinessEntity user, CancellationToken ct = default)
        {
            // ควรเช็คให้ครบ key ของความเป็น unique: OrgId + UserId + BusinessId
            var existing = await context.UserBusinesses
                .FirstOrDefaultAsync(x =>
                    x.OrgId == user.OrgId &&
                    x.UserId == user.UserId &&
                    x.BusinessId == user.BusinessId, ct);

            if (existing == null)
            {
                // สร้างใหม่ → คำนวณ EmployeeRunning/EmployeeCode ต่อองค์กร (หรือ scope ที่คุณต้องการ)
                var maxRunning = await context.UserBusinesses
                    .Where(x => x.OrgId == user.OrgId)
                    .Select(x => (int?)x.EmployeeRunning)
                    .MaxAsync(ct) ?? 0;

                user.EmployeeRunning = maxRunning + 1;
                user.EmployeeCode = user.EmployeeRunning.ToString("D4");

                // กำหนด Role จาก request
                // user.Role = user.Role; // มีอยู่แล้วจาก request

                await context.UserBusinesses.AddAsync(user, ct);
                await context.SaveChangesAsync(ct);
                return;
            }

            // อัปเดตเคส “มีอยู่แล้ว”
            // ไม่ต้อง Add() ซ้ำ
            // ถ้าอยากรีเซ็ต employee code เมื่อก่อนเป็น 0 (ตาม logic เดิม) ให้ทำแบบระวัง
            if (existing.EmployeeRunning == 0)
            {
                var maxRunning = await context.UserBusinesses
                    .Where(x => x.OrgId == existing.OrgId)
                    .Select(x => (int?)x.EmployeeRunning)
                    .MaxAsync(ct) ?? 0;

                existing.EmployeeRunning = maxRunning + 1;
                existing.EmployeeCode = existing.EmployeeRunning.ToString("D4");
            }

            existing.Role = user.Role;

            context.UserBusinesses.Update(existing);
            await context.SaveChangesAsync(ct);
        }

// ปรับเป็น async ให้สอดคล้องกับ service
        public async Task<List<UserBusinessEntity>> GetUserToBusinessAllAsync(Guid orgId, Guid userId,
            CancellationToken ct = default)
        {
            return await context.UserBusinesses
                .AsNoTracking()
                .Where(x => x.OrgId == orgId && x.UserId == userId)
                .OrderBy(x => x.BusinessId)
                .ThenBy(x => x.Role)
                .ToListAsync(ct);
        }

        public async Task RemoveUserToBusinessAsync(UserBusinessEntity user, CancellationToken ct = default)
        {
            var existing = await context.UserBusinesses
                .FirstOrDefaultAsync(x =>
                    x.OrgId == user.OrgId &&
                    x.UserId == user.UserId &&
                    x.BusinessId == user.BusinessId, ct);

            if (existing != null)
            {
                context.UserBusinesses.Remove(existing);
                await context.SaveChangesAsync(ct);
            }
        }

        public void AddRoleToUser(Guid UserId, Guid BusinessId, UserBusinessEntity user)
        {
            var query = context!.UserBusinesses.ToList();
            var currentUser = query.Where(x => x.UserId == UserId && x.BusinessId == BusinessId).FirstOrDefault();
            if (currentUser != null)
            {
                if (currentUser.EmployeeRunning == 0)
                {
                    var maxEmployeeRunning = query.Max(x => x.EmployeeRunning);

                    currentUser.EmployeeRunning = maxEmployeeRunning + 1;

                    currentUser.EmployeeCode = currentUser.EmployeeRunning.ToString("D4");
                }

                currentUser.Role = user!.Role;

                context.SaveChanges();
            }
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
                var query = context!.OrganizationUsers!.Where(x =>
                    x.Username!.Equals(username) && x.Password!.Equals(password));
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