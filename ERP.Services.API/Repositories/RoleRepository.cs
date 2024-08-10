using ERP.Services.API.Entities;
using ERP.Services.API.Interfaces;
using ERP.Services.API.PromServiceDbContext;

namespace ERP.Services.API.Repositories
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleRepository(PromDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<RoleEntity> GetRolesList(string rolesList)
        {
            var list = rolesList.Split(',').ToList();
            var arr = context!.Roles!.Where(p => list.Contains(p.RoleName!)).ToList();
            return arr;
        }
    }
}
