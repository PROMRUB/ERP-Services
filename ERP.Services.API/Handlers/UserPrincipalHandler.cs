using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ERP.Services.API.Handlers
{
    public class UserPrincipalHandler
    {
        private IHttpContextAccessor context;
        public UserPrincipalHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.context = httpContextAccessor;
        }

        public string ip
        {
            get
            {
                try
                {
                    return context.HttpContext.Connection.RemoteIpAddress.ToString();
                }
                catch
                {
                    return "";
                }
            }
        }

        public Guid Id => new Guid(GetClaimValue("Id"));
        public string Firstname => GetClaimValue("Firstname");
        public string Lastname => GetClaimValue("Lastname");
        public string Email => GetClaimValue("Email");

        private string GetClaimValue(string type)
        {
            if (!string.IsNullOrEmpty(context.HttpContext.Request.Headers["Authorization"].ToString()))
            {
                string accessToken = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault(header => header.StartsWith("Bearer ")).Split(" ")[1];
                var claimsList = ReadJWTClaimList(accessToken);

                if (type == "Id")
                {
                    return claimsList.Find(x => x.Type.Equals("Id")).Value ;
                }
                else if (type == "Firstname")
                {
                    return claimsList.Find(x => x.Type.Equals("Firstname")).Value;
                }
                else if (type == "Lastname")
                {
                    return claimsList.Find(x => x.Type.Equals("Lastname")).Value;
                }
                else if (type == "Email")
                {
                    return claimsList.Find(x => x.Type.Equals("Email")).Value;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        private static List<Claim> ReadJWTClaimList(string accessToken)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var token = jwtSecurityTokenHandler.ReadJwtToken(accessToken);

            var claims = token.Claims;

            List<Claim> claimsList = new List<Claim>();
            foreach (var claim in claims)
            {
                claimsList.Add(new Claim(claim.Type, claim.Value));
            }

            return claimsList;
        }
    }
}
