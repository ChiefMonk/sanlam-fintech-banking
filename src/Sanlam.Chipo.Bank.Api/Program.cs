using Microsoft.Extensions.Configuration;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Sanlam.Chipo.Bank.Application;
using Sanlam.Chipo.Bank.Domain;
using Sanlam.Chipo.Bank.Infrastructure;
using Sanlam.Chipo.Bank.Infrastructure.Sql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterDomainDependencies(builder.Configuration);
builder.Services.RegisterApplicationDependencies(builder.Configuration);
builder.Services.RegisterInfrastructureDependencies(builder.Configuration);
builder.Services.RegisterInfrastructureSqlDependencies(builder.Configuration);

builder.Services.AddAuthorization();

var enableOpenTelemetry =  builder.Configuration.GetValue<bool>("SanlamBank:EnableOpenTelemetry");

if (enableOpenTelemetry)
{
    builder.Services.AddOpenTelemetry()
        .ConfigureResource(r => r.AddService(builder.Environment.ApplicationName))
        .WithMetrics(metrics =>
        {
            metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddPrometheusExporter();
        })
        .WithTracing(tracing =>
        {
            tracing
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation();
        });
}

// register controllers and swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (enableOpenTelemetry)
{
    app.MapPrometheusScrapingEndpoint();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// start the app async (non-blocking)
await app.RunAsync();
