using Catalog.API.Context;
using Catalog.API.Filters;
using Common.Logging;
using Common.Logging.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// config logger
builder.Host.UseSerilog(SeriLogger.Configure);

// config db
builder.Services.AddDbContext<CatalogDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// config trace
builder.Services.RegisterOpenTelemetry(builder.Configuration.GetValue<string>("ServiceConfig:serviceName"));

// config filters
builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.Filters.Add(typeof(ExceptionFilter));
    options.Filters.Add(typeof(UnitOfWorkCommitFilter));
    options.Filters.Add(typeof(ValidationFilter));
 });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
