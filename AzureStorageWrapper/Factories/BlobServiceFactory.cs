using AzureStorageWrapper.Configurations;
using AzureStorageWrapper.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AzureStorageWrapper.Factories;

public class BlobServiceFactory : IBlobServiceFactory
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly IOptions<AzureStorageWrapperOptions> _options;

    public BlobServiceFactory(ILoggerFactory loggerFactory, IOptions<AzureStorageWrapperOptions> options)
    {
        _loggerFactory = loggerFactory;
        _options = options;
    }

    public IBlobWrapperService Create(BlobContainer blobContainer)
    {
        if (!_options.Value.IsValid())
        {
            throw new ArgumentException("No ConnectionString defined for AzureStorageWrapper");
        }
        if(!blobContainer.IsValid())
        {
            throw new ArgumentException("BlobContainer is required");
        }
        return new BlobWrapperService(_loggerFactory.CreateLogger<BlobWrapperService>(), _options, blobContainer);
    }
}