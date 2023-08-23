using System.Reflection;
using ContactManager.API.Configurations;
using ContactManager.Contract.Repositories;
using ContactManager.Data.Context;
using ContactManager.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ContactManager.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void SetupDb(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("PostgreSQL")!;

        builder.SetupRepositories();

        builder.Services.AddDbContext<ContactsDbContext>(options =>
        {
            options.UseNpgsql(connectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly(typeof(ContactsDbContext).GetTypeInfo().Assembly.GetName().Name);
                optionsBuilder.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), null);
            });
        });
    }
    
    public static void SetupSerilog(this WebApplicationBuilder builder)
    {
        var serilogConfiguration = new SerilogConfiguration();

        builder.Configuration.GetSection(nameof(SerilogConfiguration)).Bind(serilogConfiguration);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
                Path.Combine("/", serilogConfiguration.LogsPath, serilogConfiguration.LogFileName),
                rollingInterval: serilogConfiguration.RollingInterval,
                fileSizeLimitBytes: serilogConfiguration.FileSizeLimitBytes,
                retainedFileCountLimit: serilogConfiguration.RetainedFileCountLimit,
                rollOnFileSizeLimit: serilogConfiguration.RollOnFileSizeLimit,
                shared: serilogConfiguration.Shared)
            .CreateLogger();

        builder.Services.AddSingleton(serilogConfiguration);
    }
    
    private static void SetupRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IContactRepository, ContactRepository>();
    }
}