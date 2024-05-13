using ERP.Services.API.Entities;
using Npgsql.Replication.PgOutput.Messages;

namespace ERP.Services.API.Interfaces
{
    public interface IRoleRepository
    {
        public void SetCustomOrgId(string customOrgId);
        public IEnumerable<RoleEntity> GetRolesList(string rolesList);
        public void Commit();
    }
}
