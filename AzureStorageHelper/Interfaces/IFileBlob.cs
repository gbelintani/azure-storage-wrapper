namespace AzureStorageHelper.Interfaces;

public interface IFileBlob : IBlob
{
    public new string ContentType => "application/octet-stream";
    public new string GetFullBlobName()
    {
        return $"{Folder}/{FormatFileName}";
    }
}