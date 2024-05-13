using Microsoft.IdentityModel.Tokens;

namespace ERP.Services.API.Interfaces
{
    public interface IJwtSigner
    {
        public SecurityKey GetSignedKey(string? url);
    }
}
