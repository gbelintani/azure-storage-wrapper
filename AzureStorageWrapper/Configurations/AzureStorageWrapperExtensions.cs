using AzureStorageWrapper.DefaultBlobs;
using AzureStorageWrapper.Factories;
using AzureStorageWrapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AzureStorageWrapper.Configurations;

public static class AzureStorageWrapperExtensions
{
    public static void AddAzureStorageWrapper(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("ConnectionString is required");
        }
        AddAzureStorageWrapper(services, options => { options.ConnectionString = connectionString; });
    }

    public static void AddAzureStorageWrapper(this IServiceCollection services,
        Action<AzureStorageWrapperOptions> configure)
    {
        services.Configure(configure);
        services.AddScoped<IBlobServiceFactory, BlobServiceFactory>();
    }
}
