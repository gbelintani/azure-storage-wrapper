namespace AzureStorageHelper.Interfaces;

public interface IBlob
{
    public string Name { get; }
    public string Folder { get; }
    public Stream Stream { get; }
    public string Extension { get; }
    public string FormatFileName => Name;
    public string ContentType { get; }
    public IDictionary<string, string> Metadata { get;  }
    public IDictionary<string, string> Tags { get; }
    
    public string GetFullBlobName()
    {
        return $"{Folder}/{FormatFileName}";
    }
    
}