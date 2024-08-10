using ERP.Services.API.Models.ResponseModels.Role;

namespace ERP.Services.API.Interfaces
{
    public interface IRoleService
    {
        public List<RoleListResponse> GetRolesList(string orgId, string rolesList);
    }
}
