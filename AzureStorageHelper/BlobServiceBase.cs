using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureStorageHelper.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AzureStorageHelper;

public abstract class BlobServiceBase<T> : IBlobService<T> where T : IBlob
{
    protected readonly ILogger<BlobServiceBase<T>> _logger;
    protected string Container { get; private set; }
    private readonly BlobStorageConfiguration _configuration;

    protected BlobServiceBase(ILogger<BlobServiceBase<T>> logger, IConfiguration configuration, string container)
    {
        _logger = logger;
        Container = container;
        _configuration = new BlobStorageConfiguration();
        new ConfigureFromConfigurationOptions<BlobStorageConfiguration>(
                configuration.GetSection("BlobStorageConfiguration"))
            .Configure(_configuration);
        if (_configuration == null || string.IsNullOrEmpty(_configuration.BlobConnectionString))
        {
            throw new ArgumentException("No BlobConnectionString defined! Blob won't be saved");
        }
    }

    public abstract Task<string> Upload(T blobInfo);

    protected async Task<string> UploadToBlobContainer(T blobInfo, byte[] stream)
    {
        _logger.LogDebug($"Uploading blob {blobInfo.Name} to container {Container}");
        var client = new BlobServiceClient(_configuration.BlobConnectionString);
        var container = client.GetBlobContainerClient(Container);

        var blob = container.GetBlobClient(blobInfo.GetFullBlobName());
        var blobHttpHeader = new BlobHttpHeaders();
        blobHttpHeader.ContentType = GetContentType(blobInfo);

        try
        {
            using var ms = new MemoryStream(stream);
            await blob.UploadAsync(ms, blobHttpHeader);
            ms.Close();
            if (blobInfo.Metadata!= null && blobInfo.Metadata.Any())
            {
                await blob.SetMetadataAsync(blobInfo.Metadata);
            }

            if (blobInfo.Tags != null && blobInfo.Tags.Any())
            {
                await blob.SetTagsAsync(blobInfo.Tags);
            }
            _logger.LogDebug($"Blob saved for {blobInfo.Name} to {blob.Uri.AbsoluteUri}");
            return blob.Uri.AbsoluteUri;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error saving blob for {blobInfo.Name}", ex);
        }

        return null;
    }

    public async Task<bool> DeleteByUrl(string blobUrl)
    {
        var path = $"{Container}/";
        var blobName = blobUrl.Substring(blobUrl.IndexOf(path, StringComparison.Ordinal) + path.Length);
        
        return await Delete(blobName);
    }

    public async Task<bool> Delete(string blobname)
    {
        _logger.LogDebug($"{blobname} Blob will be deleted");
        var client = new BlobServiceClient(_configuration.BlobConnectionString);
        var container = client.GetBlobContainerClient(Container);
        var blob = await container.DeleteBlobIfExistsAsync(blobname);
        return blob.Value;
    }

    public async Task<bool> Delete(KeyValuePair<string, string> tagKv)
    {
        _logger.LogDebug($"Blob with tag {tagKv.Value} will be deleted");
        var client = new BlobServiceClient(_configuration.BlobConnectionString);
        var container = client.GetBlobContainerClient(Container);
        var blobs = container.FindBlobsByTagsAsync($"{tagKv.Key}='{tagKv.Value}'");
        await foreach (var blob in blobs)
        {
            _logger.LogDebug($"Deleting blob {blob.BlobName}");
            var blobResult = await container.DeleteBlobIfExistsAsync(blob.BlobName);
        }

        return true;
    }

    public async Task<IEnumerable<BlobResponse>> GetAll(string? prefix = null)
    {
        var client = new BlobServiceClient(_configuration.BlobConnectionString);
        var container = client.GetBlobContainerClient(Container);
        var blobs = new List<BlobResponse>();
        await foreach (BlobItem blobItem in container.GetBlobsAsync(prefix: prefix))
        {
            blobs.Add(new BlobResponse(blobItem.Name, blobItem.Properties.LastModified));
        }

        return blobs;
    }
    
    public async Task<IEnumerable<BlobResponse>> GetByTag(KeyValuePair<string, string> tagKv)
    {
        var client = new BlobServiceClient(_configuration.BlobConnectionString);
        var container = client.GetBlobContainerClient(Container);
        var blobs = new List<BlobResponse>();
        var blobsByTag = container.FindBlobsByTagsAsync($"{tagKv.Key}='{tagKv.Value}'");
        await foreach (var blobItem in blobsByTag)
        {
            blobs.Add(new BlobResponse(blobItem.BlobName, null));
        }

        return blobs;
    }

    public async Task<Stream?> GetFileStream(string blobName)
    {
        var client = new BlobServiceClient(_configuration.BlobConnectionString);
        var container = client.GetBlobContainerClient(Container);
        var fileStream = new MemoryStream();
        var result = await container.GetBlobClient(blobName).DownloadToAsync(fileStream);
        if (result.IsError)
        {
            return null;
        }

        return fileStream;
    }

    private string GetContentType(IBlob blob)
    {
        switch (blob.GetFullBlobName().Substring(blob.GetFullBlobName().LastIndexOf(".")))
        {
            case ".html":
                return "text/html";
            case ".css":
                return "text/css";
            case ".js":
                return "application/javascript";
            case ".json":
                return "application/json";
            case ".png":
                return "image/png";
            case ".jpg":
                return "image/jpg";
            case ".pdf":
                return "application/pdf";
            case ".txt":
                return "text/plain";
            default:
                return blob.ContentType;
        }
    }
}