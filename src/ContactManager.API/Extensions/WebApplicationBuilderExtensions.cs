using System.Reflection;
using ContactManager.Contract.Repositories;
using ContactManager.Data.Context;
using ContactManager.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void SetupDb(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("PostgreSQL");

        builder.ConfigureRepositories();

        builder.Services.AddDbContext<ContactsDbContext>(options =>
        {
            options.UseNpgsql(connectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly(typeof(ContactsDbContext).GetTypeInfo().Assembly.GetName().Name);
                optionsBuilder.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), null);
            });
        });
    }
    
    private static void ConfigureRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IContactRepository, ContactRepository>();
    }
}