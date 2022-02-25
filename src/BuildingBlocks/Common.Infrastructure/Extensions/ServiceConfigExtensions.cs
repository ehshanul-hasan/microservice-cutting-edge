using System;
using System.Configuration;
using System.Threading;
using Common.Infrastructure.Constants;
using Common.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Serilog;
using Winton.Extensions.Configuration.Consul;

namespace Common.Infrastructure.Extensions
{
    public static class ServiceConfigExtensions
    {
        public static ServiceConfig GetServiceConfig(this IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var serviceConfig = new ServiceConfig
            {
                ServiceDiscoveryAddress = configuration.GetValue<Uri>("ServiceConfig:serviceDiscoveryAddress"),
                ServiceAddress = configuration.GetValue<Uri>("ServiceConfig:serviceAddress"),
                ServiceName = configuration.GetValue<string>("ServiceConfig:serviceName"),
                ServiceId = configuration.GetValue<string>("ServiceConfig:serviceId")
            };

            return serviceConfig;
        }

        public static void IncorporateCentralConfiguration(this IConfigurationBuilder configuration)
        {
            var serviceName = ((IConfiguration)configuration).GetValue<string>("ServiceConfig:serviceName");

            foreach (var config in ConstantHelper.CONFIGURATION_KEY)
            {
                try
                {
                    configuration.AddConsul(
                        config.Replace("{servicename}", serviceName),
                        options =>
                        {
                            options.ReloadOnChange = true;
                        });
                }
                catch (Exception ex)
                {
                    Log.Warning($"Configuration key not found. {ex.Message}");
                }
                
            }
        }
    }
}