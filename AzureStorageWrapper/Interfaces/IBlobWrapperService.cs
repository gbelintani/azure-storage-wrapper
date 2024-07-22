namespace AzureStorageWrapper.Interfaces;

public interface IBlobWrapperService
{
    Task<string> Upload(BlobBase blobInfo, CancellationToken ct = default);
    Task<IEnumerable<BlobResponse>> GetAll(string? prefix = null);
    Task<bool> DeleteByUrl(string blobUrl);
    Task<bool> Delete(string blobName);
    Task<bool> Delete(KeyValuePair<string, string> tagKv);
    Task<Stream?> GetFileStream(string blobName);
    Task<IEnumerable<BlobResponse>> GetByTag(KeyValuePair<string, string> tagKv);
}