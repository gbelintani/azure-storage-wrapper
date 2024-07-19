namespace AzureStorageWrapper.Interfaces;

public interface IBlobWrapperService<in T> where T : BlobBase
{
    Task<string> Upload(T blobInfo);
    Task<IEnumerable<BlobResponse>> GetAll(string? prefix = null);
    Task<bool> DeleteByUrl(string blobUrl);
    Task<bool> Delete(string blobname);
    Task<Stream?> GetFileStream(string blobName);
    Task<IEnumerable<BlobResponse>> GetByTag(KeyValuePair<string, string> tagKv);
}