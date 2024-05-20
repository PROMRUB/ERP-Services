using ERP.Services.API.Authentications;
using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Repositories;
using ERP.Services.API.Seeder.JsonData;
using ERP.Services.API.Services.ApiKey;
using ERP.Services.API.Services.Organization;
using ERP.Services.API.Services.Role;
using ERP.Services.API.Services.User;
using Microsoft.AspNetCore.Authorization;

namespace ERP.Services.API.CrossCutting
{
    public static class NativeInjections
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<DataSeeder>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IServiceCollection, ServiceCollection>();

            services.AddTransient<IAuthorizationHandler, GenericRbacHandler>();
            services.AddScoped<IBasicAuthenticationRepo, BasicAuthenticationRepo>();
            services.AddScoped<IBearerAuthenticationRepo, BearerAuthenticationRepo>();

            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IApiKeyService, ApiKeyService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IBusinessRepository, BusinessRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISystemConfigRepository, SystemConfigRepository>();

        }
    }
}
