using AzureStorageWrapper.Configurations;
using AzureStorageWrapper.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AzureStorageWrapper.Factories;

/// <summary>
/// BlobServiceFactory class for creating a BlobWrapperService.
/// You can set this up in the DI container by calling AddAzureStorageWrapper in the Startup.cs file.
/// </summary>
public class BlobServiceFactory : IBlobServiceFactory
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly IOptions<AzureStorageWrapperOptions> _options;

    public BlobServiceFactory(ILoggerFactory loggerFactory, IOptions<AzureStorageWrapperOptions> options)
    {
        _loggerFactory = loggerFactory;
        _options = options;
    }

    /// <summary>
    /// Creates a new instance of the BlobWrapperService class.
    /// </summary>
    /// <param name="blobContainerOptions"> BlobContainerOptions for the BlobWrapperService. </param>
    /// <returns> A new instance of the BlobWrapperService class. </returns>
    /// <exception cref="ArgumentException"> Thrown when the connection string or blob container options are not valid. </exception>
    public IBlobWrapperService Create(BlobContainerOptions blobContainerOptions)
    {
        if (!_options.Value.IsValid())
        {
            throw new ArgumentException("No ConnectionString defined for AzureStorageWrapper");
        }
        if(!blobContainerOptions.IsValid())
        {
            throw new ArgumentException("BlobContainerOptions is required");
        }
        return new BlobWrapperService(_loggerFactory.CreateLogger<BlobWrapperService>(), _options, blobContainerOptions);
    }
}