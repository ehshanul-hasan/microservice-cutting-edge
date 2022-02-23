using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Common.Logging.Extensions
{
    public static class TracingExtension
    {
        public static void RegisterOpenTelemetry(this IServiceCollection services, string serviceName)
        {
            services.AddOpenTelemetryTracing(builder =>
            {
                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation(options => options.SetDbStatementForText = true)
                    .AddMassTransitInstrumentation()
                    .AddZipkinExporter();
            });
        }
    }
}
