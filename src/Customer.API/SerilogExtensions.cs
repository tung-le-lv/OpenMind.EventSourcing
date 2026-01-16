using Serilog;
using Serilog.Exceptions;
using Serilog.Extensions.Hosting;

namespace Customer.API;

public static class SerilogExtensions
{
    public static void AddSerilog(this IServiceCollection services,
        IConfiguration configuration,
        IHostBuilder hostBuilder,
        string? serviceName,
        string? appName)
    {
        Log.Logger =
            new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service", serviceName)
                .Enrich.WithProperty("Application", appName)
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Debug()
                .Filter.ByExcluding(logEvent =>
                    logEvent.Properties.TryGetValue("RequestPath", out var requestPath) &&
                    requestPath.ToString().Contains("/health") &&
                    logEvent.Properties.TryGetValue("StatusCode", out var statusCode) &&
                    statusCode.ToString() == "200")
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

        hostBuilder.UseSerilog(Log.Logger);
        services.AddSingleton(Log.Logger);

        services.AddSingleton<DiagnosticContext>();
    }
}