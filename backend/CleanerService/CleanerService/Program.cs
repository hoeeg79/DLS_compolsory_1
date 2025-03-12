using System.Diagnostics;
using System.Reflection;
using CleanerService.Repository;
using CleanerService.Service;
using Monitoring;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

string hostname = "localhost:5108";
if (!builder.Environment.IsDevelopment())
{
    hostname = "db-api:8080";
}

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
        .AddConsoleExporter()
        .AddZipkinExporter(config => config.Endpoint = new Uri("http://zipkin:9411/api/v2/spans")))
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter());

builder.Services.AddControllers();
builder.Services.AddHttpClient<ICleanerRepository, CleanerRepository>(client =>
{
    client.BaseAddress = new Uri($"http://{hostname}");
});
builder.Services.AddScoped<ICleanerService, CleanerService.Service.CleanerService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAllOrigins",
        configurePolicy: policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors(policyName: "AllowAllOrigins");

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