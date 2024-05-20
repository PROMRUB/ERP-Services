using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Authorization;
using ERP.Services.API.Models.RequestModels.User;
using ERP.Services.API.Models.ResponseModels.Organization;
using ERP.Services.API.Models.ResponseModels.User;
using ERP.Services.API.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ERP.Services.API.Services.User
{
    public class UserService : BaseService, IUserService
    {
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IUserRepository repository;

        public UserService(IMapper mapper,
            IConfiguration configuration,
            IUserRepository repository) : base()
        {
            this.mapper = mapper;
            this.configuration = configuration;
            this.repository = repository;
        }

        public List<UserResponse> GetUsers(string orgId)
        {
            repository!.SetCustomOrgId(orgId);
            var query = repository!.GetUsers();
            return mapper.Map<IEnumerable<UserEntity>, List<UserResponse>>(query);
        }

        public async Task<UserResponse> GetUserByName(string orgId, string userName)
        {
            repository!.SetCustomOrgId(orgId);
            var query = await repository!.GetUserByName(userName);
            return mapper.Map<UserEntity, UserResponse>(query);
        }

        public void AddUser(string orgId, UserRequest request)
        {
            repository!.SetCustomOrgId(orgId);
            if (IsEmailExist(orgId, request!.UserEmail!) || IsUserNameExist(orgId, request!.UserName!))
                throw new ArgumentException("1111");
            var query = mapper.Map<UserRequest, UserEntity>(request);
            repository!.AddUser(query);
        }

        public bool IsEmailExist(string orgId, string email)
        {
            repository!.SetCustomOrgId(orgId);
            var result = repository!.IsEmailExist(email);
            return result;
        }

        public bool IsUserNameExist(string orgId, string userName)
        {
            repository!.SetCustomOrgId(orgId);
            var result = repository!.IsUserNameExist(userName);
            return result;
        }

        public bool IsUserIdExist(string orgId, string userId)
        {
            repository!.SetCustomOrgId(orgId);
            var result = repository!.IsUserIdExist(userId);
            return result;
        }

        public async Task<AuthorizationResponse> SignIn(string orgId, SignInRequest user)
        {
            repository!.SetCustomOrgId(orgId);
            var query = await repository!.GetUserSignIn(user.Username, user.Password).FirstOrDefaultAsync();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourVeryLongSecretKeyForHmacSha256Authentication12345"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim("Id", query.OrgUserId.ToString()),
                new Claim("Firstname", query.FirstNameTh),
                new Claim("Lastname", query.LastnameTh),
                new Claim("Email", query.email)
            };
            var token = new JwtSecurityToken(
                issuer: "CybertracxCo.,ltd",
                audience: "CybertracxCo.,ltd",
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            var result = new AuthorizationResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
            await repository.CreateUserSession(new UserSessionEntity
            {
                Token = result.Token,
                UserId = query.OrgUserId
            });
            return result;
        }

        public async Task<OrganizationUserResponse> GetUserProfile(Guid userId)
        {
            var request = await repository.GetUserProfiles().Where(x => x.OrgUserId == userId).FirstOrDefaultAsync(); 
            return mapper.Map<OrganizationUserEntity, OrganizationUserResponse>(request);
        }
    }
}
