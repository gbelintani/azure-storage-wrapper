using AzureStorageWrapper.DefaultBlobs;
using AzureStorageWrapper.Factories;
using AzureStorageWrapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AzureStorageWrapper.Configurations;

/// <summary>
/// Extension methods for setting up AzureStorageWrapper DependencyInjection.
/// </summary>
public static class AzureStorageWrapperExtensions
{
    /// <summary>
    /// Adds the AzureStorageWrapper to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the AzureStorageWrapper to.</param>
    /// <param name="connectionString">The connection string to the Azure Storage Account. You can use the Azure Portal to get the connection string.</param>
    /// <exception cref="ArgumentException"> Thrown when the connection string is null or empty. </exception>
    public static void AddAzureStorageWrapper(this IServiceCollection services, string? connectionString)
    {
        AddAzureStorageWrapper(services, options => { options.ConnectionString = connectionString; });
    }

    private static void AddAzureStorageWrapper(this IServiceCollection services,
        Action<AzureStorageWrapperOptions> configure)
    {
        services.Configure(configure);
        services.AddLogging();
        services.AddScoped<IBlobServiceFactory, BlobServiceFactory>();
    }
}