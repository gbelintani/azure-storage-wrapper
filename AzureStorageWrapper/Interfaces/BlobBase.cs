namespace AzureStorageWrapper.Interfaces;

public abstract class BlobBase : IBlob
{
    public string Name { get; set; }
    public string Folder { get; set; }
    public Stream Stream { get; set; }
    public string Extension { get; set; }
    public virtual string ContentType { get; set; }
    public IDictionary<string, string>? Metadata { get; set; }
    public IDictionary<string, string>? Tags { get; set; }
    public virtual WrapperBlobType Type { get; }

    public string GetFullBlobName()
    {
        return $"{Folder}/{Name}.{Extension}";
    }
}