using Application.Infrastructure;
using Application.Services.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace Application.Services;

public static class ServiceInjection
{
    public static void AddServiceDependencies(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddDbContext(builder.Configuration.GetConnectionString("ConnectionString"));
        services.AddAuthentication(builder.Configuration);
        services.AddInfrastructure();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    public static void AddApplicationLogging(this ILoggingBuilder loggingBuilder)
    {
        loggingBuilder.ClearProviders();

        // Create the logger configuration
        ILogger logger = new LoggerConfiguration()
            .WriteTo.File("app-logs", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Override("warning", LogEventLevel.Information)
            .CreateLogger();

        Log.Logger = logger;
        loggingBuilder.AddSerilog();
    }
}
