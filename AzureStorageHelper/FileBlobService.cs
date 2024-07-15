using AzureStorageHelper.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AzureStorageHelper;

public class FileBlobService : BlobServiceBase<IFileBlob>
{
    private const string FILE_CONTAINER = "docs";

    public FileBlobService(ILogger<FileBlobService> logger, IConfiguration configuration) : base(logger, configuration,
        FILE_CONTAINER)
    {
    }

    public override Task<string> Upload(IFileBlob blobInfo)
    {
        using var memoryStream = new MemoryStream();
        blobInfo.Stream.CopyTo(memoryStream);
        return UploadToBlobContainer(blobInfo, memoryStream.ToArray());
    }
}