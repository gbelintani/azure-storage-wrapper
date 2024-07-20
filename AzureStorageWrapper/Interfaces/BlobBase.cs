namespace AzureStorageWrapper.Interfaces;

public abstract class BlobBase : IBlob
{
    public string Name { get; set; }
    public string? Folder { get; set; }
    public Stream Stream { get; set; }
    public string Extension { get; set; }
    public virtual string ContentType { get; set; }
    public IDictionary<string, string>? Metadata { get; set; }
    public IDictionary<string, string>? Tags { get; set; }
    public virtual WrapperBlobType Type { get; }

    protected BlobBase(string name, string extension, Stream stream, string? folder = null)
    {
        Name = name;
        Extension = extension;
        Stream = stream;
        Folder = folder ?? "";
    }
    
    public string GetFullBlobName()
    {
        if (string.IsNullOrEmpty(Folder))
        {
            return $"{Name}.{Extension}";
        }
        return $"{Folder}/{Name}.{Extension}";
    }
}