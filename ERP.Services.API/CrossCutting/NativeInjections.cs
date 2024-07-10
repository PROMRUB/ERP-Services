using ERP.Services.API.Authentications;
using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Repositories;
using ERP.Services.API.Seeder.JsonData;
using ERP.Services.API.Services.ApiKey;
using ERP.Services.API.Services.Condition;
using ERP.Services.API.Services.Customer;
using ERP.Services.API.Services.Organization;
using ERP.Services.API.Services.PaymentAccount;
using ERP.Services.API.Services.Product;
using ERP.Services.API.Services.Project;
using ERP.Services.API.Services.Role;
using ERP.Services.API.Services.SystemConfig;
using ERP.Services.API.Services.User;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace ERP.Services.API.CrossCutting
{
    public static class NativeInjections
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<DataSeeder>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<JwtSecurityTokenHandler>();
            services.AddScoped<IServiceCollection, ServiceCollection>();
            services.AddScoped<UserPrincipalHandler>();

            services.AddTransient<IAuthorizationHandler, GenericRbacHandler>();
            services.AddScoped<IBasicAuthenticationRepo, BasicAuthenticationRepo>();
            services.AddScoped<IBearerAuthenticationRepo, BearerAuthenticationRepo>();

            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IApiKeyService, ApiKeyService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISystemConfigServices, SystemConfigServices>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IConditionService, ConditionService>();
            services.AddScoped<IPaymentAccountService, PaymentAccountService>();

            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IBusinessRepository, BusinessRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISystemConfigRepository, SystemConfigRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IConditionRepository, ConditionRepository>();
            services.AddScoped<IPaymentAccountRepository, PaymentAccountRepository>();
        }
    }
}
