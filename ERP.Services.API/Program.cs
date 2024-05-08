using QuestPDF.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("IsDev")))
    throw new ArgumentNullException($"{0} is Null", "IsDev");

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PostgreSQL_Host")))
    throw new ArgumentNullException($"{0} is Null", "PostgreSQL_Host");

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PostgreSQL_Database")))
    throw new ArgumentNullException($"{0} is Null", "PostgreSQL_Database");

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PostgreSQL_User")))
    throw new ArgumentNullException($"{0} is Null", "PostgreSQL_User");

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PostgreSQL_Password")))
    throw new ArgumentNullException($"{0} is Null", "PostgreSQL_Password");

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

var connStr = $"Host={cfg["PostgreSQL:Host"]}; Database={cfg["PostgreSQL:Database"]}; Username={cfg["PostgreSQL:User"]}; Password={cfg["PostgreSQL:Password"]}";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
