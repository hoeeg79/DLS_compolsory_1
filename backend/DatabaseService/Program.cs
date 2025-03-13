using System.Reflection;
using DatabaseService.Repository;
using DatabaseService.Service;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

string serviceName = Assembly.GetCallingAssembly().GetName().Name ?? "Unknown";

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName))
        .AddConsoleExporter();
});
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter());


// Add services to the container.
// Adding Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string hostname = "localhost";
if (!builder.Environment.IsDevelopment())
{
    hostname = "db-postgres";
}
string username = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "";
string password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "";

// Read the PostgreSQL username and password from Docker secret files
var postgresUserFile = "/run/secrets/db_user";
var postgresPasswordFile = "/run/secrets/db_password";

if (File.Exists(postgresUserFile))
{
    username = File.ReadAllText(postgresUserFile).Trim();
}

if (File.Exists(postgresPasswordFile))
{
    password = File.ReadAllText(postgresPasswordFile).Trim();
}

// Adding Database Context
builder.Services.AddDbContext<DatabaseContext>(option =>
    option.UseNpgsql(
        $"Host={hostname};Port=5432;Database=postgres;Username={username};Password={password};Trust Server Certificate=true;"));

// Adding Controllers and Repos
builder.Services.AddScoped<IDatabaseRepository, DatabaseRepository>();
builder.Services.AddScoped<IDatabaseService, DatabaseService.Service.DatabaseService>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();