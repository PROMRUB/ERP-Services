using ERP.Services.API.Authentications;
using ERP.Services.API.CrossCutting;
using ERP.Services.API.Helpers;
using ERP.Services.API.Models.ResponseModels.Common;
using ERP.Services.API.PromServiceDbContext;
using ERP.Services.API.Seeder.JsonData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using sib_api_v3_sdk.Client;
using System.Threading.RateLimiting;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
//
// if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("IsDev")))
//     throw new ArgumentNullException(string.Format("{0} is Null", "IsDev"));
//
// if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PostgreSQL_Host")))
//     throw new ArgumentNullException(string.Format("{0} is Null", "PostgreSQL_Host"));
//
// if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PostgreSQL_Database")))
//     throw new ArgumentNullException(string.Format("{0} is Null", "PostgreSQL_Database"));
//
// if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PostgreSQL_User")))
//     throw new ArgumentNullException(string.Format("{0} is Null", "PostgreSQL_User"));
//
// if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PostgreSQL_Password")))
//     throw new ArgumentNullException(string.Format("{0} is Null", "PostgreSQL_Password"));
//
// if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ERP_EMAIL")))
//     throw new ArgumentNullException(string.Format("{0} is Null", "ERP_EMAIL"));

var log = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
Log.Logger = log;

var cfg = builder.Configuration;

QuestPDF.Settings.License = LicenseType.Community;

cfg["IsDev"] = Environment.GetEnvironmentVariable("IsDev")!;
cfg["PostgreSQL:Host"] = Environment.GetEnvironmentVariable("PostgreSQL_Host")!;
cfg["PostgreSQL:Database"] = Environment.GetEnvironmentVariable("PostgreSQL_Database")!;
cfg["PostgreSQL:User"] = Environment.GetEnvironmentVariable("PostgreSQL_User")!;
cfg["PostgreSQL:Password"] = Environment.GetEnvironmentVariable("PostgreSQL_Password")!;
cfg["ERP_EMAIL"] = Environment.GetEnvironmentVariable("ERP_EMAIL")!;

 
Configuration.Default.ApiKey.Add("api-key",
    Environment.GetEnvironmentVariable("ERP_EMAIL"));


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var connStr =
    // $"Host=119.13.29.117;Port=2022; Database=erp; Username=postgres; Password=yoouhCyodbow-jg0up";
    $"Host={cfg["PostgreSQL:Host"]}; Database={cfg["PostgreSQL:Database"]}; Username={cfg["PostgreSQL:User"]}; Password={cfg["PostgreSQL:Password"]}";
builder.Services.AddDbContext<PromDbContext>(
    options =>
    {
        options.UseNpgsql(connStr);
        options.UseTriggers(
            triggerOptions => { triggerOptions.AddTrigger<QuotationNoTrigger>(); }
        );
    }
);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning();
// builder.Services.Configure<MailConfig>(builder.Configuration.GetSection("MailConfig"));


builder.Services.AddAuthentication("BasicOrBearer")
    .AddScheme<AuthenticationSchemeOptions, AuthenticationHandlerProxy>("BasicOrBearer", null);

builder.Services.AddAuthorization(options =>
{
    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder("BasicOrBearer");
    defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();

    options.AddPolicy("GenericRolePolicy", policy => policy.AddRequirements(new GenericRbacRequirement()));
});

//builder.Services.AddRateLimiter(options =>
//{
//    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, IPAddress>(context =>
//    {
//        var ip = context.Connection.RemoteIpAddress;
//        return RateLimitPartition.GetFixedWindowLimiter(ip!, factory => new FixedWindowRateLimiterOptions
//        {
//            PermitLimit = 5,
//            Window = TimeSpan.FromMinutes(1),
//            QueueLimit = 0
//        });
//    });
//});


if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(config =>
    {
        config.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo()
                { Title = "Prom API", Version = "v1", Description = "Prom API Version 1", });

        config.OperationFilter<SwaggerParameterFilters>();
        config.DocumentFilter<SwaggerVersionMapping>();

        config.DocInclusionPredicate((version, desc) =>
        {
            if (!desc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
            var versions = methodInfo.DeclaringType!.GetCustomAttributes(true).OfType<ApiVersionAttribute>()
                .SelectMany(attr => attr.Versions);
            var maps = methodInfo.GetCustomAttributes(true).OfType<MapToApiVersionAttribute>()
                .SelectMany(attr => attr.Versions).ToArray();
            version = version.Replace("v", "");
            return versions.Any(v => v.ToString() == version && maps.AsEnumerable().Any(v => v.ToString() == version));
        });


        config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter the Bearer token in the field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        config.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });
}

NativeInjections.RegisterServices(builder.Services);


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PromDbContext>();
    dbContext.Database.Migrate();

    //var service = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    //service.Seed();
}


app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
        {
            // config.SwaggerEndpoint("/v1/swagger/v1/swagger.json", "ERP");
        }
    );
}

app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();