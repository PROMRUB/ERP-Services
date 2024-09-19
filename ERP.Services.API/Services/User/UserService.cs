using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Authorization;
using ERP.Services.API.Models.RequestModels.User;
using ERP.Services.API.Models.ResponseModels.Organization;
using ERP.Services.API.Models.ResponseModels.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ERP.Services.API.Services.User
{
    public class UserService : BaseService, IUserService
    {
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IUserRepository repository;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IBusinessRepository businessRepository;
        private readonly UserPrincipalHandler userPrincipalHandler;

        public UserService(IMapper mapper,
            IConfiguration configuration,
            IUserRepository repository,
            IOrganizationRepository organizationRepository,
            IBusinessRepository businessRepository,
            UserPrincipalHandler userPrincipalHandler) : base()
        {
            this.mapper = mapper;
            this.configuration = configuration;
            this.repository = repository;
            this.organizationRepository = organizationRepository;
            this.businessRepository = businessRepository;
            this.userPrincipalHandler = userPrincipalHandler;
        }

        public async Task<List<OrganizationUserResponse>> GetUsers(string orgId, Guid businessId, string role)
        {
            var result = new List<OrganizationUserResponse>();
            organizationRepository!.SetCustomOrgId(orgId);
            var org = await organizationRepository.GetOrganization();

            var userQuery = repository.GetUserProfiles().ToList();

            var userId = !role.Equals("All") && (!role.Contains("SaleManager") && !role.Contains("Admin"))
                ? userPrincipalHandler.Id
                : (Guid?)null;
            var businessQuery = businessRepository.GetUserBusinessList(userId, (Guid)org.OrgId!).ToList();

            if (!role.Equals("All") && (!role.Contains("SaleManager") && !role.Contains("Admin")))
            {
                businessQuery = businessQuery.Where(x => x.Role!.Contains(role) && org.OrgId == x.OrgId).ToList();
            }

            foreach (var item in businessQuery)
            {
                var user = userQuery.Where(x => x.OrgUserId == item.UserId).FirstOrDefault();
                var orgUser = new OrganizationUserResponse
                {
                    OrgUserId = user.OrgUserId,
                    OrgCustomId = orgId,
                    Username = user.Username,
                    Firstname = user.FirstNameTh,
                    Lastname = user.LastnameTh,
                    Fullname = user.FirstNameTh + " " + user.LastnameTh,
                    Email = user.email,
                    TelNo = user.TelNo,
                    Role = new List<string> { role }
                };

                if (item.Role == null)
                {
                    continue;
                }

                List<string> roles = item.Role
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(k => k.Trim())
                    .ToList();
                orgUser.Role = roles.Distinct().ToList();
                result.Add(orgUser);
            }

            return result;
        }

        public async Task<UserResponse> GetUserByName(string orgId, string userName)
        {
            repository!.SetCustomOrgId(orgId);
            var query = await repository!.GetUserByName(userName);
            return mapper.Map<UserEntity, UserResponse>(query);
        }


        public async Task<OrganizationUserResponse> GetUserProfile()
        {
            var request = await repository.GetUserProfiles().Where(x => x.OrgUserId == userPrincipalHandler.Id)
                .FirstOrDefaultAsync();
            var result = mapper.Map<OrganizationUserEntity, OrganizationUserResponse>(request);
            return result;
        }

        public async Task<OrganizationUserResponse> GetRole(string orgId, Guid businessId)
        {
            organizationRepository!.SetCustomOrgId(orgId);
            var org = await organizationRepository.GetOrganization();
            var request = await repository.GetUserProfiles().Where(x => x.OrgUserId == userPrincipalHandler.Id)
                .FirstOrDefaultAsync();
            var result = mapper.Map<OrganizationUserEntity, OrganizationUserResponse>(request);
            var role = await businessRepository.GetUserBusinessList(userPrincipalHandler.Id, (Guid)org.OrgId!)
                .Where(x => x.BusinessId == businessId).FirstOrDefaultAsync();

            List<string> titles = new List<string>
            {
                "SalesRepresentative",
                "SalesManager"
            };

            result.Role = role!.Role!.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(k => k.Trim())
                .ToList();

            // result.Role = keywords;  
            // foreach (var title in keywords)
            // {
            //     result.Role?.Add(title);
            // }
            return result;
        }

        public async Task RunningUser(string orgId, Guid businessId)
        {
            organizationRepository!.SetCustomOrgId(orgId);
            var org = await organizationRepository.GetOrganization();

            var users = businessRepository.GetBusinessUserList()
                .Where(x => x.OrgId == org.OrgId && x.BusinessId == businessId)
                .OrderByDescending(x => x.EmployeeRunning)
                .FirstOrDefault();

            if (users == null)
            {
                return;
            }

            var max = users.EmployeeRunning;

            if (max == 0)
            {
                var list = await businessRepository.GetBusinessUserList()
                    .Where(x => x.OrgId == org.OrgId && x.BusinessId == businessId)
                    .ToListAsync();

                foreach (var userBusinessEntity in list)
                {
                    max++;
                    userBusinessEntity.EmployeeRunning = max;
                    userBusinessEntity.EmployeeCode = max.ToString("0000");
                }

                businessRepository.UpdateUserBusiness(list);
                await businessRepository.Context().SaveChangesAsync();
            }
            else
            {
                var list = await businessRepository.GetBusinessUserList()
                    .Where(x => x.OrgId == org.OrgId && x.BusinessId == businessId
                                                     && x.EmployeeRunning == 0)
                    .OrderByDescending(x => x.EmployeeRunning)
                    .ToListAsync();

                if (!list.Any())
                {
                    return;
                }

                max = businessRepository.GetBusinessUserList()
                    .Where(x => x.OrgId == org.OrgId && x.BusinessId == businessId)
                    .OrderByDescending(x => x.EmployeeRunning)
                    .FirstOrDefault()!.EmployeeRunning;

                foreach (var userBusinessEntity in list)
                {
                    max++;
                    userBusinessEntity.EmployeeRunning = max;
                    userBusinessEntity.EmployeeCode = max.ToString("0000");
                }

                businessRepository.UpdateUserBusiness(list);
                await businessRepository.Context().SaveChangesAsync();
            }
        }

        public async Task ChangeAllPassword()
        {
            var list = new List<string>()
            {
                "amornrat.t@securesolutionsasia.com",
                "kitsada.t@securesolutionsasia.com",
                "muankhwan.u@securesolutionsasia.com",
                "witchayada.a@securesolutionsasia.com",
                "nopporn.k@cybermasters.co.th",
                "nattapol.c@securesolutionsasia.com"
            };

            var user = organizationRepository.GetUserListAsync()
                .Where(x => list.Contains(x.email))
                .ToList();

            foreach (var entity in user)
            {
                entity.Password = "2124416";
            }

            organizationRepository.UpdateUserRange(user);

            await organizationRepository.Context().SaveChangesAsync();
        }

        public async Task<IQueryable<UserBusinessEntity>> GetUserBusiness(string orgId)
        {
            organizationRepository!.SetCustomOrgId(orgId);
            var org = await organizationRepository.GetOrganization();
            var userId = userPrincipalHandler.Id;
            var result = businessRepository.GetUserBusinessList(userId, (Guid)org.OrgId!);
            var test = await result.ToListAsync();
            return result;
        }

        public void AddUser(string orgId, UserRequest request)
        {
            repository!.SetCustomOrgId(orgId);
            if (IsEmailExist(orgId, request!.UserEmail!) || IsUserNameExist(orgId, request!.UserName!))
                throw new ArgumentException("1111");
            var query = mapper.Map<UserRequest, UserEntity>(request);
            repository!.AddUser(query);
        }

        public async Task AddUserToBusinessAsync(string orgId, AddUserToBusinessRequest request)
        {
            repository!.SetCustomOrgId(orgId);
            organizationRepository!.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = mapper.Map<AddUserToBusinessRequest, UserBusinessEntity>(request);
            query.OrgId = organization.OrgId;
            repository.AddUserToBusiness(query);
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
            var securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("YourVeryLongSecretKeyForHmacSha256Authentication12345"));
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
            var session = await repository.GetUserSession().Where(x => x.Token.Equals(result.Token))
                .FirstOrDefaultAsync();
            if (session == null)
            {
                await repository.CreateUserSession(new UserSessionEntity
                {
                    Token = result.Token,
                    UserId = query.OrgUserId
                });
            }

            return result;
        }
    }
}