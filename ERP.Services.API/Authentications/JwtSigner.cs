using ERP.Services.API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace ERP.Services.API.Authentications
{
    public class JwtSigner : IJwtSigner
    {
        public JwtSigner()
        {
        }

        public static void ResetSigedKeyJson()
        {
            JwtSignerKey.ResetSigedKeyJson();
        }

        public SecurityKey GetSignedKey(string? url)
        {
            return JwtSignerKey.GetSignedKey(url);
        }
    }
}
