using Common.Infrastructure.Extensions;
using Common.Logging;
using Common.Logging.Extensions;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// config centralized config
builder.Configuration.IncorporateCentralConfiguration();

// config logger
builder.Host.UseSerilog(SeriLogger.Configure);

// config redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisCacheConnection");
});

// config trace
builder.Services.RegisterOpenTelemetry(builder.Configuration.GetValue<string>("ServiceConfig:serviceName"));


// config rabbitmq
builder.Services.AddMassTransit(config => {
    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMqConnection"));
    });
});
builder.Services.AddMassTransitHostedService();

builder.Services.AddControllers();

// config consul
builder.Services.RegisterConsulServices(builder.Configuration.GetServiceConfig());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHealthChecks()
       .AddRedis(builder.Configuration.GetConnectionString("RedisCacheConnection"), "Redis Health", HealthStatus.Degraded);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
