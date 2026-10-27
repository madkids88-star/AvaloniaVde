using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using AvaloniaVde.Models.SettingsApp;
using AvaloniaVde.Services.Db.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaVde.Services.Db;

public static class DbServiceCollectionExtensions
{
    private const string SettingsResourceName = "AvaloniaVde.appsettings.json";

    public static IServiceCollection AddDbService(this IServiceCollection services)
    {
        var configuration = BuildConfiguration();
        return services.AddDbService(configuration);
    }

    public static IServiceCollection AddDbService(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration
            .GetSection(DatabaseOptions.SectionName)
            .Get<DatabaseOptions>() ?? new DatabaseOptions();

        services.AddSingleton(options);
        services.AddSingleton<DbService>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<DbInitializer>();
        return services;
    }

    private static IConfiguration BuildConfiguration()
    {
        var assembly = typeof(DbServiceCollectionExtensions).Assembly;
        var stream = assembly.GetManifestResourceStream(SettingsResourceName)
            ?? FindSettingsStream(assembly);

        var builder = new ConfigurationBuilder();
        if (stream != null)
            builder.AddJsonStream(stream);

        return builder.Build();
    }

    private static Stream? FindSettingsStream(Assembly assembly)
    {
        foreach (var name in assembly.GetManifestResourceNames())
        {
            if (name.EndsWith("appsettings.json", StringComparison.OrdinalIgnoreCase))
                return assembly.GetManifestResourceStream(name);
        }

        return null;
    }
}
