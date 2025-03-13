using System.Diagnostics;
using System.Reflection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

namespace Monitoring;

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
            .AddZipkinExporter()
            .Build();

        Serilog.Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .WriteTo.Seq("http://seq:5341")
            .CreateLogger();
    }
}