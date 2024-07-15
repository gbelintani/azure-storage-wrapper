using AzureStorageWrapper;
using AzureStorageWrapper.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AzureStorageWrapper;

public class FileBlobService : BlobServiceBase<IFileBlob>
{
    private const string DEFAULT_FILE_CONTAINER = "docs";

    public FileBlobService(ILogger<FileBlobService> logger, IConfiguration configuration, string? container = null)
        : base(logger, configuration, container ?? DEFAULT_FILE_CONTAINER)
    {
    }

    public override Task<string> Upload(IFileBlob blobInfo)
    {
        using var memoryStream = new MemoryStream();
        blobInfo.Stream.CopyTo(memoryStream);
        return UploadToBlobContainer(blobInfo, memoryStream.ToArray());
    }
}