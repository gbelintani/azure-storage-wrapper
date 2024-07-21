using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureStorageWrapper.Configurations;
using AzureStorageWrapper.Factories;
using AzureStorageWrapper.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AzureStorageWrapper;

/// <summary>
/// BlobWrapperService class for interacting with the Azure Storage Blob Service.
/// </summary>
public class BlobWrapperService : IBlobWrapperService
{
    private readonly ILogger<BlobWrapperService> _logger;
    private readonly BlobContainerOptions _blobContainerOptions;
    private readonly AzureStorageWrapperOptions _options;

    /// <summary>
    /// Creates a new instance of the BlobWrapperService class. Should be instantiated using the <see cref="BlobServiceFactory"/> .
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    /// <param name="blobContainerOptions"> <see cref="BlobContainerOptions"/> </param>
    /// <exception cref="ArgumentException"> Thrown when the connection string or blob container options are not valid. </exception>
    public BlobWrapperService(ILogger<BlobWrapperService> logger, IOptions<AzureStorageWrapperOptions> options,
        BlobContainerOptions blobContainerOptions)
    {
        _logger = logger;
        _blobContainerOptions = blobContainerOptions;
        if (!options.Value.IsValid())
        {
            throw new ArgumentException("No ConnectionString defined for AzureStorageWrapper");
        }

        if (!blobContainerOptions.IsValid())
        {
            throw new ArgumentException("BlobContainer is required");
        }

        _options = options.Value;
    }

    /// <summary>
    /// Uploads a blob to the Azure Storage Blob Service. The blob type is determined by the <see cref="BlobBase.Type"/> property of the blob.
    /// If the blob type is <see cref="WrapperBlobType.File"/>, the <see cref="FileBlobBase.ContentType"/> property will be used to set the content type of the blob.
    /// If the blob type is <see cref="WrapperBlobType.Image"/>, the <see cref="ImageBlobBase.ContentType"/> property will be used to set the content type of the blob. And the <see cref="ImageBlobBase.ImageStrategy"/> property will be used to process the image before uploading.
    /// </summary>
    /// <param name="blobInfo"> Blob to be uploaded. </param>
    /// <param name="ct"> Cancellation token. </param>
    /// <returns> The URL of the uploaded blob. </returns>
    /// <exception cref="ArgumentException"> Thrown when the blob type is not supported. </exception>
    public Task<string> Upload(BlobBase blobInfo, CancellationToken ct = default)
    {
        return blobInfo.Type switch
        {
            WrapperBlobType.File => Upload(blobInfo as FileBlobBase, ct),
            WrapperBlobType.Image => Upload(blobInfo as ImageBlobBase, ct),
            _ => throw new ArgumentException("Blob type not supported")
        };
    }

    /// <summary>
    /// Deletes a blob by its URL.
    /// </summary>
    /// <param name="blobUrl"> URL of the blob to be deleted. </param>
    /// <returns> True if the blob was deleted, false otherwise. </returns>
    public async Task<bool> DeleteByUrl(string blobUrl)
    {
        var path = $"{_blobContainerOptions.GetFormattedName()}/";
        var blobName = blobUrl.Substring(blobUrl.IndexOf(path, StringComparison.Ordinal) + path.Length);

        return await Delete(blobName);
    }

    /// <summary>
    /// Deletes a blob by its name. 
    /// </summary>
    /// <param name="blobName"> Name of the blob to be deleted. </param>
    /// <returns> True if the blob was deleted, false otherwise. </returns>
    public async Task<bool> Delete(string blobName)
    {
        _logger.LogDebug($"{blobName} Blob will be deleted");
        var client = new BlobServiceClient(_options.ConnectionString);
        var container = client.GetBlobContainerClient(_blobContainerOptions.GetFormattedName());
        var blob = await container.DeleteBlobIfExistsAsync(blobName);
        return blob.Value;
    }

    /// <summary>
    /// Gets all blobs in the container. If a prefix is provided, only blobs with names that start with the prefix will be returned.
    /// </summary>
    /// <param name="prefix"> Prefix of the blobs to be returned. If null, all blobs will be returned. </param>
    /// <returns> A list of <see cref="BlobResponse"/> objects representing the blobs. </returns>
    public async Task<IEnumerable<BlobResponse>> GetAll(string? prefix = null)
    {
        var client = new BlobServiceClient(_options.ConnectionString);
        var container = client.GetBlobContainerClient(_blobContainerOptions.GetFormattedName());
        var blobs = new List<BlobResponse>();
        await foreach (var blobItem in container.GetBlobsAsync(prefix: prefix))
            blobs.Add(new BlobResponse(blobItem.Name, blobItem.Properties.LastModified));

        return blobs;
    }

    /// <summary>
    /// Gets all blobs with a specific tag. The tag is specified as a KeyValue
    /// </summary>
    /// <param name="tagKv"> KeyValue pair representing the tag. </param>
    /// <returns> A list of <see cref="BlobResponse"/> objects representing the blobs. </returns>
    public async Task<IEnumerable<BlobResponse>> GetByTag(KeyValuePair<string, string> tagKv)
    {
        var client = new BlobServiceClient(_options.ConnectionString);
        var container = client.GetBlobContainerClient(_blobContainerOptions.GetFormattedName());
        var blobs = new List<BlobResponse>();
        var blobsByTag = container.FindBlobsByTagsAsync($"{tagKv.Key}='{tagKv.Value}'");
        await foreach (var blobItem in blobsByTag) blobs.Add(new BlobResponse(blobItem.BlobName, null));

        return blobs;
    }

    /// <summary>
    /// Gets a stream of the blob with the specified name.
    /// </summary>
    /// <param name="blobName"> Name of the blob. </param>
    /// <returns> A stream of the blob with the specified name. </returns>
    public async Task<Stream?> GetFileStream(string blobName)
    {
        var client = new BlobServiceClient(_options.ConnectionString);
        var container = client.GetBlobContainerClient(_blobContainerOptions.GetFormattedName());
        var fileStream = new MemoryStream();
        var result = await container.GetBlobClient(blobName).DownloadToAsync(fileStream);
        if (result.IsError) return null;

        return fileStream;
    }

    private Task<string> Upload(FileBlobBase blobInfo, CancellationToken ct = default)
    {
        using var memoryStream = new MemoryStream();
        blobInfo.Stream.CopyTo(memoryStream);
        return UploadToBlobContainer(blobInfo, memoryStream.ToArray(), ct);
    }

    private async Task<string> Upload(ImageBlobBase blobInfo, CancellationToken ct = default)
    {
        byte[]? finalImage;
        if (blobInfo.ImageStrategy != null)
        {
            _logger.LogDebug($"Image will be processed and saved for {blobInfo.Name}");
            finalImage = await ImageProcessor.ProcessAsync(blobInfo, ct);
            _logger.LogDebug($"Image processed for {blobInfo.Name}");
        }
        else
        {
            var streamEnd = Convert.ToInt32(blobInfo.Stream.Length);
            var buffer = new byte[streamEnd];
            var readAsync = await blobInfo.Stream.ReadAsync(buffer, 0, streamEnd, ct);
            finalImage = buffer;
        }

        if (finalImage == null)
        {
            throw new ArgumentException("Image processing failed, no bytes returned");
        }

        return await UploadToBlobContainer(blobInfo, finalImage, ct);
    }

    private async Task<string> UploadToBlobContainer(IBlob blobInfo, byte[] stream, CancellationToken ct = default)
    {
        _logger.LogDebug($"Uploading blob {blobInfo.Name} to container {_blobContainerOptions.GetFormattedName()}");
        var client = new BlobServiceClient(_options.ConnectionString);

        var container = client.GetBlobContainerClient(_blobContainerOptions.GetFormattedName());
        var containerExists = await container.ExistsAsync(ct);
        if (!containerExists.HasValue || !containerExists.Value)
        {
            _logger.LogDebug($"Creating container {_blobContainerOptions.GetFormattedName()}");
            var response = await client.CreateBlobContainerAsync(_blobContainerOptions.GetFormattedName(),
                _blobContainerOptions.DefaultPublicAccessType, cancellationToken: ct);
            if (!response.HasValue)
            {
                throw new Exception("Error creating container");
            }

            container = response.Value;
        }

        var blob = container.GetBlobClient(blobInfo.GetFullBlobName());
        var blobHttpHeader = new BlobHttpHeaders
        {
            ContentType = GetContentType(blobInfo)
        };

        try
        {
            using var ms = new MemoryStream(stream);
            await blob.UploadAsync(ms, blobHttpHeader, cancellationToken: ct);
            ms.Close();
            if (blobInfo.Metadata != null && blobInfo.Metadata.Any()) await blob.SetMetadataAsync(blobInfo.Metadata, cancellationToken: ct);

            if (blobInfo.Tags != null && blobInfo.Tags.Any()) await blob.SetTagsAsync(blobInfo.Tags, cancellationToken: ct);
            _logger.LogDebug($"Blob saved for {blobInfo.Name} to {blob.Uri.AbsoluteUri}");
            return blob.Uri.AbsoluteUri;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error saving blob for {blobInfo.Name}", ex);
            throw;
        }
    }

    /// <summary>
    /// Deletes all blobs with a specific tag.
    /// </summary>
    /// <param name="tagKv"> KeyValue pair representing the tag. </param>
    /// <returns> True if all blobs with the tag were deleted, false otherwise. </returns>
    public async Task<bool> Delete(KeyValuePair<string, string> tagKv)
    {
        _logger.LogDebug($"Blob with tag {tagKv.Value} will be deleted");
        var client = new BlobServiceClient(_options.ConnectionString);
        var container = client.GetBlobContainerClient(_blobContainerOptions.GetFormattedName());
        var blobs = container.FindBlobsByTagsAsync($"{tagKv.Key}='{tagKv.Value}'");
        await foreach (var blob in blobs)
        {
            _logger.LogDebug($"Deleting blob {blob.BlobName}");
            var blobResult = await container.DeleteBlobIfExistsAsync(blob.BlobName);
        }

        return true;
    }

    private static string GetContentType(IBlob blob)
    {
        return blob.GetFullBlobName()
                .Substring(blob.GetFullBlobName().LastIndexOf(".", StringComparison.Ordinal)) switch
            {
                ".html" => "text/html",
                ".css" => "text/css",
                ".js" => "application/javascript",
                ".json" => "application/json",
                ".png" => "image/png",
                ".jpg" => "image/jpg",
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                _ => blob.ContentType
            };
    }
}