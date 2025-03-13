using System.Diagnostics;
using System.Reflection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using ILogger = Serilog.ILogger;

namespace DatabaseService.Monitoring;

public static class MonitoringService
{
    public static readonly string ServiceName = Assembly.GetCallingAssembly().GetName().Name ?? "Unknown";
    public static TracerProvider TracerProvider;
    public static ActivitySource ActivitySource = new ActivitySource(ServiceName);
    public static ILogger Log => Serilog.Log.Logger;

    static MonitoringService()
    {
        TracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSource(ActivitySource.Name)
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ServiceName))
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter()
            .AddZipkinExporter(config => config.Endpoint = new Uri("http://zipkin:9411/api/v2/spans"))
            .Build();

        Serilog.Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .WriteTo.Seq("http://seq:5341")
            .CreateLogger();
    }
}