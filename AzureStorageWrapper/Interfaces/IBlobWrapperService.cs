namespace AzureStorageWrapper.Interfaces;

public interface IBlobWrapperService
{
    Task<string> Upload(IBlob blobInfo);
    Task<IEnumerable<BlobResponse>> GetAll(WrapperBlobType type, string? prefix = null);
    Task<bool> DeleteByUrl(WrapperBlobType type, string blobUrl);
    Task<bool> Delete(WrapperBlobType type, string blobname);
    Task<Stream?> GetFileStream(WrapperBlobType type, string blobName);
    Task<IEnumerable<BlobResponse>> GetByTag(WrapperBlobType type, KeyValuePair<string, string> tagKv);
}