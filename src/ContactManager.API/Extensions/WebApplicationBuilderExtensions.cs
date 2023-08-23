using System.Reflection;
using ContactManager.API.Configurations;
using ContactManager.Contract.Repositories;
using ContactManager.Contract.Services;
using ContactManager.Core.Services;
using ContactManager.Data.Context;
using ContactManager.Data.Entities;
using ContactManager.Data.Repositories;
using ContactManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Models.ContactManager;
using Serilog;

namespace ContactManager.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void SetupServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IContactService, ContactService>();
    }
    
    public static void SetupMapper(this WebApplicationBuilder builder)
    {
        var notificationConfiguration = new ContactNotificationsConfiguration();
        builder.Configuration.GetSection(nameof(ContactNotificationsConfiguration)).Bind(notificationConfiguration);

        builder.Services.AddAutoMapper(config =>
        {
            config.CreateMap<APIContactModel, ContactModel>().ReverseMap();
            config.CreateMap<APIActionContactModel, UpdateContactModel>();
            config.CreateMap<APIActionContactModel, ContactModel>();
            config.CreateMap<UpdateContactModel, ContactModel>();
            
            config.CreateMap<ContactModel, Contact>()
                .ReverseMap();

            config.CreateMap<Contact, ContactModel>()
                .ForMember(contactModel => contactModel.NotifyHasBirthdaySoon, opt =>
                {
                    opt.MapFrom(src =>
                        src.Birthday.HasValue && Math.Abs(DateTime.UtcNow.DayOfYear - src.Birthday.Value.DayOfYear) <=
                        notificationConfiguration.BirthdaySoonDays);
                });
        });
    }

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