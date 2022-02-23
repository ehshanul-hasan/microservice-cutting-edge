using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Logging.Enrichers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog;
using Serilog.Configuration;

namespace Common.Logging.Extensions
{
    public static class LoggingExtensions
    {
        public static string GetCorrelationId(this HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue("Cko-Correlation-Id", out StringValues correlationId);

            return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
        }

        public static LoggerConfiguration CustomerLogEnricher(
            this LoggerEnrichmentConfiguration enrich)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.With<CustomLogEnricher>();
        }
    }
}
